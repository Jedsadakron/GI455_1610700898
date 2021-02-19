using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.UI;

namespace Lobby
{
    public class Lobby : MonoBehaviour
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

        struct SocketEvent
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

        public GameObject createrooms;
        public GameObject aftercreaterooms;
        public InputField roomname;
        public GameObject joinrooms;
        public GameObject failpopup;

        public Text sendText;
        public Text receiveText;

        private WebSocket ws;

        private string tempMessageString;

        public void Start()
        {
            entername.SetActive(true);
            createandjoin.SetActive(false);
            room.SetActive(false);
        }

        //lobby
        public void Connect()
        {
            string url = $"ws://127.0.0.1:25500/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            entername.SetActive(false);
            createandjoin.SetActive(true);
            room.SetActive(false);

            CreateRoom("TestRoom01");
        }

        public void BeforeConnect()
        {
            //
        }

        //before lobby
        public void CreateRoom(string roomName)
        {

            if (ws.ReadyState == WebSocketState.Open)
            {
                SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName);

                string jsonStr = JsonUtility.ToJson(socketEvent);

                ws.Send(jsonStr);
            }
        }

        //join
        public void JoinRoom(string roomName)
        {

        }

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }

        public void SendMessage()
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
                ws.Close();
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                MessageData receiveMessageData = JsonUtility.FromJson<MessageData>(tempMessageString);

                if (receiveMessageData.username == inputText.text)
                {
                    sendText.text += receiveMessageData.username + ": " + receiveMessageData.message + "\n";
                }
                else
                {
                    receiveText.text += receiveMessageData.username + ": " + receiveMessageData.message + "\n";
                }

                tempMessageString = "";
            }
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            Debug.Log(messageEventArgs.Data);

            //tempMessageString = messageEventArgs.Data;
        }
    }
}

