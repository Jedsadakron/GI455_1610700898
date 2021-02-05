using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WebSocketSharp;

namespace ProgramChat
{
    public class WebsocketConnection : MonoBehaviour
    {
        private WebSocket websocket;

        public int maxMessages = 100;

        //public string msge;

        public GameObject chatPanel01, textObject;
        public InputField chatbox;
        public InputField Ip;
        public InputField Port;

        [SerializeField]
        List<Message> messageList = new List<Message>();

        public Image ipcon;
        public GameObject uicon;

        //ip,port
        private void Start()
        {

        }

        //connect botton
        public void PressedButtonConnect()
        {
            websocket = new WebSocket("ws://127.0.0.1:25500");

            websocket.OnMessage += OnMessages;

            uicon.gameObject.SetActive(true);

            websocket.Connect();
        }

        //sendmess to ui
        private void Update()
        {
            if (chatbox.text != "")
            {            
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    if(websocket.ReadyState == WebSocketState.Open)
                    {

                        SendMessageToUi(chatbox.text);

                        chatbox.text = "";
                        Debug.Log("");
                  
                    }
                }

            }

            if(!chatbox.isFocused)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SendMessageToUi("do some thing!");

                }

            }
        }

        //send message to update
        public void SendMessageToUi(string text)
        {
            //msge = chatbox.text;

            if (messageList.Count >= maxMessages)
            {
                Destroy(messageList[0].textObject.gameObject);
                messageList.Remove(messageList[0]);
            }

            Message newMessage = new Message();

            newMessage.text = text;

            GameObject newText = Instantiate(textObject, chatPanel01.transform);

            newMessage.textObject = newText.GetComponent<Text>();

            newMessage.textObject.text = newMessage.text;

            messageList.Add(newMessage);
        }

        //ลบ ws
        public void OnDestroy()
        {
            if(websocket != null)
            {
                websocket.Close();
            }
        }

        //log show message
        public void OnMessages(object sender, MessageEventArgs messageEventArgs)
        {
            Debug.Log("Receive msg : " + messageEventArgs.Data);
        }

        public void Discon()
        {
            Application.Quit();
        }
    }

    [System.Serializable]
    public class Message
    {
        public string text;
        public Text textObject;
    }

}

