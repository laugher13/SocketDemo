using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static  class ExtensionFunction
{
    public static void Write(this MonoBehaviour mono, byte type, int area, int command, object message)
    {
       NetIO.Instance.write(type, area, command, message);
    }

}
