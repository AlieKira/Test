using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class HeadPortraitInfo:ISerializationCallbackReceiver
{
    [NonSerialized]
    public HeadPortraitType headProtraitType;
    public string imageTypeString;
    public string path;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        HeadPortraitType temp = (HeadPortraitType) Enum.Parse(typeof(HeadPortraitType), imageTypeString);
        headProtraitType = temp;
    }

}

