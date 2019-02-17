using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        get { return isPickedItem; }
    }

    private ItemUI pickedItem; //鼠标选中的物体

    public ItemUI PickedItem
    {
        get { return pickedItem; }
    }

    #endregion


    public override void OnInit()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pickedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
        EventCenter.AddListener(EventType.OverGame, ClearItem);
        EventCenter.AddListener(EventType.ReturnMenuPanel, ClearItem);
        pickedItem.Hide();
    }

    private void ClearItem()
    {
        characterPanel.ClearItem();
        knapsackPanel.ClearItem();
    }

    public override void OnUpdate()
    {
        //if (isPickedItem)
        //{
        //    //如果我们捡起了物品，我们就要让物品跟随鼠标
        //    Vector2 position;
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
        //    pickedItem.SetLocalPosition(position);
        //}

#if UNITY_EDITOR
        if (true)
        {
            if (isPickedItem)
            {
                //如果我们捡起了物品，我们就要让物品跟随鼠标
                Vector2 position;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                    Input.mousePosition, null, out position);
                pickedItem.SetLocalPosition(position);
                if (Input.GetMouseButtonUp(0))
                {
#else
        if (Input.touchCount > 0)
        {

            Touch myTouch = Input.touches[0];
            if (isPickedItem)
            {
                //if (myTouch.phase != TouchPhase.Ended && myTouch.phase != TouchPhase.Canceled)
                //{
                //如果我们捡起了物品，我们就要让物品跟随鼠标
                Vector2 position;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                    Input.touches[0].position, null, out position);
                pickedItem.SetLocalPosition(position);
                if (myTouch.phase == TouchPhase.Ended)
                {

#endif
                    isPickedItem = false;
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.mousePosition;
                    GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
                    List<RaycastResult> results = new List<RaycastResult>();
                    gr.Raycast(pointerEventData, results);
                    if (results.Count <= 0)
                    {
                        isPickedItem = false;
                        PickedItem.Hide();
                        return;
                    }
                    GameObject go = results[0].gameObject;
                    if (go.tag == "Slot")
                    {

                        Slot slot = go.GetComponent<Slot>();
                        if (slot is EquipmentSlot)
                        {
                            EquipmentSlot equipmentSlot = slot as EquipmentSlot;
                            if (equipmentSlot.IsRightItem(pickedItem.Item))
                            {
                                if (slot.transform.childCount > 0)
                                {
                                    int amount = slot.transform.GetChild(0).GetComponent<ItemUI>().Amount;
                                    Item item = slot.transform.GetChild(0).GetComponent<ItemUI>().Item;
                                    for (int i = 0; i < pickedItem.Amount; i++)
                                    {
                                        GameObject.DestroyImmediate(slot.transform.GetChild(0).gameObject);
                                        slot.StoreItem(pickedItem.Item);
                                    }

                                    for (int i = 0; i < amount; i++)
                                    {
                                        knapsackPanel.StoreItem(item);
                                    }
                                }
                                else
                                {

                                    slot.StoreItem(pickedItem.Item);
                                }

                            }
                            else
                            {
                                knapsackPanel.StoreItem(pickedItem.Item);
                            }

                            isPickedItem = false;
                            PickedItem.Hide();
                        }
                        else if (slot is ShopSlot)
                        {
                            knapsackPanel.GetComponent<KnapsackPanel>().SellItem(pickedItem);
                            isPickedItem = false;
                            pickedItem.Hide();
                        }
                        else
                        {
                            if (slot.transform.childCount > 0)
                            {
                                int amount = slot.transform.GetChild(0).GetComponent<ItemUI>().Amount;
                                Item item = slot.transform.GetChild(0).GetComponent<ItemUI>().Item;
                                for (int i = 0; i < pickedItem.Amount; i++)
                                {
                                    GameObject.DestroyImmediate(slot.transform.GetChild(0).gameObject);
                                    slot.StoreItem(pickedItem.Item);
                                }

                                for (int i = 0; i < amount; i++)
                                {
                                    knapsackPanel.StoreItem(item);
                                }

                                isPickedItem = false;
                                PickedItem.Hide();
                            }
                            else
                            {
                                for (int i = 0; i < pickedItem.Amount; i++)
                                {
                                    slot.StoreItem(pickedItem.Item);
                                }

                                isPickedItem = false;
                                PickedItem.Hide();
                            }
                        }
                    }
                    else
                    {
                        if (go.tag == "ShopPanel")
                        {
                            knapsackPanel.SellItem(pickedItem);
                        }
                        else
                        {
                            for (int i = 0; i < pickedItem.Amount; i++)
                            {
                                knapsackPanel.StoreItem(pickedItem.Item);
                            }
                        }
                        isPickedItem = false;
                        PickedItem.Hide();
                    }
                }
            }
        }
    }


    ////物品丢弃的处理
    //if (isPickedItem && Input.GetMouseButtonDown(0) &&
    //    UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == false)
    //{
    //    isPickedItem = false;
    //    PickedItem.Hide();
    //}
    //#else
    //        if (Input.touchCount > 0)
    //        {

    //            Touch myTouch = Input.touches[0];
    //            if (isPickedItem)
    //            {
    //                if (myTouch.phase != TouchPhase.Ended && myTouch.phase != TouchPhase.Canceled)
    //                {
    //                    //如果我们捡起了物品，我们就要让物品跟随鼠标
    //                    Vector2 position;
    //                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.touches[0].position, null, out position);
    //                    pickedItem.SetLocalPosition(position);
    //                }
    //                else
    //                {
    //                    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
    //                    {
    //                        isPickedItem = false;
    //                        PickedItem.Hide();
    //                    }
    //                    else
    //                    {
    //                        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    //                        pointerEventData.position = Input.mousePosition;
    //                        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
    //                        List<RaycastResult> results = new List<RaycastResult>();
    //                        gr.Raycast(pointerEventData, results);
    //                        if (results.Count != 0)
    //                        {
    //                            results[0].gameObject.GetComponent<ItemUI>().SetItem(pickedItem.Item,pickedItem.Amount);
    //                        }

    //                        isPickedItem = false;

    //                    }
    //                }
    //            }
    //        }



    ////物品丢弃的处理
    //if (isPickedItem && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == false)
    //{
    //    isPickedItem = false;
    //    PickedItem.Hide();
    //}

    //if (isUseItem)
    //{
    //    Vector2 position;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
    //    pickedItem.SetLocalPosition(position);
    //}






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
        EventCenter.Broadcast(EventType.PlayNormalSound, Audios.Sound_PickUp);
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