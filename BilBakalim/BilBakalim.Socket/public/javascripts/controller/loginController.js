app.controller('loginController', ['$scope', 'loginFactory', ($scope, loginFactory) => {  
    $scope.messages = [ ];
    $scope.players = { };
    $scope.questions = [ ];
    $scope.showQuestions = [ ];
    var point;

    $scope.init = () => {
        initSocket();
    };

    $scope.Cevap = (cevap) => {
        answers = ['Cevap1', 'Cevap2', 'Cevap3', 'Cevap4'];
        answers.forEach(element => {
            $('#' + element).prop( "disabled", true );
            $('#' + cevap).css( "background-color", "yellow" )
        });

        questionID = $('.questionID').attr('id');
        $scope.questions.forEach(element=>{
            if(element.ID == questionID){
                if(element.DogruCevap == cevap){
                    console.log('Tebrikler');
                }
            }
        });
    }

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
                    $('#name').hide('slow');
                    socket.emit('newUser', { roomId: $('#id').val(), username: $('#inputName').val() });
                });

                socket.on('initPlayers', (players) => {
                    Object.keys(players).forEach(item => {
                        console.log(item);
                        if(players[item].roomId == $('#id').val()){
                            $scope.players[item] = players[item];
                        }
                    });

                    $scope.$apply();
                });

                socket.on('newUserConnect', (data) => {
                    $scope.players[data.id] = data;
                    $scope.$apply();
                    console.log($scope.players);
                });

                socket.on('log', (data) => {
                    const messagesData = {
                        type: {
                            code: 0, //server or user messages: type of code
                        }, //info
                        message: data.messages
                    };

                    $scope.messages.push(messagesData);
                    $scope.$apply();
                });

                socket.on('inputNameShow', (data) => {
                    $('#name').show('slow');
                    $('#login').hide('slow');
                });

                socket.on('hata', (data) => {
                    const messagesData = {
                        type: {
                            code: 0, //server or user messages: type of code
                        }, //info
                        message: data.err
                    };
                    
                    $scope.messages.push(messagesData);
                    $scope.$apply();
                });

                socket.on('questions', (data) => {
                    var array = data;
                    array.forEach(element => {
                        if(element.realRoomId == $('#id').val()) $scope.questions.push(element);
                    });
                    $scope.$apply();

                    $scope.showQuestions.push($scope.questions[0]);
                    $scope.$apply();

                    setTimeout(() => {
                        $scope.showQuestions.pop();
                        $scope.$apply();
                        $('#score').show('slow');
                        point=1;
                    }, $scope.questions[0].Sure * 1000);

                    $('#start').hide('slow');
                });

                socket.on('pass', () => {
                    $('#score').hide('slow');
                    if(point <= $scope.questions.length){
                        $scope.showQuestions.push($scope.questions[point]);
                        $scope.$apply();

                        setTimeout(() => {
                            $scope.showQuestions.pop();
                            $scope.$apply();
                            $('#score').show('slow');
                        }, $scope.questions[point].Sure * 1000);

                        point++;
                    }
                    $scope.$apply();
                });
            }
            catch(err){
                console.log(err);
            }
        }
}]);