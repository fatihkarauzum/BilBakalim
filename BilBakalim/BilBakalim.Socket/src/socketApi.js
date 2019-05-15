const socketio = require('socket.io');
const io = socketio();
const sql = require('../helpers/sql');
// const a = require('../../BilBakalim.Web/Content/Resimler/SinifSoru/');
const socketApi = {
    io
};

const users = { };
const questions = [ ];
const roomIds = [ ];
const answers = { };
const choosingAnswer = { };
const sessionIds = { };

// function initPlayers(roomID) {
//     users.forEach(element => {

//     });
// }

var finish = (getRoomId) => {
    var roomId = getRoomId;
    Object.keys(users).forEach(item => {
        if(users[item].roomId == roomId){
            delete users[item];
        }
    });

    var length = questions.length;
    for( var i = length - 1 ; i >= 0; i--){          
        if ( questions[i].realRoomId == roomId) {
            questions.splice(i, 1);
        }
    }

    roomIds.forEach( item => {
        if ( item == roomId ) {
            roomIds.splice(i, 1);
        }
    });
    
    console.log(roomIds);

    Object.keys(answers).forEach(item => {
        if(answers[item].roomId == roomId){
            delete answers[item];
        }
    });

    Object.keys(choosingAnswer).forEach(item => {
        if(choosingAnswer[item].roomId == roomId){
            delete choosingAnswer[item];
        }
    });
    
    // Object.keys(sessionIds).forEach(item => {
    //     if(sessionIds[item].roomId == roomId){
    //         delete sessionIds[item];
    //     }
    // });
};

io.on('connection', (socket) => {

    socket.on('registerRoom', (data) => {

        if(roomIds.length == 0){
            data.roomId = Math.floor(Math.random() * 100000) + 1;
            roomIds.push(data.roomId);
        }
        else
        {roomIds.forEach(element => {
            if(element != data.roomId){
                data.roomId = Math.floor(Math.random() * 100000) + 1;
                roomIds.push(data.roomId);
            }   
            else{
                data.roomId = Math.floor(Math.random() * 100000) + 1;
                roomIds.push(data.roomId);
            }
        });}

        const defaultData = {
            answer: 0,
            roomId: data.roomId
        };
        answers[data.roomId] = defaultData;

        const defaultChoosing = {
            Cevap1: 0,
            Cevap2: 0,
            Cevap3: 0,
            Cevap4: 0,
            roomId: data.roomId,
            isSurvey: false
        };

        if(data.type == 'survey'){
            defaultChoosing.isSurvey = true;
        }

        choosingAnswer[data.roomId] =  defaultChoosing;

        const defaultRoom = {
            roomId: data.roomId,
        };
        
        sessionIds[socket.id] = defaultRoom;

        socket.join(data.roomId, () => {
            io.to(data.roomId).emit('log', { messages: data.roomId + ' Numaralı Odaya Girildi.', realRoomId: data.roomId });
        });
    });

    socket.on('joinRoom', (data) => {
        if(io.sockets.adapter.rooms[data.roomId] != null)
        {
            var control = 0;
            questions.forEach( element => {
                if(element.realRoomId == data.roomId) {
                    control = 1;
                }
            });

            if(!control){
                socket.join(data.roomId, () => {
                    if(choosingAnswer[data.roomId].isSurvey){
                        socket.emit('inputNameShow', { type: 'survey' });
                    }
                    else{
                        socket.emit('inputNameShow', { type: 'question' });
                    }
                });
            }
            else{
                socket.emit('hata', { err: 'Odada Oyun Başlamış Durumda.'});
            }
        }
        else{
            socket.emit('hata', { err: 'Oda Bulunamadı.'});
        }
        
    });

    socket.on('newUser', (data) => {
        var control = true;
        Object.keys(users).forEach(item => {
            if(users[item].username == data.username && users[item].roomId == data.roomId){
                control = false;
            }
        });

        if(control){
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
        }
        else if(!control){
            socket.emit('hata', { err: 'İsim Kullanılmakta.'});
        }
    });

    socket.on('start', (data) => {
        var request = new sql.Request();
        request.query('SELECT '+ data.type +'.*, Resim.Url FROM '+ data.type +' FULL OUTER JOIN Resim ON '+ data.type +'.MedyaID = Resim.ID WHERE '+ data.type +'.SinifID = '+ data.roomId, function (err, recordset) {
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

    socket.on('saveResult', (data) => {
        console.log(data.realRoomId, choosingAnswer[data.realRoomId].Cevap1, choosingAnswer[data.realRoomId].Cevap2, choosingAnswer[data.realRoomId].Cevap3, choosingAnswer[data.realRoomId].Cevap4, data.soruID);

        var request = new sql.Request();
        request.query('INSERT INTO AnketOturum (Pin, Cevap1, Cevap2, Cevap3, Cevap4, SoruID) VALUES ('+ data.realRoomId +', '+ choosingAnswer[data.realRoomId].Cevap1 +', '+ choosingAnswer[data.realRoomId].Cevap2 +', '+ choosingAnswer[data.realRoomId].Cevap3 +', '+ choosingAnswer[data.realRoomId].Cevap4 +', '+ data.soruID +')', function (err, recordset) {
            if (err){
                console.log(err);
            }                 
        });
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

    socket.on('disconnect', () => {
        if(sessionIds[socket.id] != null){
            var roomId = sessionIds[socket.id].roomId;

            finish(roomId);

            io.to(sessionIds[socket.id].roomId).emit('admin');
        }
        else if(users[socket.id] != null){
            io.to(users[socket.id].roomId).emit('oyuncu', { oyuncuId: socket.id });
        }
    });
});

const getOnlineCount = (io, data) => {
    const room = io.sockets.adapter.rooms[data.name];
    return room ? room.length : 0; 
};

module.exports = socketApi;