using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class UIManager : BaseManager
{
    public UIManager(GameFacade gameFacade) : base(gameFacade)
    {
        ParseUIPanelTypeJson();
        ParseHeadPortraitTypeJson();
        ParseRoleTypeJson();
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

    private MessagePanel msgPanel;
    private ToolTipPanel toolTipPanel;
    private Stack<BasePanel> panelStack;
    private Dictionary<UIPanelType, BasePanel> panelDic;
    private Dictionary<UIPanelType, string> panelPathDic;
    public Dictionary<HeadPortraitType, string> headPortraitPathDic;
    public Dictionary<RoleType, RoleTypeData> roleTypeDic;
    private UIPanelType pushedPanel=UIPanelType.none;

    public override void OnInit()
    {
        PushPanel(UIPanelType.ToolTip);
        PushPanel(UIPanelType.MessagePanel);
        PushPanel(UIPanelType.OriginalPanel);
    }

    public override void OnUpdate()
    {
        if (pushedPanel == UIPanelType.none)
        {
            return ;
        }
        PushPanel(pushedPanel);
        pushedPanel = UIPanelType.none;
    }

    public void InjectMsgPanel(MessagePanel msgPanel)
    {
        this.msgPanel = msgPanel;
    }

    public void InjectToolTipPanel(ToolTipPanel panel)
    {
        this.toolTipPanel = panel;
    }

    public void ShowToolTip(string message, Vector2 offset)
    {
        toolTipPanel.Show(message,offset);
    }

    public void HideToolTip()
    {
        toolTipPanel.Hide();
    }

    public void PushPanelSync(UIPanelType uiPanelType)
    {
        pushedPanel = uiPanelType;
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
        instPanel.GetComponent<BasePanel>().GameFacade = gameFacade;
        panelDic.Add(uiPanelType, instPanel.GetComponent<BasePanel>());
        return instPanel.GetComponent<BasePanel>();
    }


    public void RemoveLastPanel()
    {
        BasePanel TopPanel=panelStack.Peek();
        BasePanel lastPanel = panelStack.Pop();
        string st=lastPanel.gameObject.name.Replace("(Clone)", null);
        lastPanel.OnExit();
        panelDic.Remove((UIPanelType)Enum.Parse(typeof(UIPanelType), st));
        
    }
    public void ShowMessage(string data)
    {
        if (data == null)
        {
            Debug.LogWarning("ShowMessage has error,the data is null");
        }
        msgPanel.ShowMessage(data);
    }

    public void ShowMessageSync(string data)
    {
        if (data == null)
        {
            Debug.LogWarning("ShowMessage has error,the data is null");
        }
        msgPanel.ShowMessageSync(data);
    }

    public string GetHeadPortraitPath(HeadPortraitType type)
    {
        if (headPortraitPathDic == null)
        {
            ParseHeadPortraitTypeJson();
        }

        string path;
        headPortraitPathDic.TryGetValue(type, out path);
        return path;
    }

    public void SetHouseOwnerSync(RoomPanel panel)
    {
        panel.SetHouseOwnerSync();
    }

    public void SetHouseGuestSync(RoomPanel panel,string data)
    {
        panel.SetHouseGuestSync(data);
    }

    public RoleTypeData GetRoleTypeData(RoleType roleType)
    {
        RoleTypeData temp;
        roleTypeDic.TryGetValue(roleType, out temp);
        return temp;
    }
        

    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }

    private void ParseUIPanelTypeJson()
    {
        panelPathDic = new Dictionary<UIPanelType, string>();
        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");
        UIPanelTypeJson panelTypeJson = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);

        foreach (UIPanelInfo info in panelTypeJson.infoList)
        {
            panelPathDic.Add(info.uiPanelType, info.path);
        }
    }
    [Serializable]
    class HeadPortraitTypeJson
    {
        public List<HeadPortraitInfo> infoList;
    }

    private void ParseHeadPortraitTypeJson()
    {
        headPortraitPathDic = new Dictionary<HeadPortraitType, string>();
        TextAsset ta = Resources.Load<TextAsset>("HeadPortraitType");
        HeadPortraitTypeJson panelTypeJson = JsonUtility.FromJson<HeadPortraitTypeJson>(ta.text);

        foreach (HeadPortraitInfo info in panelTypeJson.infoList)
        {
            headPortraitPathDic.Add(info.headProtraitType, info.path);
        }
    }
    [Serializable]
    class RoleTypeJson
    {
        public List<RoleInfo> infoList;
    }

    private void ParseRoleTypeJson()
    {
        roleTypeDic = new Dictionary<RoleType, RoleTypeData>();
        string s = File.ReadAllText(@"E:\UnityDocuments/进化大陆/Assets/Resources/RoleType.json", Encoding.GetEncoding("utf-8"));
        //string text = s.Replace("\\n", "\n");
        RoleTypeJson t = JsonConvert.DeserializeObject<RoleTypeJson>(s);
        foreach (RoleInfo info in t.infoList)
        {
            RoleTypeData temp = new RoleTypeData(info.roleTypeString, info.prefabPath, info.imagePath, info.skill_01ImagePath,
                info.skill_02ImagePath, info.skill_03ImagePath, info.attack, info.defence, info.maxHP, info.maxMP,
                info.skill_01Message, info.skill_02Message, info.skill_03Message);
            roleTypeDic.Add(temp.RoleType, temp);
        }
    }

    
}