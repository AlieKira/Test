using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class View:MonoBehaviour
{
    public Model model;

    public UIManager uiMng;

    //public KnapsackPanel knapsackPanel;

    //public CharacterPanel characterPanel;

    //public ShopPanel shopPanel;

    private InventoryManager inventoryMng;

    public ToolTip toolTip;

    private MessagePanel msgPanel;

    private Vector2 toolTipOffset = new Vector2(0, -10);

    private void Awake()
    {
        inventoryMng=new InventoryManager(this);
        uiMng=new UIManager(this);
        uiMng.OnInit();
        inventoryMng.OnInit();
        ShowPanel(UIPanelType.CharacterPanel);
        ShowPanel(UIPanelType.KnapsackPanel);
        ShowPanel(UIPanelType.ShopPanel);
        HidePanel(UIPanelType.CharacterPanel);
        HidePanel(UIPanelType.KnapsackPanel);
        HidePanel(UIPanelType.ShopPanel);

        AddListener();
    }

    private void AddListener()
    {
        EventCenter.AddListener<string,Vector2>(EventType.ShowToolTip,ShowToolTip);
        EventCenter.AddListener(EventType.HideToolTip, HideToolTip);
    }

    private void Update()
    {
        inventoryMng.OnUpdate();
    }


    public BasePanel PushPanel(UIPanelType type)
    {
        return uiMng.PushPanel(type);
    }

    public void PopPanel()
    {
        uiMng.PopPanel();
    }

    public void ShowPanel(UIPanelType type)
    {
        uiMng.ShowPanel(type,inventoryMng);
    }

    public void HidePanel(UIPanelType type)
    {
        uiMng.HidePanel(type);
        inventoryMng.InitBool();
        HideToolTip();
    }

    public void GameOver(string level,string time,int number)
    {
        OverPanel panel=uiMng.PushPanel(UIPanelType.OverPanel)as OverPanel;
        panel.ShowRecod(level,time,number);
    }

    #region ToolTip
    public void ShowToolTip(string message, Vector2 offset)
    {
        toolTip.Show(message, offset);
    }

    public void HideToolTip()
    {
        toolTip.Hide();
    }

    #endregion

    #region MessagePanel

    public void ShowMessage(string data)
    {
        if (data == null)
        {
            Debug.LogWarning("ShowMessage has error,the data is null");
        }
        msgPanel.ShowMessage(data);
    }

    #endregion

    #region GetRecords

    public string[] GetEasyRecords()
    {
        return model.GetEasyRecords();
    }

    public string[] GetCommonRecords()
    {
        return model.GetCommonRecords();
    }

    public string[] GetDifficultRecords()
    {
        return model.GetDifficultRecords();
    }

    #endregion




    public InventoryManager GetInventoryMng()
    {
        return inventoryMng;
    }

    public Item GetItemById(int id)
    {
        return model.GetItemById(id);
    }

    public void StroeItem(Item item)
    {
        inventoryMng.StoreItem(item);
    }

    public void InitInventoryMng()
    {
        inventoryMng.InitBool();
    }

    //public void UpdateGameTimer(int time)
    //{
    //    uiMng.UpdateGameTimer(time);
    //}


    //public void SetEditor(float m_BG_Volume, float m_Effectsound_Volume, Difficulty difficulty)
    //{
    //    uiMng.SetEditor(m_BG_Volume, m_Effectsound_Volume, difficulty);
    //}
}
