using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : Inventory
{
    public bool hasInit = false;

    public int[] itemIdArray;

    private Button CloseButton;

    //public KnapsackPanel knapsackPanel;

    public override void Start()
    {
        base.Start();

        this.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(OnSellClick);
        this.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(OnBuyClick);
        CloseButton = transform.Find("CloseButton").GetComponent<Button>();
        CloseButton.onClick.AddListener(OnCloseClick);
        inventoryMng.shopPanel = this;
        this.gameObject.SetActive(false);
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
        bool isSuccess = view.knapsackPanel.ConsumeCoin(item.BuyPrice);
        if (isSuccess)
        {
            view.knapsackPanel.StoreItem(item);
        }
    }

    /// <summary>
    /// 点击售卖按钮
    /// </summary>
    private void OnSellClick()
    {
        if (inventoryMng.IsSellItem)
        {
            inventoryMng.IsSellItem = false;
            GameManager.Instance.HideToolTip();
        }
        else
        {
            inventoryMng.InitBool();
            inventoryMng.IsSellItem = true;
            GameManager.Instance.ShowToolTip("出售");
        }
    }

    /// <summary>
    /// 点击购买按钮
    /// </summary>
    private void OnBuyClick()
    {
        if (inventoryMng.IsBuyItem)
        {
            inventoryMng.IsBuyItem = false;
            GameManager.Instance.HideToolTip();
        }
        else
        {
            inventoryMng.InitBool();
            inventoryMng.IsBuyItem = true;
            GameManager.Instance.ShowToolTip("购买");
        }
    }

    private void OnCloseClick()
    { 
        if (uiMng == null)
        {
            uiMng = GameObject.Find("View").GetComponent<View>().uiMng;
        }
        inventoryMng.InitBool();
        view.HideToolTip();
        uiMng.HideInventory(UIPanelType.ShopPanel);

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
