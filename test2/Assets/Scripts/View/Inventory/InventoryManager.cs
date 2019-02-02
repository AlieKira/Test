using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : BaseManager
{
    public InventoryManager(View view)
    {
        this.view = view;
    }

    /// <summary>
    ///  物品信息的列表（集合）
    /// </summary>
    private List<Item> itemList;


    public bool IsSellItem { get; set; }

    public bool IsBuyItem { get; set; }

    public bool isUseItem { get; set; }

    private Canvas canvas;

    public ShopPanel shopPanel;

    public CharacterPanel characterPanel;

    public KnapsackPanel knapsackPanel;

    public View view;

    #region PickedItem
    private bool isPickedItem = false;

    public bool IsPickedItem
    {
        get
        {
            return isPickedItem;
        }
    }

    private ItemUI pickedItem;//鼠标选中的物体

    public ItemUI PickedItem
    {
        get
        {
            return pickedItem;
        }
    }
    #endregion


    public override void OnInit()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pickedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
        EventCenter.AddListener(EventType.OverGame, ClearItem);
        pickedItem.Hide();
    }

    private void ClearItem()
    {
        characterPanel.ClearItem();
        knapsackPanel.ClearItem();
    }

    public override void OnUpdate()
    {
        if (isPickedItem)
        {
            //如果我们捡起了物品，我们就要让物品跟随鼠标
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            pickedItem.SetLocalPosition(position);
        }

        //物品丢弃的处理
        if (isPickedItem && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == false)
        {
            isPickedItem = false;
            PickedItem.Hide();
        }

        if (isUseItem)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            pickedItem.SetLocalPosition(position);
        }
    }





    //捡起物品槽指定数量的物品
    public void PickupItem(Item item, int amount)
    {
        PickedItem.SetItem(item, amount);
        isPickedItem = true;
        PickedItem.Show();
        //如果我们捡起了物品，我们就要让物品跟随鼠标
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
        pickedItem.SetLocalPosition(position);
        EventCenter.Broadcast(EventType.PlayNormalSound,Audios.Sound_PickUp);
    }

    /// <summary>
    /// 从手上拿掉一个物品
    /// </summary>
    public void RemoveItem(int amount = 1)
    {
        PickedItem.ReduceAmount(amount);
        if (PickedItem.Amount <= 0)
        {
            isPickedItem = false;
            PickedItem.Hide();
        }
        EventCenter.Broadcast(EventType.PlayNormalSound, Audios.Sound_PutDown);
    }


    public void InitBool()
    {
        isPickedItem = false;
        IsSellItem = false;
        IsBuyItem = false;
        isUseItem = false;
        PickedItem.Hide();
    }

    public void StoreItem(Item item)
    {
        Debug.Log("Mng");
       knapsackPanel.StoreItem(item);
        EventCenter.Broadcast(EventType.PlayNormalSound, Audios.Sound_PutDown);
    }
}