const socketio = require('socket.io');
const io = socketio();
const socketApi = {
    io
};

var rooms;

io.on('connection', (socket) => {

    socket.on('joinRoom', (data) => {
        rooms = socket.rooms;
        
        rooms.forEach(items => {
            
        });
        
        socket.join(data.roomId, () => {
            io.to(data.roomId).emit('log', { messages: data.roomId + ' Numaralı Odaya Girildi.' });
        });
    });

    socket.on('registerRoom', (data) => {
        socket.join(data.roomId, (data) => {
            io.to(data.roomId).emit('log', { messages: data.roomId + ' Numaralı Odaya Girildi.' });
        });
    });

});

module.exports = socketApi;