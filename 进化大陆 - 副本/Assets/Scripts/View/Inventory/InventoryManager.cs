using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : BaseManager
{
    public InventoryManager(View view)
    {
        this.view = view;
    }

    //#region 单例模式
    //private static InventoryManager _instance;

    //public static InventoryManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            //下面的代码只会执行一次
    //            _instance = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    //        }
    //        return _instance;
    //    }
    //}
    //#endregion

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
        pickedItem.Hide();
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
        //gameFacade.HideToolTip();
        //如果我们捡起了物品，我们就要让物品跟随鼠标
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
        pickedItem.SetLocalPosition(position);
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
    }

    public void SaveInventory()
    {
        view.knapsackPanel.SaveInventory();
        characterPanel.SaveInventory();
    }

    //public void LoadInventory()
    //{
    //    knapsackPanel.LoadInventory();
    //    characterPanel.LoadInventory();

    //}

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
        view.knapsackPanel.StoreItem(item);
    }
}