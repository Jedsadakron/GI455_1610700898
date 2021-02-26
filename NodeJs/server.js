var websocket = require('ws');
const sqlite3 = require('sqlite3').verbose();



var callbackInitServer = ()=>
{
    console.log("XServer is running");
}

var wss = new websocket.Server({port:25500}, callbackInitServer);

var wsList = [];
var roomList = [];

/*
{
    roomName: ""
    wsList: []
}
*/

let db = new sqlite3.Database('./userdatabase/loginDB.db', sqlite3.OPEN_CREATE | sqlite3.OPEN_READWRITE, (err)=>
{
    if(err) throw err;

    console.log('Connected to database.');

    wss.on("connection", (ws)=>{
    
        //Lobby
        console.log("client connected.");
        //Reception
        ws.on("message", (data)=>{
            console.log("send from client :"+ data);
    
            //========== Convert jsonStr into jsonObj =======
    
            //toJsonObj = JSON.parse(data);
    
            // I change to line below for prevent confusion
            var toJsonObj = { 
                roomName:"",
                data:""
            }
            toJsonObj = JSON.parse(data);
            //===============================================

            //================user register and login=======================//
            var dataFromClient =
            {
                eventName:"",
                data:""
            }

            //================user register=======================//


            if(toJsonObj.eventName == "Register")
            {

                dataFromClient = JSON.parse(data);
                var sqliteStr = dataFromClient.data.split('#');
                var userID = sqliteStr[0];
                var password = sqliteStr[1];
                var name = sqliteStr[2];

                var sqlInsert = "INSERT INTO DataUser (UserID, Password, Name) VALUES ('"+userID+"', '"+password+"', '"+name+"')";//register
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
                        ws.send(toJsonStr);
                        console.log("===[3]===")
                        console.log(toJsonStr)
                        console.log("===[3]===")
                    }
                    else
                    {
                        var callbackMsg =
                        {
                            eventName:"Register",
                            data:"success"
                        }
                        var toJsonStr = JSON.stringify(callbackMsg);
                        ws.send(toJsonStr);
                        console.log("===[4]===")
                        console.log(toJsonStr)
                        console.log("===[4]===")
                    }
                });
            }
            
            //===================register================================//

            //===================login===================================//


            if(toJsonObj.eventName == "Login")
            {
                dataFromClient = JSON.parse(data);
                var sqliteStr = dataFromClient.data.split('#');
                var userID = sqliteStr[0];
                var password = sqliteStr[1];
            
                var sqlSelect = "SELECT * FROM DataUser WHERE UserID='"+userID+"' AND Password='"+password+"'";//login
                db.all(sqlSelect,(err, rows)=>
                {
                    if(err)
                    {
                        console.log("===[5]===")
                        console.log(err);
                        console.log("===[5]===")
                    }
                    else
                    {
                        if(rows.length > 0)
                        {
                            console.log("===[6]===")
                            console.log(rows);
                            console.log("===[6]===")
    
                            var callbackMsg =
                            {
                                eventName:"Login",
                                //data:"success"
                                data:rows[0].name
                                
                            }
    
                            var toJsonStr = JSON.stringify(callbackMsg);
                            ws.send(toJsonStr);
                            console.log("===[7]===")
                            console.log(toJsonStr);
                            console.log("login success");
                            console.log("===[7]===")
    
                        }
                        else
                        {
                            var callbackMsg =
                            {
                                eventName:"Login",
                                data:"fail"
                            }
    
                            var toJsonStr = JSON.stringify(callbackMsg);
                            ws.send(toJsonStr);
                            console.log("===[8]===")
                            console.log(toJsonStr);
                            console.log("login fail");
                            console.log("===[8]===")
                        }
                    }
                });
            }
            //===================login===================================//

            if(toJsonObj.eventName == "CreateRoom")//CreateRoom
            {
                //============= Find room with roomName from Client =========
                var isFoundRoom = false;
                for(var i = 0; i < roomList.length; i++)
                {
                    if(roomList[i].roomName == toJsonObj.data)
                    {
                        isFoundRoom = true;
                        break;
                    }
                }
                //===========================================================
    
                if(isFoundRoom == true)// Found room
                {
                    //Can't create room because roomName is exist.
                    //========== Send callback message to Client ============
                    var callbackMsg = {
                        eventName:"CreateRoom",
                        data:"fail"
                    }
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                    //=======================================================
    
                    console.log("client create room fail.");
                }
                else
                {
                    //============ Create room and Add to roomList ==========
                    var newRoom = {
                        roomName: toJsonObj.data,
                        wsList: []
                    }
    
                    newRoom.wsList.push(ws);
    
                    roomList.push(newRoom);
                    //=======================================================
                    var callbackMsg = {
                        eventName:"CreateRoom",
                        data:toJsonObj.data
                    }
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                    //=======================================================
                    console.log("client create room success.");
                }
                  
            }
            else if(toJsonObj.eventName == "JoinRoom")//JoinRoom
            {
                console.log("client request join room.");

                if(roomList.length > 0)
                {
                    for(var i = 0; i < roomList.length; i++)
                    {
                        if(roomList[i].roomName == toJsonObj.data)
                        {
                            roomList[i].wsList.push(ws);
    
                            var callbackMsg = 
                            {
                                eventName:"JoinRoom",
                                data:toJsonObj.data
                            }
                            var toJsonStr = JSON.stringify(callbackMsg);
                            ws.send(toJsonStr);
                            console.log("client join room success.")
                            console.log(roomList);
                            break;
                        }
                        if(i == roomList.length - 1)
                        {
                            var callbackMsg = 
                            {
                                eventName:"JoinRoom",
                                data:"fail"
                            }
                            var toJsonStr = JSON.stringify(callbackMsg);
                            ws.send(toJsonStr);
                            console.log("client join room fail.")
                            break;
                        }
                    }
                }
                
                //========================================
            }
            else if(toJsonObj.eventName == "LeaveRoom")//LeaveRoom
            {
                //============ Find client in room for remove client out of room ================
                var isLeaveSuccess = false;//Set false to default.
                for(var i = 0; i < roomList.length; i++)//Loop in roomList
                {
                    for(var j = 0; j < roomList[i].wsList.length; j++)//Loop in wsList in roomList
                    {
                        if(ws == roomList[i].wsList[j])//If founded client.
                        {
                            roomList[i].wsList.splice(j, 1);//Remove at index one time. When found client.
    
                            if(roomList[i].wsList.length <= 0)//If no one left in room remove this room now.
                            {
                                roomList.splice(i, 1);//Remove at index one time. When room is no one left.
                            }
                            isLeaveSuccess = true;
                            break;
                        }
                    }
                }
                //===============================================================================
    
                if(isLeaveSuccess)
                {
                    //========== Send callback message to Client ============
    
                    //ws.send("LeaveRoomSuccess");
    
                    //I will change to json string like a client side. Please see below
                    var callbackMsg = {
                        eventName:"LeaveRoom",
                        data:"success"
                    }
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                    //=======================================================
    
                    console.log("leave room success");
                }
                else
                {
                    //========== Send callback message to Client ============
    
                    //ws.send("LeaveRoomFail");
    
                    //I will change to json string like a client side. Please see below
                    var callbackMsg = {
                        eventName:"LeaveRoom",
                        data:"fail"
                    }
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                    //=======================================================
    
                    console.log("leave room fail");
                }
            }

            else if(toJsonObj.eventName == "SendMessage")
            {

                console.log("Receive message :" + data);
                Boardcast(ws,message);
            }
        });
   
        //wsList.push(ws);
        
        /*ws.on("message", (data)=>{
            console.log("send from client :"+ data);
            Boardcast(ws,data);
        });*/
        
        ws.on("close", ()=>{
            console.log("client disconnected.");
    
            //============ Find client in room for remove client out of room ================
            for(var i = 0; i < roomList.length; i++)//Loop in roomList
            {
                for(var j = 0; j < roomList[i].wsList.length; j++)//Loop in wsList in roomList
                {
                    if(ws == roomList[i].wsList[j])//If founded client.
                    {
                        roomList[i].wsList.splice(j, 1);//Remove at index one time. When found client.
    
                        if(roomList[i].wsList.length <= 0)//If no one left in room remove this room now.
                        {
                            roomList.splice(i, 1);//Remove at index one time. When room is no one left.
                        }
                        break;
                    }
                }
            }
            //===============================================================================
        });
    });
    
    function Boardcast(ws, message)
    {
  
        {
            var selectionRoomIndex = -1;
        
            for(var i = 0; i > roomList.length; i++){
                for(var j = 0; j < roomList[i].wsList.length; j++){
                    if(ws == roomList[i].wsList[j]){
                        selectionRoomIndex = i;
                        break;
                    }
                }
            }
        
            for(var i = 0; i < roomList[selectionRoomIndex].wsList.length; i++)
            {
                roomList[selectionRoomIndex].wsList[i].send(message);
            }
        }
    }
    
    
});

