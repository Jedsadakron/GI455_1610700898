using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.UI;

namespace ChatWebSocket
{
    public class WebSocketConnection0 : MonoBehaviour
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

        public GameObject entername;
        public GameObject createandjoin;
        public GameObject room;

        public InputField username;
        public InputField inputText;
        public InputField inputroomnameuser;

        public GameObject aftercreaterooms;
        public GameObject afterjoinroom;
        public GameObject failpopup;

        public Text userroomnameText;
        public Text roomnameui;

        private WebSocket ws;

        private string tempMessageString;
        private string _roomname;

        public delegate void DelegateHandle(SocketEvent result);
        public DelegateHandle OnCreateRoom;
        public DelegateHandle OnJoinRoom;
        public DelegateHandle OnLeaveRoom;

        public void Start()
        {
            entername.SetActive(true);
            createandjoin.SetActive(false);
            room.SetActive(false);
            failpopup.SetActive(false);
        }

        private void Update()
        {
            UpdateNotifyMessage();
        }

        public void Connect()
        {
            string url = "ws://127.0.0.1:25500/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            entername.SetActive(false);
            createandjoin.SetActive(true);
            room.SetActive(false);
        }

        public void CreateRoom(string roomName)
        {
            aftercreaterooms.SetActive(true);
            createandjoin.SetActive(false);

            roomnameui.text = ("Room : " + inputroomnameuser.text);

            /*if (inputroomnameuser.text != null)
            {
                failpopup.SetActive(true);
            }*/

            SocketEvent socketEvent = new SocketEvent("CreateRoom", inputroomnameuser.text);
                 
            string toJsonStr = JsonUtility.ToJson(socketEvent);
          
            ws.Send(toJsonStr);

        }

        /*public void CreateRoomUI()
        {

        }*/

        public void FalPOPUp()
        {
            failpopup.SetActive(false);
        }

        public void Room()
        {
            room.SetActive(true);

        }

        public void JoinRoom(string roomName)
        {

            SocketEvent socketEvent = new SocketEvent("JoinRoom", inputroomnameuser.text);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);

            afterjoinroom.SetActive(true);
            createandjoin.SetActive(false);

            roomnameui.text = ("Room : " + inputroomnameuser.text);

        }

        public void LeaveRoom()
        {
            room.SetActive(false);
            afterjoinroom.SetActive(false);
            aftercreaterooms.SetActive(false);
            createandjoin.SetActive(true);

            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }

        /*public void LeaveRoomUI()
        {

        }*/

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }

        public void SendMessages(string message)
        {
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

                if (receiveMessageData.eventName == "CreateRoom")
                {
                    if (OnCreateRoom != null)
                        OnCreateRoom(receiveMessageData);
                    if (receiveMessageData.data != "fail")
                    {
                        failpopup.SetActive(false);
                    }
                    else
                    {
                        failpopup.SetActive(true);
                    }
                }
                else if (receiveMessageData.eventName == "JoinRoom")
                {
                    if (OnJoinRoom != null)
                        OnJoinRoom(receiveMessageData);
                    if (receiveMessageData.data != "fail")
                    {
                        failpopup.SetActive(true);
                    }
                    else
                    {
                       failpopup.SetActive(false);
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


