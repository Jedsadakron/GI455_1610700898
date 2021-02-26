const sqlite3 = require('sqlite3').verbose();

let db = new sqlite3.Database('./database/chatDB.db', sqlite3.OPEN_CREATE | sqlite3.OPEN_READWRITE, (err)=>
{
    if(err) throw err;

    console.log('Connected to database.');

    /*//(login (old id in db))
    var dataFromClient =
    {
        eventName:"Login",
        data:"test00000#00000#test0"
    }

    var splitStr = dataFromClient.data.split('#');
    var userID = splitStr[0];
    var password = splitStr[1];
    var name = splitStr[2];
    */

    /*//(register (new))
    var dataFromClient =
    {
        eventName:"Register",
        data:"test55555#55555#test5"
    }

    var splitStr = dataFromClient.data.split('#');
    var userID = splitStr[0];
    var password = splitStr[1];
    var name = splitStr[2];
    */

    //updatemoney
   var dataFromClient =
   {
       eventName:"AddMoney",
       data:"test55555#100"
   }

   var splitStr = dataFromClient.data.split('#');
   var userID = splitStr[0];
   var addedmoney = parseInt(splitStr[1]);
   /*var password = splitStr[2];
   var name = splitStr[3];*/


    //var sqlSelect = "SELECT UserID, Password, Name FROM UserData";//วิธีที่ 1 show ทั้งหมด//ทุก User

    //var sqlSelect = "SELECT * FROM UserData";//วิธีที่ 2 show ทั้งหมด ลดรูป//ทุก User

    //var sqlSelect = "SELECT * FROM UserData WHERE NAME='test2'";//กรณีชื่อซ้ำ//show ทุกชื่อ

    //var sqlSelect = "SELECT * FROM UserData WHERE UserID='test22222'";//กรณีหา id แบบเจาะจง

    //var sqlSelect = "SELECT * FROM UserData WHERE Password='33333' AND NAME='test2'";//หา password และ name แบบเจาะจง

    //var sqlSelect = "SELECT * FROM UserData WHERE UserID='test11111' AND Password='11111'";//login v1

    //var sqlSelect = "SELECT * FROM UserData WHERE UserID='"+userID+"' AND Password='"+password+"'";//login v2

    //var sqlInsert = "INSERT INTO UserData (UserID, Password, Name) VALUES ('"+userID+"', '"+password+"', '"+name+"')";//register
    
    //var sqlUpdate = "UPDATE UserData SET Money='200' WHERE UserID='"+userID+"'";//update(someitem , coin/money) in one shot no dupt
    
    /*//add money // + money +100 +200 +300
    db.all("SELECT Money FROM UserData WHERE UserID='"+userID+"'",(err,rows)=>
    {
        if(err)
        {
            var callbackMsg =
            {
                eventName:"AddMoney",
                data:"fail"
            }
            var toJsonStr = JSON.stringify(callbackMsg);
            console.log("======[0]======")
            console.log(toJsonStr)
            console.log("======[0]======")
        }
        else
        {
            console.log("======[1]======")
            console.log(rows);
            console.log("======[1]======")

            if(rows.length > 0)
            {
                var currentMoney = rows[0].Money;
                currentMoney += addedmoney;

                db.all("UPDATE UserData SET Money='"+currentMoney+"' WHERE UserID='"+userID+"'",(err, rows)=>
                {
                    if(err)
                    {
                        var callbackMsg =
                        {
                            eventName:"AddMoney",
                            data:"fail"
                        }
                        var toJsonStr = JSON.stringify(callbackMsg);
                        console.log("======[2]======")
                        console.log(toJsonStr)
                        console.log("======[2]======")
                    }
                    else
                    {
                        var callbackMsg =
                        {
                            eventName:"AddMoney",
                            data:currentMoney
                        }
                        var toJsonStr = JSON.stringify(callbackMsg);
                        console.log("======[3]======")
                        console.log(toJsonStr)
                        console.log("======[3]======")
                    }
                });//(WHERE ? ระบุให้ใคร)
            }
            else
            {
                var callbackMsg =
                {
                    eventName:"AddMoney",
                    data:"fail"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                console.log("======[4]======")
                console.log(toJsonStr)
                console.log("======[4]======")
            }
        }
    });*/

    /*//updatemoney one shot
    db.all(sqlUpdate,(err,rows)=>
    {
        if(err)
        {
            var callbackMsg =
            {
                eventName:"AddMoney",
                data:"fail"
            }
            var toJsonStr = JSON.stringify(callbackMsg);
            console.log("======[0]======")
            console.log(toJsonStr)
            console.log("======[0]======")
        }
        else
        {
            var callbackMsg =
            {
                eventName:"AddMoney",
                data:"success"
            }
            var toJsonStr = JSON.stringify(callbackMsg);
            console.log("======[1]======")
            console.log(toJsonStr)
            console.log("======[1]======")
        }
    });*/
    
    /*//register
    db.all(sqlInsert,(err,rows)=>
    {
        if(err)
        {
            var callbackMsg =
            {
                eventName:"Register",
                data:"fail"
            }

            var toJsonStr = JSON.stringify(callbackMsg);
            console.log("======[0]======")
            console.log(toJsonStr)
            console.log("======[0]======")
        }
        else
        {
            var callbackMsg =
            {
                eventName:"Register",
                data:"success"
            }

            var toJsonStr = JSON.stringify(callbackMsg);
            console.log("======[1]======")
            console.log(toJsonStr)
            console.log("======[1]======")
        }
    });*/

    /*//login
    db.all(sqlSelect,(err, rows)=>//login
    {
        if(err)
        {
            console.log("[0]" + err);
        }
        else
        {
            if(rows.length > 0)
            {
                console.log("======[1]======")
                console.log(rows);//"[?]" log บอกตำแหน่ง จุดที่ log ออกมา
                console.log("======[1]======")
                var callbackMsg =
                {
                    eventName:"Login",
                    //data:"success"//v1
                    data:rows[0].Name
                }

                //console.log("login success");//v1

                var toJsonStr = JSON.stringify(callbackMsg);
                console.log("======[2]======")
                console.log(toJsonStr);
                console.log("======[2]======")
            }
            else
            {
                var callbackMsg =
                {
                    eventName:"Login",
                    data:"fail"
                }

                var toJsonStr = JSON.stringify(callbackMsg);
                console.log("======[3]======")
                console.log(toJsonStr);
                console.log("======[3]======")
            }
            //console.log(rows);
        }
    });*/
});