using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public struct StudentData
    {
        public string studentID;

        public StudentData(string studentID)
        {
            this.studentID = studentID;
        }
    }

    [System.Serializable]
    public class WsEvent
    {
        public string eventName;
    }
    [System.Serializable]
    public class EventServer : WsEvent
    {
        public string data;
    }
    [System.Serializable]
    public class EventStudent : WsEvent
    {
        public StudentData data;
    }
    [System.Serializable]
    public class Token : WsEvent
    {
        public string token;
    }
    [System.Serializable]
    public class Answer : WsEvent
    {
        public string answer;
    }

    private WebSocket ws;
    string tempData;
    public InputField StudentIDInput;
    public InputField AnswerInput;
    public string Mytoken;
    //public int[] numbers;
    //public int findAnswer;
    // Start is called before the first frame update
    void Start()
    {
        string url = "ws://gi455-305013.an.r.appspot.com/";
        ws = new WebSocket(url);
        ws.OnMessage += OnMessage;
        ws.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNotify();
    }
    public void RequestToken(string studentID)
    {
        studentID = StudentIDInput.text;
        EventStudent eventData = new EventStudent();
        eventData.eventName = "RequestToken";
        eventData.data = new StudentData(studentID);

        string toJson = JsonUtility.ToJson(eventData);
        ws.Send(toJson);
    }
    public void GetStudentDataButton(string studentID)
    {
        studentID = StudentIDInput.text;
        EventStudent eventData = new EventStudent();
        eventData.eventName = "GetStudentData";
        eventData.data = new StudentData(studentID);

        string toJson = JsonUtility.ToJson(eventData);
        ws.Send(toJson);
    }
    public void GetExamInfo(string token)
    {
        token = StudentIDInput.text;
        Token eventData = new Token();
        eventData.eventName = "RequestExamInfo";
        eventData.token = Mytoken;

        string toJson = JsonUtility.ToJson(eventData);
        ws.Send(toJson);
    }
    public void SendAnswer(string answer)
    {
        answer = AnswerInput.text;
        Token eventData = new Token();
        eventData.eventName = "SendAnswer";
        eventData.token = Mytoken;

        string toJson = JsonUtility.ToJson(eventData);
        ws.Send(toJson);
    }
    //public void AnswerButton()
    //{
    //    for (int i = 0; i < numbers.Length; i++)
    //    {
    //        if (i == findAnswer)
    //        {
    //            SocketEvent sendAnswerData = new SocketEvent();
    //            sendAnswerData.eventName = "SendAnswer";
    //            sendAnswerData.token = myToken;
    //            sendAnswerData.answer = numbers[i].ToString();

    //            string stringAns = JsonUtility.ToJson(sendAnswerData);
    //            ws.Send(stringAns);

    //            Debug.Log(stringAns);
    //            break;
    //        }
    //    }
    //}

    private void UpdateNotify()
    {
        if (string.IsNullOrEmpty(tempData) == false)
        {
            EventServer receiveMessageData = JsonUtility.FromJson<EventServer>(tempData);
            if (receiveMessageData.eventName == "RequestToken")
            {
                Debug.Log("My token :" + receiveMessageData.data);
            }
            if (receiveMessageData.eventName == "GetStudentData")
            {
                Debug.Log("My data :" + receiveMessageData.data);
            }
            if (receiveMessageData.eventName == "RequestExamInfo")
            {
                Debug.Log("Exam :" + receiveMessageData.data);
            }
            if (receiveMessageData.eventName == "SendAnswer")
            {
                Debug.Log("Answer :" + receiveMessageData.data);
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
