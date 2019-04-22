const socketio = require('socket.io');
const io = socketio();
const sql = require('../helpers/sql');
const socketApi = {
    io
};

const users = { };
const questions = [ ];
const roomIds = [ ];

// function initPlayers(roomID) {
//     users.forEach(element => {

//     });
// }

io.on('connection', (socket) => {

    socket.on('joinRoom', (data) => {
        if(io.sockets.adapter.rooms[data.roomId] != null)
        {
            socket.join(data.roomId, () => {
                io.to(data.roomId).emit('log', { messages: data.roomId + ' Numaralı Odaya Girildi.' });
                socket.emit('inputNameShow');
            });
        }
        else{
            socket.emit('hata', { err: 'Oda Bulunamadı.'});
        }
        
    });

    socket.on('newUser', (data) => {
        const defaultData = {
            id: socket.id,
            roomId: data.roomId,
            username: data.username
        };

        users[socket.id] = defaultData;

        io.to(data.roomId).emit('newUserConnect', users[socket.id]);    
        socket.emit('initPlayers', users);
    });

    socket.on('registerRoom', (data) => {

        data.roomId = Math.floor(Math.random() * 1000) + 1;

        roomIds.forEach(element => {
            if(element != data.roomIds){
                roomIds.push(data.roomIds);
            }   
            else{
                data.roomId = Math.floor(Math.random() * 1000) + 1;
                roomIds.push(data.roomIds);
            }
        });

        socket.join(data.roomId, () => {
            io.to(data.roomId).emit('log', { messages: data.roomId + ' Numaralı Odaya Girildi.', realRoomId: data.roomId });
        });
    });

    socket.on('start', (data) => {
        var request = new sql.Request();
        request.query('SELECT * FROM Sorular WHERE SinifID = '+ data.roomId, function (err, recordset) {
            if (err){
                console.log(err)
            }
            else{
                var array = recordset.recordset;
                array.forEach(element => {
                    element.realRoomId = data.realRoomId;
                    questions.push(element);
                });
                io.to(data.realRoomId).emit('questions', questions);
            }                       
        });     
    });

    socket.on('pass', (data) => {
        io.to(data.realRoomId).emit('pass');
    });

});

const getOnlineCount = (io, data) => {
    const room = io.sockets.adapter.rooms[data.name];
    return room ? room.length : 0; 
};

module.exports = socketApi;