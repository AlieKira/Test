using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class HeadPortraitPanel : BasePanel
{
    private Button CloseButton;
    private Button ConfirmButton;
    private GameObject headPortraitItem;
    private GridLayoutGroup Layout;
    private SetHeadPortraitRequest setHeadPortraitRequest;
    private HeadPortraitItem itemData;
    private HeadPortraitItem currentItem=null;
    private Image image;
    private void Start()
    {
        setHeadPortraitRequest = GetComponent<SetHeadPortraitRequest>();
        headPortraitItem = Resources.Load("UIPanel/HeadPortraitItem") as GameObject;
        Layout = transform.Find("ImageList/Layout").GetComponent<GridLayoutGroup>();
        CloseButton = transform.Find("CloseButton").GetComponent<Button>();
        ConfirmButton = transform.Find("ConfirmButton").GetComponent<Button>();
        CloseButton.onClick.AddListener(OnCloseClick);
        ConfirmButton.onClick.AddListener(OnConfirmClick);
        LoadHeadPortraitItem(uiMng.headPortraitPathDic);
    }

    public void OnSetHeadPortraitResponse(ReturnCode returnCode) {
        if (returnCode==ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("头像设置失败");
        }
        else
        {
            uiMng.PushPanelSync(UIPanelType.RoomListPanel);
        }
    }

    private void OnCloseClick()
    {
        uiMng.PopPanel();
    }

    private void OnConfirmClick()
    {
        uiMng.RemoveLastPanel();
        if (currentItem.HeadPortraitPath=="n")
        {
            return;
        }
        UserData userData = gameFacade.GetUserData();
        setHeadPortraitRequest.SendRequest(userData.userID.ToString(), currentItem.HeadPortraitPath);
    }

    public void SetCurrtenItem(HeadPortraitItem item)
    {
        if (currentItem != null)
        {
            currentItem.transform.Find("Image").GetComponent<Image>().CrossFadeAlpha(0.5f, 0.1f, false);
        }
        currentItem = item;
        currentItem.transform.Find("Image").GetComponent<Image>().CrossFadeAlpha(0, 0.1f, false);
    }
    public void LoadHeadPortraitItem(Dictionary<HeadPortraitType, string> headPortraitPathDic)
    {
        int count1 = headPortraitPathDic.Count/3;
        int count2 = headPortraitPathDic.Count % 3;
        int count = count1 + count2;
        foreach (var temp in headPortraitPathDic)
        {
            GameObject roomItem = GameObject.Instantiate(headPortraitItem);
            roomItem.transform.SetParent(Layout.transform,false);
            itemData = roomItem.GetComponent<HeadPortraitItem>();
            itemData.headPortraitPanel = this;
            itemData.SetPath(temp.Value);
            image = roomItem.GetComponent<Image>();
            image.sprite = Resources.Load<Sprite>(temp.Value);
        }
        Vector2 size = Layout.GetComponent<RectTransform>().sizeDelta;
        Layout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            count * (headPortraitItem.GetComponent<RectTransform>().sizeDelta.y + Layout.spacing.y));
    }
    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnPause()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnExit()
    {
        this.gameObject.SetActive(false);
    }
}
