using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SocketModel
{
    /// <summary>
    /// 一级协议 用于区分所属模块
    /// </summary>
    public byte Type { get; set; }
    /// <summary>
    /// 二级协议 用于区分 模块下所属子模块
    /// </summary>
    public int Area { get; set; }
    /// <summary>
    /// 三级协议  用于区分当前处理逻辑功能
    /// </summary>
    public int Command { get; set; }
    /// <summary>
    /// 三级协议  用于区分当前处理逻辑功能
    /// </summary>
    public object Message { get; set; }

    public SocketModel() { }
    public SocketModel(byte type, int area, int command, object message)
    {
        this.Type = type;
        this.Area = area;
        this.Command = command;
        this.Message = message;
    }

    public T GetMessage<T>()
    {
        return (T)Message;
    }
}

