using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using UnityEngine.UI;

namespace testMid
{
    public class MidTermTest : MonoBehaviour
    {
        public struct SocketEvent
        {
            public string studentID;
            public string eventName;
            public string token;
            public string data;
            public string link;
            public string ans;


            public SocketEvent (string studentID, string eventName, string token,string data,string ans,string link)
            {
                this.studentID = studentID;
                this.eventName = eventName;
                this.token = token;
                this.data = data;
                this.link = link;
                this.ans = ans;
            }

        }

        public string[] words = { "Hello","","",};
        public int[] numbers = { 10,00,00, };
        public float[] nos = { 10.1f,00.0f,00.0f,};

        //public string myToken;
        string tempData;
        string result;
        int num;

        

        public int findAns;

        public string index;

        public InputField studentidandtokenInput;
        public InputField ansInput;

        private WebSocket ws;

        private void Start()
        {
            string url = "ws://gi455-305013.an.r.appspot.com/";

            ws = new WebSocket(url);

            ws.Connect();

            ws.OnMessage += OnMessage;


        }

        void FindNum()
        {
            char[] words = index.ToCharArray();
            string oddword = "";

            for(int i = 0; i < words.Length;i++)
            {
                if(i % 2 != 0)
                {
                    oddword += words[i];
                }
            }
            Debug.Log("Ans" + oddword);
        }

        private void Update()
        {
            UpdateNotify();
        }

        public void Count()
        {
            for(int j = 0; j < numbers.Length; j++)
            {
                if( j == 5)
                {
                    num = numbers[j];
                    break;
                }
            }

            for(int i = 0; i < words.Length; i++)
            {
                if(i == 1)
                {
                    result = words[i];
                    break;
                }
            }
            Debug.Log("Words : " + result + "{} Numbers : " + num.ToString());
        }

        public void Token(string studentID)
        {
            studentID = studentidandtokenInput.text;

            SocketEvent getStudentData = new SocketEvent();

            getStudentData.eventName = "RequestToken";

            getStudentData.studentID = studentID;

            string requestDataToJson = JsonUtility.ToJson(getStudentData);

            ws.Send(requestDataToJson);
        }

        public void StudentDataButton(string studentID)
        {
            studentID = studentidandtokenInput.text;

            SocketEvent getStudentData = new SocketEvent();

            getStudentData.eventName = "GetStudentData";

            getStudentData.studentID = studentID;

            string requestDataToJson = JsonUtility.ToJson(getStudentData);

            ws.Send(requestDataToJson);
        }

        /*public void StartExam()
        {
            if (ansInput.text != "")
            {
                SocketEvent examData = new SocketEvent();
                examData.eventName = "StartExam";
                examData.token = studentidandtokenInput.text;
                examData.link = "";

                string stringlink = JsonUtility.ToJson(examData);
                ws.Send(stringlink);

                Debug.Log(stringlink);
            }

            else
            {
                Debug.Log("do some thing");
            }
        }*/

        public void Answer()
        {
            if(ansInput.text != "")
            {
                SocketEvent sendAnswerData = new SocketEvent();
                sendAnswerData.eventName = "SendAnswer";
                sendAnswerData.token = studentidandtokenInput.text;
                sendAnswerData.ans = ansInput.text;

                string stringAns = JsonUtility.ToJson(sendAnswerData);
                ws.Send(stringAns);

                Debug.Log(stringAns);
            }

            else
            {
                Debug.Log("do some thing");
            }
        }

        public void ExamInfo()
        {
            SocketEvent getStudentData = new SocketEvent();

            getStudentData.eventName = "RequestExamInfo";

            getStudentData.token = studentidandtokenInput.text;

            string requestDataToJson = JsonUtility.ToJson(getStudentData);

            ws.Send(requestDataToJson);
        }

        private void UpdateNotify()
        {
            if (string.IsNullOrEmpty(tempData) == false)
            {
                var receiveTempData = JsonUtility.FromJson<SocketEvent>(tempData);

                switch (receiveTempData.eventName)
                {
                    case "RequestToken":
                        {
                            Debug.Log("RequestToken");
                            //Debug.Log("RequestToken" + receiveTempData.token);
                            //myToken = receiveTempData.token;//
                            break;
                        }
                    case "GetStudentData":
                        {
                            Debug.Log("GetStudentData");
                            //Debug.Log("GetStudentData" + receiveTempData.data);
                            break;
                        }

                    /*case "StartExam":
                        {
                            Debug.Log("StartExam : " + receiveTempData.data);
                            break;
                        }*/

                    case "RequestExamInfo":
                        {
                            Debug.Log("RequestExamInfo");
                            break;
                        }
                    case "SendAnswer":
                        {
                            Debug.Log("Your Answer is " + receiveTempData.ans);
                            break;
                        }
                }
            }

            tempData = "";
        }

        public void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            tempData = messageEventArgs.Data;
            Debug.Log("Message from server : " + messageEventArgs.Data);
        }
    }
}

