using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace login
{
    public class LoginS : MonoBehaviour
    {
        struct MessageData
        {
            public string username;
            public string message;

            public MessageData(string username, string message)
            {
                this.username = username;
                this.message = message;
            }
        }

        public struct SocketEvent
        {
            public string eventName;
            public string data;

            public SocketEvent(string eventName, string data)
            {
                this.eventName = eventName;
                this.data = data;
            }
        }

        [System.Serializable]
        public class EventServer
        {
            public string eventName;
            public object data;
        }

        public GameObject connected;
        public GameObject login;
        public GameObject register;
        public GameObject loginconnected;
        public GameObject afterhitcreateroom;
        public GameObject afterhitjoinroom;
        public GameObject room;
        public GameObject failsomething;
        public GameObject faillogin;
        public GameObject failpasswordnotmatch;
        public GameObject idpassinusealready;
        public GameObject inputallfild;
        public GameObject failcreateroom;
        public GameObject failjoinroom;

        public InputField username;
        public InputField createroomname;
        public InputField inputroomnameuser;
        public InputField joinroomname;
        public InputField inputText;

        public InputField id;
        public InputField password;

        public InputField regisID;
        public InputField regisName;
        public InputField regisPassword;
        public InputField regisrePassword;

        public InputField aftercreateroom;
        public InputField afterjoinroom;

        public Text usertextname;
        public Text roomnameui;
        public Text textOwner;
        public Text textReceive;

        private WebSocket ws;

        private string tempMessageString;

        public delegate void DeledateHandle(string msg);
        public delegate void DelegateHandle(SocketEvent result);

        public DelegateHandle OnLogin;
        public DelegateHandle OnRegister;
        public DelegateHandle OnCreateRoom;
        public DelegateHandle OnJoinRoom;
        public DelegateHandle OnLeaveRoom;
        public DelegateHandle OnSendMessage;

        public void Start()
        {
            connected.SetActive(true);
            login.SetActive(false);
            register.SetActive(false);
            loginconnected.SetActive(false);
            afterhitcreateroom.SetActive(false);
            afterhitjoinroom.SetActive(false);
            room.SetActive(false);
            failsomething.SetActive(false);
            faillogin.SetActive(false);
            failpasswordnotmatch.SetActive(false);
            idpassinusealready.SetActive(false);
        }

        private void Update()
        {
            UpdateNotifyMessage();

            /*if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                MessageData receiveMessageData = JsonUtility.FromJson<MessageData>(tempMessageString);

                if (receiveMessageData.username == inputText.text)
                {
                    textOwner.text += receiveMessageData.username + ": " + receiveMessageData.message + "\n";
                }
                else
                {
                    textReceive.text += receiveMessageData.username + ": " + receiveMessageData.message + "\n";
                }

                tempMessageString = "";
            }*/
        }

        public void Connected()
        {
            //string url = "ws://gi455-305013.an.r.appspot.com/";
            string url = "ws://127.0.0.1:25500/";//change

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            connected.SetActive(false);
            login.SetActive(true);
            register.SetActive(false);
            loginconnected.SetActive(false);
            afterhitcreateroom.SetActive(false);
            afterhitjoinroom.SetActive(false);
            room.SetActive(false);
            failsomething.SetActive(false);
            faillogin.SetActive(false);
            failpasswordnotmatch.SetActive(false);
        }

        public void Login(string lgi)
        {

            if(id.text == "" || password.text == "")
            {
                inputallfild.SetActive(true);
            }
            else
            {
                lgi = id.text + "#" + password.text;

                regisName.text = usertextname.text;

                SocketEvent socketEvent = new SocketEvent("Login", lgi);

                string toJsonStr = JsonUtility.ToJson(socketEvent);

                ws.Send(toJsonStr);

                login.SetActive(false);         //
                loginconnected.SetActive(true); // แก้ด้วย

            }
        }

        //login
        public void Logininputallfild()
        {
            inputallfild.SetActive(false);
            login.SetActive(true);
        }

        public void LoginFail()
        {
            login.SetActive(true);
            faillogin.SetActive(false);
            loginconnected.SetActive(false);
        }

        //login

        public void Register(string rgt)
        {

            if(regisID.text == "" || regisName.text == "" || regisPassword.text == "" || regisrePassword.text == "")
            {
                failsomething.SetActive(true);              
            }
            else
            {

                if (regisPassword.text == regisrePassword.text)
                {
                    rgt = regisID.text + "#" + regisPassword.text + "#" + regisName.text;

                    SocketEvent socketEvent = new SocketEvent("Register", rgt);

                    string toJsonStr = JsonUtility.ToJson(socketEvent);

                    ws.Send(toJsonStr);

                    login.SetActive(true);
                    register.SetActive(false);
                    inputallfild.SetActive(false);
                }
                else
                {
                    failpasswordnotmatch.SetActive(true);
                }
            }
        }

        //regis
        public void Register()
        {
            failsomething.SetActive(false);
            register.SetActive(true);
            login.SetActive(false);
        }

        public void Failpassword()
        {
            failpasswordnotmatch.SetActive(false);
            register.SetActive(true);
            login.SetActive(false);
        }

        //regis

        public void AfterRegister()
        {
            login.SetActive(true);
        }

        public void Lobby()
        {

        }

        public void CreateRoom(string roomName)
        {
            roomName = createroomname.text;
            roomnameui.text = roomName;

            SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);

            room.SetActive(true);
            afterhitcreateroom.SetActive(false);
            afterhitjoinroom.SetActive(false);

        }

        public void FailCreateRoom()
        {
            failcreateroom.SetActive(false);
            room.SetActive(false);
            afterhitcreateroom.SetActive(true);
        }

        public void AfterCreatrRoom()
        {
            loginconnected.SetActive(false);
            afterhitcreateroom.SetActive(true);
        }

        public void JoinRoom(InputField roomName)
        {
            roomnameui.text = roomName.text;

            SocketEvent socketEvent = new SocketEvent("JoinRoom", roomName.text);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);

            room.SetActive(true);
            afterhitcreateroom.SetActive(false);
            afterhitjoinroom.SetActive(false);

            roomName.text = "";
            inputroomnameuser.text = "";

        }

        public void FailJoinRoom()
        {
            failjoinroom.SetActive(false);
            room.SetActive(false);
            afterhitjoinroom.SetActive(true);

        }

        public void AfterJoinRoom()
        {
            loginconnected.SetActive(false);
            afterhitjoinroom.SetActive(true);
        }

        public void LeaveRoom()
        {
            room.SetActive(false);
            loginconnected.SetActive(true);

            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);

            textOwner.text = "";
            textReceive.text = "";
        }

        public void Quit()
        {
            room.SetActive(false);
            login.SetActive(true);
        }

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }

        public new void SendMessage(string message)
        {
            //แก้ไขบางอย่าง
            if (inputText.text == "" || ws.ReadyState != WebSocketState.Open)
                return;

            MessageData newMessageData = new MessageData();
            newMessageData.username = username.text;
            newMessageData.message = inputText.text;

            string toJsonStr = JsonUtility.ToJson(newMessageData);


            ws.Send(toJsonStr);
            inputText.text = "";
        }

        private void OnDestroy()
        {
            if (ws != null)
            {
                ws.Close();
            }
        }

        private void UpdateNotifyMessage()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(tempMessageString);
                if(receiveMessageData.eventName == "Login")
                {
                    if(OnLogin != null)
                       OnLogin(receiveMessageData);
                    if(receiveMessageData.data != "fail")
                    {
                        faillogin.SetActive(false);
                    }
                    else
                    {
                        faillogin.SetActive(true);
                    }
                }

                else if(receiveMessageData.eventName == "Register")
                {
                    if (OnRegister != null)
                        OnRegister(receiveMessageData);
                    if(receiveMessageData.data != "fail")
                    {
                        failpasswordnotmatch.SetActive(false);
                    }
                    else
                    {
                        failpasswordnotmatch.SetActive(true);
                    }
                }

                else if (receiveMessageData.eventName == "CreateRoom")
                {
                    if (OnCreateRoom != null)
                        OnCreateRoom(receiveMessageData);
                    if(receiveMessageData.data != "fail")
                    {
                        failcreateroom.SetActive(false);
                    }
                    else
                    {
                        failcreateroom.SetActive(true);
                    }
                }

                else if(receiveMessageData.eventName == "SendMessage")
                {
                    if(OnSendMessage != null)
                    {
                        OnSendMessage(receiveMessageData);
                    }


                    if (receiveMessageData.data == inputText.text)
                    {
                        textOwner.text += receiveMessageData.data + ": " + receiveMessageData.data + "\n";
                    }
                    else
                    {
                        textReceive.text += receiveMessageData.data + ": " + receiveMessageData.data + "\n";
                    }

                    tempMessageString = "";
                }

                else if (receiveMessageData.eventName == "JoinRoom")
                {
                    if (OnJoinRoom != null)
                        OnJoinRoom(receiveMessageData);
                    if (receiveMessageData.data != "fail")
                    {
                        failjoinroom.SetActive(false);
                    }
                    else
                    {
                        failjoinroom.SetActive(true);
                    }
                }
                else if (receiveMessageData.eventName == "LeaveRoom")
                {
                    if (OnLeaveRoom != null)
                        OnLeaveRoom(receiveMessageData);
                }

                tempMessageString = "";
            }
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            Debug.Log(messageEventArgs.Data);

            tempMessageString = messageEventArgs.Data;
        }
    }
}

