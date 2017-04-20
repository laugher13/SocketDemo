using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;

public class NetIO {

   
    private  static NetIO _instance;

    private Socket socket;
    private string host = "127.0.0.1";
    private int port = 7326;
    private byte[] readBuff = new byte[1024];

    List<byte> cache = new List<byte>();
    private bool isReading = false;

    public static NetIO Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new NetIO();
            }
            return _instance;
        }
    }

    private NetIO()
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(host, port);
            socket.BeginReceive(readBuff, 0, 1024, SocketFlags.None, ReceiveCallback, readBuff);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }     
    }
    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            int lenght = socket.EndReceive(ar);
            byte[] message = new byte[lenght];
            Buffer.BlockCopy(readBuff, 0, message, 0, lenght);
            cache.AddRange(message);
            if (!isReading)
            {
                isReading = true;
                OnData();
            }
            socket.BeginReceive(readBuff, 0, 1024, SocketFlags.None, ReceiveCallback, readBuff);
        }
        catch (Exception e)
        {
            Debug.Log("远程服务器断开连接");
            socket.Close();
        }      
    }

    public void write(byte type,int area,int command,object message)
    {
        ByteArray ba = new ByteArray();
        ba.write(type);
        ba.write(area);
        ba.write(command);
        //判断消息体是否为空  不为空则序列化后写入
        if (message != null)
        {
            ba.write(SerializeUtil.Encode(message));
        }
        ByteArray ba1 = new ByteArray();
        ba1.write(ba.Length);
        ba1.write(ba.GetBuff());
        try
        {
            socket.Send(ba1.GetBuff());
        }
        catch (Exception e)
        {
            Debug.Log("网络错误！");
            //Debug.Log(e.Message);
        }
    }

    private void OnData()
    {
        byte[] result = LDecode(ref cache);
        if (result == null)
        {
            isReading = false;
            return;
        }
        SocketModel message = MDecode(result);
        if (message == null)
        {
            isReading = false;
            return;
        }
        
        //尾递归
        OnData();

    }

    /// <summary>
    /// 长度解码
    /// </summary>
    /// <param name="cache"></param>
    /// <returns></returns>
    public static byte[] LDecode(ref List<byte> cache)
    {
        if (cache.Count < 4) return null;

        MemoryStream ms = new MemoryStream(cache.ToArray());//创建内存流对象，并将缓存数据写入进去
        BinaryReader br = new BinaryReader(ms);//二进制读取流
        int length = br.ReadInt32();//从缓存中读取int型消息体长度
        //如果消息体长度 大于缓存中数据长度 说明消息没有读取完 等待下次消息到达后再次处理
        if (length > ms.Length - ms.Position)
        {
            return null;
        }
        //读取正确长度的数据
        byte[] result = br.ReadBytes(length);
        //清空缓存
        cache.Clear();
        //将读取后的剩余数据写入缓存
        cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
        br.Close();
        ms.Close();
        return result;
    }
    /// <summary>
    /// 消息解码
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static SocketModel MDecode(byte[] value)
    {
        ByteArray ba = new ByteArray(value);
        SocketModel model = new SocketModel();
        byte type;
        int area;
        int command;
        //从数据中读取 三层协议  读取数据顺序必须和写入顺序保持一致
        ba.read(out type);
        ba.read(out area);
        ba.read(out command);
        model.Type = type;
        model.Area = area;
        model.Command = command;
        //判断读取完协议后 是否还有数据需要读取 是则说明有消息体 进行消息体读取
        if (ba.Readnable)
        {
            byte[] message;
            //将剩余数据全部读取出来
            ba.read(out message, ba.Length - ba.Position);
            //反序列化剩余数据为消息体
            model.Message = SerializeUtil.Decode(message);
        }
        ba.Close();
        return model;
    }
}
