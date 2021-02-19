using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace ChatwithJson
{
    public class JustTest : MonoBehaviour
    {
        public GameObject connection;
        public GameObject messageui;

        public InputField inputField;
        public Text sendtext;
        public Text receivetext;

        private WebSocket ws;

        private string tempMesString;

        public void Start()
        {
            connection.SetActive(true);
            messageui.SetActive(false);
        }

        public void Connect()
        {
            string url = $"ws://127.0.0.1:25500/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            connection.SetActive(false);
            messageui.SetActive(true);
        }

        public void Disconnect()
        {
            if (ws != null )
            {
                ws.Close();
            }
        }

        public void SendMessage()
        {
            if(inputField.text == "" || ws.ReadyState != WebSocketState.Open)
            {
                return;
            }
            ws.Send(inputField.text);
            inputField.text = "";
        }

        private void OnDestroy()
        {
            if (ws != null)
                ws.Close();
        }

        private void Update()
        {
            if (tempMesString != "")
            {
                receivetext.text += tempMesString + "\n";
                tempMesString = "";
            }
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {

            tempMesString = messageEventArgs.Data;
        }

    }
}

