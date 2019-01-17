using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ToolTipPanel : BasePanel { 
    private Text toopTipText;
    private Text contentText;
    private CanvasGroup canvasGroup;
    private bool isShow=false;
    private Canvas canvas;
    private Vector2 offset;

    private void Start()
    {
        toopTipText = GetComponent<Text>();
        contentText = transform.Find("Content").GetComponent<Text>();
        canvasGroup = transform.GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasGroup.alpha = 0;
        
    }

    public override void OnEnter()
    {
        uiMng.InjectToolTipPanel(this);
    }

    private void Update()
    {
        if (isShow)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position);
            this.transform.localPosition = position + offset;
        }
    }

    public void Show(string data,Vector2 offset)
    {
        this.transform.SetAsLastSibling();
        isShow = true;
        canvasGroup.alpha = 1;
        toopTipText.text = data;
        contentText.text = data;
        this.offset = offset;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        isShow = false;
    }
    
    
}
