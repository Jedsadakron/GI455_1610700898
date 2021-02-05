var websocket = require('ws');

var callbackInitServer = ()=>
{
    console.log("XServer is running");
}

var wss = new websocket.Server({port:25500}, callbackInitServer);

var wsList = [];

wss.on("connection", (ws)=>
{
    console.log("some client connected.");
    wsList.push(ws);

    ws.on("message", (data)=>
    {
        console.log("send from some client :"+ data);
        Boardcast(data);

    });

    ws.on("close", ()=>
    {
        console.log("some client disconnected.")
        wsList = ArrayRemove(wsList, ws);
    });
});

function ArrayRemove(arr, value)
{
    return arr.filter((element)=>
    {
        return element != value;

    });

}

function Boardcast(data)
{
    for(var i = 0; i < wsList.length; i++)
    {
        wsList[i].send(data);
    }
}

