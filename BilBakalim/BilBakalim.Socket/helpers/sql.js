const sql = require("mssql");

const config = {
    user: 'userBilBakalim',
    password: 'Pass2019*Bakalim',
    server: '79.123.147.224', 
    database: 'BilBakalimDB' 
};

sql.connect(config, function (err) {
    if (err) console.log(err);
});

module.exports = sql;