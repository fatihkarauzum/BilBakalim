app.controller('indexController', ['$scope', 'indexFactory', ($scope, indexFactory) => {
    $scope.messages = [ ];
    $scope.players = { };
    $scope.questions = [ ];
    $scope.showQuestions = [ ];
    $scope.answersCount = 0;
    $scope.oyuncular = [ ];
    $scope.playersCount = 0;
    $scope.distance;
    $scope.point;
    var point;
    var distance = 0;
    var passingTime;
    var score;
    var first = 1;
    var x;
    var y;
    var audioCount = 0;
    $scope.sinifId;
    var audio = new Audio('/sounds/main.mp4');

    $scope.init = () => {
        initSocket();
    };

    Object.size = function(obj) {
        var size = 0, key;
        for (key in obj) {
            if (obj.hasOwnProperty(key)) size++;
        }
        return size;
    };

    correctAnswer = (answer) => {
        if(answer == 'Cevap1'){
            $('#correctA').html('<img src="/SecimSayilari/images/correct.png" width="24%">');
        }
        if(answer == 'Cevap2'){
            $('#correctB').html('<img src="/SecimSayilari/images/correct.png" width="24%">');
        }
        if(answer == 'Cevap3'){
            $('#correctC').html('<img src="/SecimSayilari/images/correct.png" width="24%">');
        }
        if(answer == 'Cevap4'){
            $('#correctD').html('<img src="/SecimSayilari/images/correct.png" width="24%">');
        }
    }

    function counter (){
        var parentWidth = $('#parent').width();
        var azaltma = $('#parent').width()/$scope.distance;
        var yeniDeger = parentWidth;
        var distance = $scope.distance;

        console.log(parentWidth, yeniDeger);
        y = setInterval(() => {
            distance -= 1;

            yeniDeger = yeniDeger - azaltma;
            console.log(yeniDeger);
            $('.Loading-progress').animate({
                width: yeniDeger
            }, 'slow');
            if (distance < 0) {
                $('.Loading-progress').width($('#parent').width());
                clearInterval(y);
            }
        }, 1000);
    }

    async function initSocket(){
        const connectionOptions = {
            reconnectionAttempts: 3,
            reconnectionDelay: 600
        };

        const socket = await indexFactory.connectSocket("http://localhost:3000", connectionOptions)
            try{
                socket.emit('registerRoom', { roomId: $('#id').text() });

                socket.on('newUserConnect', (data) => {
                    $scope.players[data.id] = data;
                    $scope.playersCount += 1;
                    $scope.$apply();
                    
                    $('#start').removeAttr("disabled");
                });

                socket.on('log', (data) => {
                    const messagesData = {
                        type: {
                            code: 0, //server or user messages: type of code
                        }, //info
                        message: data.messages
                    };
                    
                    audio.play();
                    $('#realId').html(data.realRoomId);
                    $('#realIdShow').html(data.realRoomId);
                    $scope.sinifId = data.realRoomId;
                    $scope.messages.push(messagesData);
                    $scope.$applyAsync();
                });

                socket.on('correctAnswer', (data) => {
                    Object.keys($scope.players).forEach(item => {
                        if(item == data.mineId){
                            if(data.award){
                                score = 2000;
                            }
                            else{
                                score = 1000;
                            }

                            $scope.players[item].score += (score - (data.timeLeft * 25));
                            $scope.$applyAsync();
                            
                            //console.log($scope.players);
                        }
                    });
                });

                socket.on('answersCount', (data) => {
                    $scope.answersCount = data.answerCount;
                    $scope.$applyAsync();

                    console.log(Object.size($scope.players));
                    if($scope.answersCount == Object.size($scope.players)){
                        clearInterval(x);
                        $scope.showQuestions.pop();
                        $scope.$applyAsync();
                        $('#middle').show();
                        audio.pause();
                        audio = new Audio('/sounds/timeUp.mp3');
                        audio.play();
                        clearInterval(y);
                        $('.Loading-progress').width($('#parent').width());
                        $('#remaining').hide();
                        $('#answersCount').hide();
                        if(first == 1){
                            point = 1;
                            $scope.point = point;
                            $scope.$apply();
                        }
                        socket.emit('showMiddle', { realRoomId: $('#realId').text() });
                    }
                });

                socket.on('showMiddle', (data) => {
                    $('#choosingA').html(data.choosingAnswers.Cevap1);
                    $('#choosingB').html(data.choosingAnswers.Cevap2);
                    $('#choosingC').html(data.choosingAnswers.Cevap3);
                    $('#choosingD').html(data.choosingAnswers.Cevap4);

                    var rate = 0;

                    if(data.choosingAnswers.Cevap1 != 0){
                        rate = (data.choosingAnswers.Cevap1/$scope.playersCount)*70;
                        $('#aRate').height(($('#aRate').parent().height() * rate) / 100)
                    }

                    if(data.choosingAnswers.Cevap2 != 0){
                        rate = (data.choosingAnswers.Cevap2/$scope.playersCount)*70;
                        $('#bRate').height(($('#bRate').parent().height() * rate) / 100)
                    }

                    if(data.choosingAnswers.Cevap3 != 0){
                        rate = (data.choosingAnswers.Cevap3/$scope.playersCount)*70;
                        $('#cRate').height(($('#cRate').parent().height() * rate) / 100)
                    }

                    if(data.choosingAnswers.Cevap4 != 0){
                        rate = (data.choosingAnswers.Cevap4/$scope.playersCount)*70;
                        $('#dRate').height(($('#dRate').parent().height() * rate) / 100)
                    }
                });

                $('#start').on('click', () => {
                    $('#oturum').hide();
                    audio.pause();
                    
                    if(audioCount=0){
                        audio = new Audio('/sounds/questions1.mp3');
                        audioCount = 1;
                        
                    }
                    else{
                        audio = new Audio('/sounds/questions.mp3');
                        audioCount = 0;

                    }

                    socket.emit('start', { roomId: $('#id').text(), realRoomId: $('#realId').text() });
                    socket.emit('delay');
                    socket.emit('delay');
                    $('#remaining').show();
                });

                $scope.passScores = () => {
                    if(point == $scope.questions.length){
                        audio = new Audio('/sounds/finish.wav');
                        audio.play();
                    }

                    $('#middle').hide();

                    Object.keys($scope.players).forEach(element => {
                        $scope.oyuncular.push($scope.players[element]);
                        $scope.$applyAsync();
                    });

                    $('#correctA').html('');
                    $('#correctB').html('');
                    $('#correctC').html('');
                    $('#correctD').html('');
                    $('#score').show();
                }

                $('#pass').on('click', () => {
                    if(audioCount == 0){
                        audio = new Audio('/sounds/questions1.mp3');
                        audioCount = 1;
                    }
                    else{
                        audio = new Audio('/sounds/questions.mp3');
                        audioCount = 0;
                    }

                    $('#aRate').height(($('#aRate').parent().height() * 3) / 100);
                    $('#bRate').height(($('#bRate').parent().height() * 3) / 100);
                    $('#cRate').height(($('#cRate').parent().height() * 3) / 100);
                    $('#dRate').height(($('#dRate').parent().height() * 3) / 100);
                    $scope.answersCount = 0;
                    audio.play();
                    first = 0;
                    $('#remaining').show();
                    if(point < $scope.questions.length){
                        socket.emit('pass', { roomId: $('#id').text(), realRoomId: $('#realId').text() })

                        $('#score').hide();
                        if(point <= $scope.questions.length){
                            $scope.showQuestions.push($scope.questions[point]);
                            $scope.$applyAsync();
                            $('#SecimSoru').html($scope.questions[point].Soru);
                            distance = $scope.showQuestions[0].Sure;
                            $scope.distance = $scope.showQuestions[0].Sure;
                            $scope.$apply();
                            correctAnswer($scope.showQuestions[0].DogruCevap);

                            counter();

                            x = setInterval(() => {
                                $('#remaining').html(distance);
                                distance -= 1;
                                passingTime = $scope.showQuestions[0].Sure - distance;
                                
                                if (distance < 0) {
                                    clearInterval(x);
                                    $scope.showQuestions.pop();
                                    $scope.$applyAsync();
                                    $('#middle').show();
                                    audio.pause();
                                    audio = new Audio('/sounds/timeUp.mp3');
                                    audio.play();
                                    
                                    $('#remaining').hide();
                                    $('#answersCount').hide();

                                    socket.emit('showMiddle', { realRoomId: $('#realId').text() });
                                }
                            }, 1000);
                            point++;
                            $scope.point = point;
                            $scope.$apply();
                        }
                        $scope.$applyAsync();

                        socket.emit('answersCountReset', { realRoomId: $('#realId').text() });
                    }
                    else{
                        
                    } 
                });

                socket.on('questions', (data) => {
                    var array = data;
                    array.forEach(element => {
                        if(element.realRoomId == $('#realId').text()) $scope.questions.push(element);
                    });
                    $scope.$applyAsync();

                    $scope.showQuestions.push($scope.questions[0]);
                    $scope.$applyAsync();
                    $('#SecimSoru').html($scope.questions[0].Soru);
                    audio.play();
                    distance = $scope.showQuestions[0].Sure;
                    $scope.distance = $scope.showQuestions[0].Sure;
                    $scope.$apply();

                    counter();
                    x = setInterval(() => {
                        $('#remaining').html(distance);
                        distance -= 1;
                        passingTime = $scope.showQuestions[0].Sure - distance;
                        correctAnswer($scope.showQuestions[0].DogruCevap);
                        if (distance < 0) {
                            $('.Loading-progress').width($('#parent').css.width);
                            clearInterval(x);
                            $scope.showQuestions.pop();
                            $scope.$applyAsync();
                            $('#middle').show();
                            $('#remaining').hide();
                            $('#answersCount').hide();
                            audio.pause();
                            audio = new Audio('/sounds/timeUp.mp3');
                            audio.play();

                            socket.emit('showMiddle', { realRoomId: $('#realId').text() });
                            point=1;
                            $scope.point = point;
                            $scope.$apply();
                        }
                    }, 1000);

                    $('#start').hide();
                });

                socket.on('oyuncu', (data) => {
                    delete $scope.players[data.oyuncuId];
                    $scope.$apply();
                });

            }
            catch(err){
                console.log(err);
            }
        }
}]);