using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ToolTipItem: BasePanel,IPointerEnterHandler,IPointerExitHandler
{
    private GamePanel gamePanel;
    public string message;
    public Vector2 offset;

    public void SetMessage(string data)
    {
        message = data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiMng.ShowToolTip(message,offset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiMng.HideToolTip();
    }

}
