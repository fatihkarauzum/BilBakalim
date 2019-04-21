const sql = require("mssql");

const config = {
    user: 'fatih',
    password: '123456',
    server: 'localhost', 
    database: 'BilBakalimDB' 
};

sql.connect(config, function (err) {
    if (err) console.log(err);
});

module.exports = sql;