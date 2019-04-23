app.controller('indexController', ['$scope', 'indexFactory', ($scope, indexFactory) => {
    $scope.messages = [ ];
    $scope.players = { };
    $scope.questions = [ ];
    $scope.showQuestions = [ ];
    $scope.answersCount;
    $scope.oyuncular = [ ];
    var point;
    var distance;
    var passingTime;
    var score;
    var first = 1;
    var x;

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

        const socket = await indexFactory.connectSocket("http://localhost:3000", connectionOptions)
            try{
                socket.emit('registerRoom', { roomId: $('#id').text() });

                socket.on('newUserConnect', (data) => {
                    $scope.players[data.id] = data;
                    $scope.$applyAsync();

                    $('#start').removeAttr("disabled");
                });

                socket.on('log', (data) => {
                    const messagesData = {
                        type: {
                            code: 0, //server or user messages: type of code
                        }, //info
                        message: data.messages
                    };
                    
                    $('#realId').html(data.realRoomId);
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
                        $('#middle').show('slow');
                        $('#remaining').hide();
                        $('#answersCount').hide();
                        if(first == 1){
                            point = 1;
                        }
                        socket.emit('showMiddle', { realRoomId: $('#realId').text() });
                    }
                });

                socket.on('showMiddle', (data) => {
                    $('#choosingA').html(data.choosingAnswers.Cevap1);
                    $('#choosingB').html(data.choosingAnswers.Cevap2);
                    $('#choosingC').html(data.choosingAnswers.Cevap3);
                    $('#choosingD').html(data.choosingAnswers.Cevap4);
                });

                $('#start').on('click', () => {
                    socket.emit('start', { roomId: $('#id').text(), realRoomId: $('#realId').text() });
                    $('#remaining').show();
                });

                $scope.passScores = () => {
                    $('#middle').hide();

                    Object.keys($scope.players).forEach(element => {
                        $scope.oyuncular.push($scope.players[element]);
                        $scope.$applyAsync();
                    });

                    $('#score').show();
                }

                $('#pass').on('click', () => {
                    first = 0;
                    $('#remaining').show();
                    if(point < $scope.questions.length){
                        socket.emit('pass', { roomId: $('#id').text(), realRoomId: $('#realId').text() })

                        $('#score').hide('slow');
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
                                    $('#middle').show('slow');
                                    $('#remaining').hide();
                                    $('#answersCount').hide();

                                    socket.emit('showMiddle', { realRoomId: $('#realId').text() });
                                }
                            }, 1000);
                            point++;
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

                    distance = $scope.showQuestions[0].Sure;
                    x = setInterval(() => {
                        $('#remaining').html(distance);
                        distance -= 1;
                        passingTime = $scope.showQuestions[0].Sure - distance;
                        if (distance < 0) {
                            clearInterval(x);
                            $scope.showQuestions.pop();
                            $scope.$applyAsync();
                            $('#middle').show('slow');
                            $('#remaining').hide();
                            $('#answersCount').hide();

                            socket.emit('showMiddle', { realRoomId: $('#realId').text() });
                            point=1;
                        }
                    }, 1000);

                    $('#start').hide('slow');
                });

            }
            catch(err){
                console.log(err);
            }
        }
}]);