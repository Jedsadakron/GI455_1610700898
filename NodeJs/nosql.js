var mongo = require('mongodb').MongoClient;
var url = "mongodb://localhost:27017//";

mongo.connect(url, {useUnifiedTopology: true}, (err, result)=>{
    if(err) throw err;

    console.log(result);

    var selectDB = result.db("gi455");

    Register = (selectDB,"5678","5678","hahahaha");

});

var Register = (db,playerid,password,playername)=>{
    var newData = {
        platerID:playerid,
        playerPassword:password,
        playerName:playername
    }

    db.collection("studen").insertOne(newData,(err,result)=>{
        if(err)
        {
            console.log(err);
        }
    });
}