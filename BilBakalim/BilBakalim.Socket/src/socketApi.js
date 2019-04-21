const socketio = require('socket.io');
const io = socketio();
const sql = require('../helpers/sql');
const socketApi = {
    io
};

const users = { };
const questions = [ ];

io.on('connection', (socket) => {

    socket.on('joinRoom', (data) => {
        
        if(io.sockets.adapter.rooms[data.roomId] != null)
        {
            socket.join(data.roomId, () => {
                io.to(data.roomId).emit('log', { messages: data.roomId + ' Numaralı Odaya Girildi.' });
            });
        }
        else{
            socket.emit('hata', { err: 'Oda Bulunamadı.'});
        }
        
    });

    socket.on('newUser', (data) => {
        const defaultData = {
            id: socket.id,
            username: data.username
        };

        users[socket.id] = defaultData;
        io.to(data.roomId).emit('newUserConnect', users[socket.id]);
        socket.emit('initPlayers', users);
    });

    socket.on('registerRoom', (data) => {
        socket.join(data.roomId, () => {
            io.to(data.roomId).emit('log', { messages: data.roomId + ' Numaralı Odaya Girildi.' });
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
                    questions.push(element);
                });
    
                io.to(data.roomId).emit('questions', questions);
            }                       
        });     
    });

    socket.on('pass', (data) => {
        io.to(data.roomId).emit('pass');
    });

});

const getOnlineCount = (io, data) => {
    const room = io.sockets.adapter.rooms[data.name];
    return room ? room.length : 0; 
};

module.exports = socketApi;