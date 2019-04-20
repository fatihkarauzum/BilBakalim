app.controller('loginController', ['$scope', 'loginFactory', ($scope, loginFactory) => {
    $scope.messages = [ ];

    $scope.init = () => {
        initSocket();
    };

    async function initSocket(){
        const connectionOptions = {
            reconnectionAttempts: 3,
            reconnectionDelay: 600
        };
    
        const socket = await loginFactory.connectSocket("http://localhost:3000", connectionOptions)
            try{
                $('#login').on('click', () => {
                    socket.emit('joinRoom', { roomId: $('#id').val() });
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
            }
            catch(err){
                console.log(err);
            }
        }
}]);