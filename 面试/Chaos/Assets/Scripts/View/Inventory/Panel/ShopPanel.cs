using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : Inventory
{
    public bool hasInit = false;

    public int[] itemIdArray;

    private Button CloseButton;

    public override void OnStart()
    {
        base.OnStart();
        this.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(OnSellClick);
        this.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(OnBuyClick);
        CloseButton = transform.Find("CloseButton").GetComponent<Button>();
        CloseButton.onClick.AddListener(OnCloseClick);
    }

    private void InitShop()
    {
        foreach (int itemId in itemIdArray)
        {
            StoreItem(itemId);
        }

        hasInit = true;
    }
    /// <summary>
    /// 主角购买
    /// </summary>
    /// <param name="item"></param>
    public void BuyItem(Item item)
    {
        bool isSuccess = inventoryMng.knapsackPanel.ConsumeCoin(item.BuyPrice);  
        if (isSuccess)
        {
            inventoryMng.knapsackPanel.StoreItem(item);
        }
    }

    /// <summary>
    /// 点击售卖按钮
    /// </summary>
    private void OnSellClick()
    {
        if (inventoryMng.IsPickedItem)
        {
            return;
        }
        OnClick();
        if (inventoryMng.IsSellItem)
        {
            inventoryMng.IsSellItem = false;
            EventCenter.Broadcast(EventType.HideToolTip);
        }
        else
        {
            inventoryMng.InitBool();
            inventoryMng.IsSellItem = true;
            EventCenter.Broadcast(EventType.ShowToolTip, "出售", Vector2.zero);
        }
    }

    /// <summary>
    /// 点击购买按钮
    /// </summary>
    private void OnBuyClick()
    {
        if (inventoryMng.IsPickedItem)
        {
            return;
        }
        OnClick();
        if (inventoryMng.IsBuyItem)
        {
            inventoryMng.IsBuyItem = false;
            EventCenter.Broadcast(EventType.HideToolTip);
        }
        else
        {
            inventoryMng.InitBool();
            inventoryMng.IsBuyItem = true;
            EventCenter.Broadcast(EventType.ShowToolTip, "购买", Vector2.zero);
        }
    }

    private void OnCloseClick()
    {
        OnClick();
        if (inventoryMng.IsPickedItem)
        {
            return;
        }
        EventCenter.Broadcast(EventType.HidePanel, UIPanelType.ShopPanel);

    }

    public override void OnEnter()
    {
        base.OnEnter();
        OnStart();
    }

    public override void OnResume()
    {
        base.OnResume();
        if (!hasInit)
        {
            InitShop();
        }
    }
}
