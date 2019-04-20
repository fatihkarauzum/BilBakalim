app.controller('indexController', ['$scope', 'indexFactory', ($scope, indexFactory) => {
    $scope.messages = [ ];

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