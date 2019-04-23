const socketio = require('socket.io');
const io = socketio();
const sql = require('../helpers/sql');
const socketApi = {
    io
};

const users = { };
const questions = [ ];
const roomIds = [ ];
const answers = { };
const choosingAnswer = { };

// function initPlayers(roomID) {
//     users.forEach(element => {

//     });
// }

io.on('connection', (socket) => {

    socket.on('joinRoom', (data) => {
        if(io.sockets.adapter.rooms[data.roomId] != null)
        {
            socket.join(data.roomId, () => {
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
            username: data.username,
            score: 0
        };

        users[socket.id] = defaultData;

        io.to(data.roomId).emit('newUserConnect', users[socket.id]);    
        socket.emit('mineId', socket.id);
        socket.emit('initPlayers', users);
    });

    socket.on('registerRoom', (data) => {

        data.roomId = Math.floor(Math.random() * 100000) + 1;

        roomIds.forEach(element => {
            if(element != data.roomIds){
                roomIds.push(data.roomIds);
            }   
            else{
                data.roomId = Math.floor(Math.random() * 100000) + 1;
                roomIds.push(data.roomIds);
            }
        });

        const defaultData = {
            answer: 0
        };
        answers[data.roomId] = defaultData;

        const defaultChoosing = {
            Cevap1: 0,
            Cevap2: 0,
            Cevap3: 0,
            Cevap4: 0
        };
        choosingAnswer[data.roomId] =  defaultChoosing;

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
        choosingAnswer[data.realRoomId].Cevap1 = 0;
        choosingAnswer[data.realRoomId].Cevap2 = 0;
        choosingAnswer[data.realRoomId].Cevap3 = 0;
        choosingAnswer[data.realRoomId].Cevap4 = 0;
        io.to(data.realRoomId).emit('pass');
    });

    socket.on('correctAnswer', (data) => {
        io.to(data.roomId).emit('correctAnswer', { mineId: data.mineId, timeLeft: data.timeLeft, award: data.award });
        socket.emit('meCorrect', { mineId: data.mineId });
    });

    socket.on('answersCount', (data) => {
        answers[data.roomId].answer += 1;
        io.to(data.roomId).emit('answersCount', { answerCount: answers[data.roomId].answer });
    });

    socket.on('delay', () => {
        io.sockets.in('delay');
    });

    socket.on('showMiddle', (data) => {
        io.to(data.realRoomId).emit('showMiddle', { choosingAnswers: choosingAnswer[data.realRoomId] });
    });

    socket.on('answersCountReset', (data) => {
        answers[data.realRoomId].answer = 0;
    });

    socket.on('choosingAnswer', (data) => {
        switch (data.answer) {
            case 'Cevap1':
                choosingAnswer[data.roomId].Cevap1 += 1;
                break;
            case 'Cevap2':
                choosingAnswer[data.roomId].Cevap2 += 1;
                break;
            case 'Cevap3':
                choosingAnswer[data.roomId].Cevap3 += 1;
                break;
            case 'Cevap4':
                choosingAnswer[data.roomId].Cevap4 += 1;
                break;
            default:
                break;
        }
    });

    socket.on('score', (data) => {
        console.log(data.score);
        socket.emit('score', { score: data.score });
    });

});

const getOnlineCount = (io, data) => {
    const room = io.sockets.adapter.rooms[data.name];
    return room ? room.length : 0; 
};

module.exports = socketApi;