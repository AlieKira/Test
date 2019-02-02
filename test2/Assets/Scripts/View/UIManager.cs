using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class UIManager : BaseManager
{
    public View view;

    public UIManager(View view)
    {
        this.view = view;
    }

    private Transform canvasTansform;

    public Transform CanvasTransform
    {
        get
        {
            if (canvasTansform == null)
            {
                canvasTansform = GameObject.Find("Canvas").transform;
            }

            return canvasTansform;
        }
    }

    private Stack<BasePanel> panelStack;
    public Dictionary<UIPanelType, BasePanel> panelDic = new Dictionary<UIPanelType, BasePanel>();
    public Dictionary<UIPanelType, BasePanel> inventoryDic = new Dictionary<UIPanelType, BasePanel>();
    private Dictionary<UIPanelType, string> panelPathDic;
    private UIPanelType pushedPanel = UIPanelType.none;

    public override void OnInit()
    {
        ParseUIPanelTypeJson();
        PushPanel(UIPanelType.OriginalPanel);
        //CharacterPanel panel = GetPanel(UIPanelType.CharacterPanel) as CharacterPanel;
        //panel.inventoryMng = view.GetInventoryMng();
        //panel.OnStart();
        //ShopPanel panel2 = GetPanel(UIPanelType.ShopPanel) as ShopPanel;
        //panel2.inventoryMng = view.GetInventoryMng();
        //panel2.OnStart();
    }

    public BasePanel PushPanel(UIPanelType uiPanelType)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(uiPanelType);

        panel.OnEnter();
        panelStack.Push(panel);
        return panel;
    }

    public void PopPanel()
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        if (panelStack.Count <= 0)
        {
            return;
        }

        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();
        if (panelStack.Count <= 0)
        {
            return;
        }

        BasePanel panel = panelStack.Peek();
        panel.OnResume();
    }

    public void ShowPanel(UIPanelType type,InventoryManager mng)
    {
        BasePanel panel;
        if (panelDic.ContainsKey(type))
        {
            panelDic.TryGetValue(type, out panel);
            Inventory mPanel = GetPanel(type) as Inventory;
            if (!inventoryDic.ContainsKey(type))
            {
                inventoryDic.Add(type, mPanel);
            }
            panel.OnResume();
        }
        else
        {
             Inventory mPanel = GetPanel(type) as Inventory;
            if (!inventoryDic.ContainsKey(type))
            {
                inventoryDic.Add(type, mPanel);
            }
            mPanel.inventoryMng = mng;
            mPanel.OnEnter();
        }

        if (panelStack.Count<=1)
        {
            return;
        }
        GamePanel gamePanel = panelStack.Peek() as GamePanel;
        gamePanel.OnPause();
        gamePanel.PauseButton(type);
    }

    public void HidePanel(UIPanelType type)
    {
        if (type == UIPanelType.All)
        {
            foreach (Inventory temp in inventoryDic.Values)
            {
                temp.OnPause();
            }
            inventoryDic.Clear();
            panelStack.Peek().OnResume();
            return;
        }
        if (inventoryDic.ContainsKey(type))
        {
            BasePanel peekPanel = panelStack.Peek();
            foreach (var temp in inventoryDic)
            {
                if (temp.Key == type)
                {
                    temp.Value.OnExit();
                }
            }
            inventoryDic.Remove(type);
            if (inventoryDic.Count <= 0)
            {
                peekPanel.OnResume();
            }
        }
        else
        {
            Debug.Log("HideInventory has error," + type);
        }
    }


    public BasePanel GetPanel(UIPanelType uiPanelType)
    {
        if (panelDic == null)
        {
            panelDic = new Dictionary<UIPanelType, BasePanel>();
        }
        BasePanel panel;
        if (panelDic.ContainsKey(uiPanelType) == true)
        {
            panelDic.TryGetValue(uiPanelType, out panel);
            return panel;
        }
        if (panelPathDic.ContainsKey(uiPanelType) == false)
        {
            Debug.LogWarning("UIManager/Push has error.The uiPanelType is not exist");
        }
        string path = null;
        panelPathDic.TryGetValue(uiPanelType, out path);
        GameObject instPanel = GameObject.Instantiate(Resources.Load(path) as GameObject);
        instPanel.transform.SetParent(CanvasTransform, false);
        instPanel.GetComponent<BasePanel>().UIMng = this;
        panelDic.Add(uiPanelType, instPanel.GetComponent<BasePanel>());
        return instPanel.GetComponent<BasePanel>();
    }


    public void RemoveLastPanel()
    {
        BasePanel TopPanel = panelStack.Peek();
        BasePanel lastPanel = panelStack.Pop();
        string st = lastPanel.gameObject.name.Replace("(Clone)", null);
        lastPanel.OnExit();
        panelDic.Remove((UIPanelType)Enum.Parse(typeof(UIPanelType), st));

    }


    //public void ShowTimer()
    //{
    //    gamePanel.ShowTimer();
    //}



    public void ShowCareerPanel()
    {
        PushPanel(UIPanelType.CareerPanel);
    }

    //public void SetEditor(float m_BG_Volume, float m_Effectsound_Volume, Difficulty difficulty)
    //{
    //    editorPanel.SetEditor(m_BG_Volume,m_Effectsound_Volume,difficulty);
    //}

    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }

    private void ParseUIPanelTypeJson()
    {
        panelPathDic = new Dictionary<UIPanelType, string>();
        TextAsset ta = Resources.Load<TextAsset>("UIPanelInfo/UIPanelTypeInfo");
        UIPanelTypeJson panelTypeJson = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);

        foreach (UIPanelInfo info in panelTypeJson.infoList)
        {
            panelPathDic.Add(info.uiPanelType, info.path);
        }
    }
}