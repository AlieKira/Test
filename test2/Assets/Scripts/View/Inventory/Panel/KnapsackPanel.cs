using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class KnapsackPanel : Inventory
{
    private Button CloseButton;

    private Button useButton;

    private int coinAmount = 0;

    private Text coinText;

    public int CoinAmount
    {
        get
        {
            return coinAmount;
        }
        set
        {
            coinAmount = value;
            coinText.text = coinAmount.ToString();
        }
    }


    public override void OnStart()
    {
        base.OnStart();
        inventoryMng.knapsackPanel = this;
        coinText = transform.Find("Coin/Text").GetComponent<Text>();
        useButton = transform.Find("UseButton").GetComponent<Button>();
        CloseButton = transform.Find("CloseButton").GetComponent<Button>();
        CloseButton.onClick.AddListener(OnCloseClick);
        useButton.onClick.AddListener(OnUseClick);
        EventCenter.AddListener<GameObject>(EventType.MonsterDead, GetItem);
    }

    private void OnCloseClick()
    {
        if (inventoryMng.IsPickedItem)
        {
            return;
        }
        OnClick();
        EventCenter.Broadcast(EventType.HidePanel,UIPanelType.KnapsackPanel);
    }

    /// <summary>
    /// 消费
    /// </summary>
    public bool ConsumeCoin(int amount)
    {
        if (coinAmount >= amount)
        {
            coinAmount -= amount;
            coinText.text = coinAmount.ToString();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 赚取金币
    /// </summary>
    /// <param name="amount"></param>
    public void EarnCoin(int amount)
    {
        this.coinAmount += amount;
        coinText.text = coinAmount.ToString();
    }

    /// <summary>
    /// 主角出售物品
    /// </summary>
    public void SellItem(ItemUI itemUi)
    {

        if (inventoryMng.IsSellItem)
        {
            int coinAmount = itemUi.Item.SellPrice * itemUi.Amount;
            EarnCoin(coinAmount);
            DestroyImmediate(itemUi.gameObject);
        }
    }



    /// <summary>
    /// 额外保存金币信息
    /// </summary>
    public override void SaveInventory()
    {
        base.SaveInventory();
        PlayerPrefs.SetInt("Coin",coinAmount);
    }

    //public override void LoadInventory()
    //{
    //    base.LoadInventory();
    //    CoinAmount = PlayerPrefs.GetInt("Coin");
    //}

    /// <summary>
    /// 点击使用按钮
    /// </summary>
    private void OnUseClick()
    {
        OnClick();
        if (inventoryMng.isUseItem)
        {
            inventoryMng.isUseItem = false;
            EventCenter.Broadcast(EventType.HideToolTip);
        }
        else
        {
            inventoryMng.InitBool();
            inventoryMng.isUseItem = true;
            EventCenter.Broadcast(EventType.ShowToolTip,"使用",Vector2.zero); 
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        OnStart();
    }

    public void GetItem(GameObject monster)
    {
        Item item= Model.Instance.GetRandomItem(monster.GetComponent<MonsterData>().monsterType);
        StoreItem(item);
    }

    public void ClearItem()
    {
        foreach (Slot temp in slotList)
        {
            if (temp.transform.childCount != 0)
            {
                Destroy(temp.transform.GetChild(0).gameObject);
            }
        }
    }
}
