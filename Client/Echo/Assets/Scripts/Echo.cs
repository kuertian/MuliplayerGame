using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Echo : MonoBehaviour {
    //定义套接字
    Socket socket;
    //UGUI
    public InputField InputFeld;
    public Text text;
    //接收缓冲区
    byte[] readBuff = new byte[1024];
    string recStr = "";

    //点击连接按钮
    public void ConnectBtn()
    {
        //Socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Connect
        socket.BeginConnect("127.0.0.1", 8888, ConnectCallBack, socket);
    }
    //Connect回调
    public void ConnectCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket Connect Succ");
            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.LogError("Socket Connect fail" + ex.ToString());
        }
    }
    //Receive回调
    public void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndReceive(ar);
            string s = System.Text.Encoding.Default.GetString(readBuff, 0, count);
            recStr = s + "\n" + recStr;

            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.LogError("socket Receive fail" + ex.ToString());
        }

    }
    //点击发送按钮
    public void SendBtn()
    {
        //Send
        string sendStr = InputFeld.text;
        byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
        socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallBack, socket);
    }
    //Send回调
    public void SendCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndSend(ar);
            Debug.Log("Socket Send Succ" + count);
        }
        catch (SocketException ex)
        {
            Debug.LogError("Socket Send fail" + ex.ToString());
        }
    }
	// Update is called once per frame
	void Update () {
        if(!string.IsNullOrEmpty(recStr))
        {
            text.text = recStr;
        }        
	}
}
