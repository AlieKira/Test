using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class UIPanelInfo : ISerializationCallbackReceiver
{
    [NonSerialized]
    public UIPanelType uiPanelType;

    public string panelTypeString;
    public string path;

    public void OnBeforeSerialize(){}

    public void OnAfterDeserialize()
    {
        UIPanelType temp = (UIPanelType)Enum.Parse(typeof(UIPanelType), panelTypeString);
        uiPanelType = temp;
    }

}
