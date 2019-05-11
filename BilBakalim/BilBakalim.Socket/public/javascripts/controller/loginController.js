app.controller('loginController', ['$scope', 'loginFactory', ($scope, loginFactory) => {  
    $scope.messages = [ ];
    $scope.players = { };
    $scope.questions = [ ];
    $scope.showQuestions = [ ];
    $scope.answersCount;
    $scope.oyuncular = [ ];
    $scope.playersOfScore = 0;
    var point;
    var distance;
    var passingTime;
    var score;
    var timeUp = 0;
    var first = 1;
    var x;
    const scoreMiddle = [ ];

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

    async function initSocket(){
        const connectionOptions = {
            reconnectionAttempts: 3,
            reconnectionDelay: 600
        };
    
        const socket = await loginFactory.connectSocket("http://localhost:3000", connectionOptions)
            try{
                $('#loginButton').on('click', () => {
                    socket.emit('joinRoom', { roomId: $('#id').val() });
                });

                $('#send').on('click', () => {
                    socket.emit('newUser', { roomId: $('#id').val(), username: $('#inputName').val() });
                });

                $scope.Cevap = (cevap) => {
                    socket.emit('choosingAnswer', { roomId: $('#id').val(), answer: cevap });

                    $('#middleSub').show();
                    $('#middleSub1').show();
                    socket.emit('delay');
                    
                    answers = ['Cevap1', 'Cevap2', 'Cevap3', 'Cevap4'];
                    answers.forEach(element => {
                        $('#' + element).attr('ng-click', 'empty()');
                        $('#' + element).css( "cursor", "default" );
                    });
            
                    $('#' + cevap).css( "opacity", "0.5" );
                    questionID = $('.questionID').attr('id');
                    $scope.questions.forEach(element=>{
                        if(element.ID == questionID){
                            if(element.DogruCevap == cevap){
                                $('#correct').show();
                                socket.emit('correctAnswer', { mineId: $('#mineId').text(), roomId: $('#id').val(), timeLeft: passingTime, award: $scope.showQuestions[0].Odul });
                                timeUp = 1;
                            }
                            else{
                                $('#wrong').show();
                                timeUp = 1;
                            }
                        }
                    });

                    socket.emit('answersCount', { roomId: $('#id').val() });
                };

                socket.on('correctAnswer', (data) => {
                    Object.keys($scope.players).forEach(item => {
                        if(item == data.mineId){
                            if(data.award){
                                score = 2000;
                            }
                            else{
                                score = 1000;
                            }

                            scoreMiddle[0] = score - data.timeLeft * 25;
                            $scope.players[item].score += (score - data.timeLeft * 25);
                            $scope.$applyAsync();
                            //console.log($scope.players);
                        }
                    });
                });

                socket.on('meCorrect', (data) => { 
                    socket.emit('score', { score: scoreMiddle[0] });
                });

                socket.on('score', (data) => {
                    console.log(data.score);
                    $('#Puan').html(data.score);
                });

                socket.on('answersCount', (data) => {
                    $scope.answersCount = data.answerCount;
                    $scope.$applyAsync();

                    if($scope.answersCount == Object.size($scope.players)){
                        clearInterval(x);
                        $scope.showQuestions.pop();
                        $scope.$applyAsync();
                        $('#middleSub').show();
                        $('#middleSub1').show();
                        $('#remaining').hide();
                        $('#answersCount').hide();
                        if(!timeUp){
                            $('#timeUp').show();
                        }
                        if(first == 1){
                            point = 1;
                        }
                        timeUp = 0;

                        Object.keys($scope.players).forEach(element => {
                            $scope.oyuncular.push($scope.players[element]);
                            $scope.$applyAsync();
                        });
    
                        var max = Math.max.apply(Math, $scope.oyuncular.map(x => x.score));
                        var fark = max - $scope.players[$('#mineId').text()].score;
                        if( fark != 0 ){
                            $('#firstBeetwen').html('1.ile Aranızdakı Puan Farkı: '+fark);
                            $('#firstBeetwenWrong').html('1.ile Aranızdakı Puan Farkı: '+fark);
                            $('#firstBeetwenTimeUp').html('1.ile Aranızdakı Puan Farkı: '+fark);
                        }
                        else{
                            $('#firstBeetwen').html('Birincisiniz');
                            $('#firstBeetwenWrong').html('Birincisiniz');
                            $('#firstBeetwenTimeUp').html('Birincisiniz');
                        }
                        
                    }
                });

                socket.on('mineId', (id) => {
                    $('#mineId').html(id);
                });

                socket.on('initPlayers', (players) => {
                    Object.keys(players).forEach(item => {
                        if(players[item].roomId == $('#id').val()){
                            $scope.players[item] = players[item];
                        }
                    });

                    $scope.$applyAsync();
                });

                socket.on('newUserConnect', (data) => {
                    $('#name').hide();
                    $('#loginBlock').hide();
                    $('#middle').show();
                    $('#middleSub').show();
                    $('#middleSub1').show();
                    $('#wait').show();

                    $scope.players[data.id] = data;
                    $scope.$applyAsync();
                });

                socket.on('log', (data) => {
                    const messagesData = {
                        type: {
                            code: 0, //server or user messages: type of code
                        }, //info
                        message: data.messages
                    };

                    $scope.messages.push(messagesData);
                    $scope.$applyAsync();
                });

                socket.on('inputNameShow', (data) => {
                    $('#login').animate({left: '100%'}, () => {
                        $('#login').hide();
                        $('#name').show();
                        $('#name').animate({left: '0%'});
                    });   
                });

                socket.on('showMiddle', () => {
                    $('#middle').show();
                });

                socket.on('hata', (data) => {
                    const messagesData = {
                        type: {
                            code: 0, //server or user messages: type of code
                        }, //info
                        message: data.err
                    };
                    
                    var username = document.getElementById('login');
                    username.classList.add('error');
                
                    setTimeout(function() {
                        username.classList.remove('error');
                    }, 300);

                    var username = document.getElementById('name');
                    username.classList.add('error');
                
                    setTimeout(function() {
                        username.classList.remove('error');
                    }, 300);


                    $scope.messages.push(messagesData);
                    $scope.$applyAsync();
                });

                socket.on('questions', (data) => {
                    $('#remaining').show();
                    var array = data;
                    array.forEach(element => {
                        if(element.realRoomId == $('#id').val()) $scope.questions.push(element);
                    });
                    $scope.$applyAsync();
                    $scope.playersOfScore = $scope.players[$('#mineId').text()].score;
                    $scope.showQuestions.push($scope.questions[0]);
                    $scope.$applyAsync();
                    $('#middle').hide();
                    $('#middleSub').hide();
                    $('#wait').hide();
                    $('#middleSub1').hide(); 

                    distance = $scope.showQuestions[0].Sure;
                    x = setInterval(() => {
                        $('#remaining').html(distance);
                        distance -= 1;
                        passingTime = $scope.showQuestions[0].Sure - distance;
                        if (distance < 0) {
                            clearInterval(x);
                            $scope.showQuestions.pop();
                            $scope.$applyAsync();
                            $('#middleSub').show();
                            $('#middleSub1').show();
                            $('#remaining').hide();
                            $('#answersCount').hide();
                            if(!timeUp){
                                $('#timeUp').show();
                            }
                            timeUp = 0; 
                            point=1;

                            Object.keys($scope.players).forEach(element => {
                                $scope.oyuncular.push($scope.players[element]);
                                $scope.$applyAsync();
                            });
        
                            var max = Math.max.apply(Math, $scope.oyuncular.map(x => x.score));
                            console.log(max);
                        }
                    }, 1000);

                    $('#start').hide();
                });

                socket.on('delay', () => {
                    distance -= 1;
                });

                socket.on('pass', () => {
                    first = 0;
                    $('#middle').hide('slow');
                    $('#middleSub').hide();
                    $('#middleSub1').hide();
                    $('#timeUp').hide();
                    $('#correct').hide();
                    $('#wrong').hide();
                    $('#remaining').show();
                    $scope.playersOfScore = $scope.players[$('#mineId').text()].score;
                    $scope.$apply();

                    if(point <= $scope.questions.length){
                        $scope.showQuestions.push($scope.questions[point]);
                        $scope.$applyAsync();

                        distance = $scope.showQuestions[0].Sure;
                            x = setInterval(() => {
                            $('#remaining').html(distance);
                            distance -= 1;
                            passingTime = $scope.showQuestions[0].Sure - distance;
                            if (distance < 0) {
                                clearInterval(x);
                                $scope.showQuestions.pop();
                                $scope.$applyAsync();
                                $('#middleSub').show();
                                $('#middleSub1').show();
                                $('#remaining').hide();
                                $('#answersCount').hide();
                                if(!timeUp){
                                    $('#timeUp').show();
                                }
                                timeUp = 0;

                                Object.keys($scope.players).forEach(element => {
                                    $scope.oyuncular.push($scope.players[element]);
                                    $scope.$applyAsync();
                                });
            
                                var max = Math.max.apply(Math, $scope.oyuncular.map(x => x.score));
                                console.log(max);
                            }
                        }, 1000);

                        point++;
                    }
                    $scope.$applyAsync();
                });

                socket.on('admin', () => {
                    $(".passive").show();
                    socket.disconnect();
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