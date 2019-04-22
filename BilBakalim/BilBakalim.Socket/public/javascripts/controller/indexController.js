app.controller('indexController', ['$scope', 'indexFactory', ($scope, indexFactory) => {
    $scope.messages = [ ];
    $scope.players = { };
    $scope.questions = [ ];
    $scope.showQuestions = [ ];
    var point;

    $scope.init = () => {
        initSocket();
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
                    console.log($scope.messages);
                });

                $('#start').on('click', () => {
                    socket.emit('start', { roomId: $('#id').text() });
                });

                $('#pass').on('click', () => {
                    socket.emit('pass', { roomId: $('#id').text() })

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

                socket.on('questions', (data) => {
                    var array = data;
                    array.forEach(element => {
                        if(element.SinifID == $('#id').text()) $scope.questions.push(element);
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

            }
            catch(err){
                console.log(err);
            }
        }
}]);