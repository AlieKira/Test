using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    private Model model;
    private View view;
    private CameraManager cameraManager;
    private AudioManager audioManager;
    private PoolManager poolManager;
    private GameObject animation;

    private FSMSystem fsm;

    private GameObject role;

    private MonsterTypeInfo monsterTypeInfo;

    private RoleTypeInfo roletypeInfo;

    private int gameLevel;

    private List<GameObject> monsterList;

    private int time = 0;

    private Thread timer;

    private bool isCloseTimer = false;

    private bool isRunTimer = false;

    private bool isPlaySound = false;

    private CanvasGroup AnimMask;

    private void Awake()
    {
        model = GameObject.Find("Model").GetComponent<Model>();
        view = GameObject.Find("View").GetComponent<View>();
    }

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        cameraManager = new CameraManager();
        audioManager = new AudioManager();
        poolManager = new PoolManager();
        animation=GameObject.Find("Animation");
        if (AnimMask == null)
        {
            AnimMask = GameObject.Find("AnimMask").GetComponent<CanvasGroup>();
        }


        LoadData();
        cameraManager.OnInit();
        audioManager.OnInit();

        AddListener();
    }

    private void AddListener()
    {
        EventCenter.AddListener(EventType.StartGame, StartGame);
        EventCenter.AddListener<RoleType>(EventType.StartPlay, StartPlay);
        EventCenter.AddListener(EventType.PauseGame, PauseGame);
        EventCenter.AddListener(EventType.ResumeGame, ResumeGame);
        EventCenter.AddListener(EventType.OverGame, OverGame);
        EventCenter.AddListener(EventType.ExitGame, ExitGame);

        EventCenter.AddListener(EventType.ReturnMenuPanel, ReturnMenuPanel);
        EventCenter.AddListener<UIPanelType>(EventType.PushPanel, PushPanel);
        EventCenter.AddListener(EventType.PopPanel, PopPanel);
        EventCenter.AddListener<UIPanelType>(EventType.ShowPanel, ShowPanel);
        EventCenter.AddListener<UIPanelType>(EventType.HidePanel, HidePanel);

        EventCenter.AddListener<GameObject>(EventType.MonsterTakeDamage, MonsterTakeDamage);
        EventCenter.AddListener<GameObject>(EventType.MonsterDead, MonsterDead);
        EventCenter.AddListener(EventType.RoleTakeDamage, RoleTakeDamage);
        EventCenter.AddListener<int, int, int, int, int>(EventType.UpdateAttri, UpdateAttri);
        EventCenter.AddListener<int>(EventType.RecoverHp, RecoverHp);
        EventCenter.AddListener(EventType.StopRoleControl, StopRoleControl);
        EventCenter.AddListener(EventType.ControlRole, ControlRole);


        EventCenter.AddListener<string>(EventType.PlayBGSound, PlayBgSound);
        EventCenter.AddListener<string>(EventType.PlayNormalSound, PlayNormalSound);
    }

    private void Update()
    {
        audioManager.OnUpdate();
    }

    public void StartGame()
    {
        AnimMask.alpha = 0;
        time = 0;
        Model.Instance.UpdateGameTime(time);
        gameLevel = 1;
        isRunTimer = false;
        cameraManager.ShowMainCamera();
        PushPanel(UIPanelType.GamePanel);
        PushPanel(UIPanelType.CareerPanel);
        Time.timeScale = 1;
    }

    public void StartPlay(RoleType type)
    {
        animation.SetActive(false);
        SpawnRole(type);
        SpawnMonster();
        role.GetComponent<PlayerControl>().enabled = true;
        if (timer == null)
        {
            RunGameTimer();
        }
    }

    public void PauseGame()
    {

        isRunTimer = false;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isRunTimer = true;
        Time.timeScale = 1;
    }

    public void OverGame()
    {
        int minute = time / 60;
        int second = time % 60;
        string gameTimeString = minute.ToString("00") + ":" + second.ToString("00");
        int i = model.UpdateRecord(gameLevel, minute, second);
        view.GameOver(gameLevel.ToString(), gameTimeString, i);
        isRunTimer = false;
        time = 0;
        Model.Instance.UpdateGameTime(time);
    }

    public void SpawnRole(RoleType roleType)
    {
        roletypeInfo = model.GetRoleTypeInfo(roleType);
        GameObject temp = Resources.Load<GameObject>(roletypeInfo.prefabPath);
        role = Instantiate(temp, new Vector3(0, 0.5f, 0), Quaternion.identity);
        cameraManager.FollowRole(role);
        role.GetComponent<RoleData>().SetAttri(roletypeInfo);
        switch (roleType)
        {
            case RoleType.Archer:
                role.AddComponent<ArcherControl>().enabled = false;
                break;
            case RoleType.Mage:
                role.AddComponent<MageControl>().enabled = false;
                break;
            default:
                role.AddComponent<PlayerControl>().enabled = false;
                break;
        }
        Model.Instance.SetRoleData(role.GetComponent<RoleData>());

    }

    public void SpawnMonster()
    {
        monsterList = new List<GameObject>();
        int a = (gameLevel + 5) / 5;
        int degree = a * model.GetDifficultyDegree();
        int monsterType = (gameLevel - 1) % 5;
        monsterTypeInfo = model.GetMonsterTypeInfo((MonsterType)monsterType);
        while (true)
        {
            GameObject go = poolManager.GetInst(((MonsterType)monsterType).ToString());
            if (go == null)
            {
                break;
            }
            monsterList.Add(go);
        }
        for (int i = 0; i < monsterList.Count; i++)
        {
            Vector3 pos = new Vector3();
            while (true)
            {
                pos = new Vector3(Random.Range(-12, 12), 0, Random.Range(-8, 8));
                if (Vector3.Distance(pos, role.transform.position) > 5)
                {
                    break;
                }
            }
            monsterList[i].transform.position = pos;
            monsterList[i].GetComponent<NPCControl>().enabled = true;
            monsterList[i].GetComponent<NPCControl>().InitFSM();
            monsterList[i].GetComponent<NPCControl>().ClearTarget();
            monsterList[i].GetComponent<MonsterData>().SetAttri(monsterTypeInfo, degree);
            monsterList[i].GetComponent<MonsterData>().Init();
        }
    }


    public void RoleTakeDamage()
    {
        role.GetComponent<PlayerControl>().TakeDamage(monsterTypeInfo.attack);
    }

    public void MonsterTakeDamage(GameObject target)
    {
        target.GetComponentInParent<NPCControl>().TakeDamage(role.GetComponent<RoleData>().Damage);
    }

    public void MonsterDead(GameObject monster)
    {
        monster.transform.GetComponent<NPCControl>().enabled = false;
        role.GetComponent<RoleData>().AddExp(monsterTypeInfo.exp);
        //GameObject.Destroy(monster, 3f);
        StartCoroutine(HideMonster(monster));
        monsterList.Remove(monster);
        if (monsterList.Count <= 0)
        {
            gameLevel++;
            Model.Instance.gameLevel = gameLevel;
            SpawnMonster();
        }
    }

    IEnumerator HideMonster(GameObject monster)
    {
        yield return new WaitForSeconds(3);
        monster.SetActive(false);
       // monster.GetComponent<MonsterData>().Init();
    }

    public void RunGameTimer()
    {
        timer = new Thread(TimerThread);
        timer.Start();
    }

    private void TimerThread()
    {
        while (true)
        {
            if (isRunTimer)
            {
                Thread.Sleep(1000);
                if (isRunTimer)
                {
                    time++;
                    Model.Instance.UpdateGameTime(time);
                }
            }

            if (isCloseTimer)
            {
                break;
            }
        }
    }

    public void ReturnMenuPanel()
    {
        animation.SetActive(true);
        Time.timeScale = 1;
        cameraManager.StopFollowRole();
        cameraManager.ShowTimeLine();
        GameObject.Destroy(role);
        foreach (GameObject mons in monsterList)
        {
            mons.SetActive(false);
        }
        monsterList = new List<GameObject>();
        time = 0;
        isRunTimer = false;
        Model.Instance.UpdateGameTime(time);
    }

    public int[] GetLevelAndTime()
    {
        int[] temp = new int[] { gameLevel, time };
        return temp;
    }
    private void OnApplicationQuit()
    {
        isCloseTimer = true;
    }

    public void HideToolTip()
    {

        view.HideToolTip();
    }

    public void UpdateAttri(int strength, int intellect, int agility, int stamina, int attack)
    {
        role.GetComponent<RoleData>().UpdateAttri(strength, intellect, agility, stamina, attack);

    }

    public void RecoverHp(int hp)
    {
        role.GetComponent<RoleData>().AddHp(hp);
    }

    public void PlayBgSound(string soundName)
    {
        audioManager.PlayBgSound(soundName);
    }

    public void PlayNormalSound(string soundName)
    {
        audioManager.PlayNormalSound(soundName);
    }

    public void MenuEdit(float BG_Volume, float Effectsound_Volume, Difficulty difficulty)
    {
        audioManager.bgSoundValue = BG_Volume;
        audioManager.normalSoundValue = Effectsound_Volume;
        string s = BG_Volume + "," + Effectsound_Volume + "," + difficulty.ToString();
        model.SetEditor(s);
    }

    public void LoadData()
    {
        model.LoadRecord();
        string s = model.LoadSetting();
        audioManager.bgSoundValue = float.Parse(s.Split(',')[0]);
        audioManager.normalSoundValue = float.Parse(s.Split(',')[1]);
    }

    public string GetSetting()
    {
        string s = audioManager.bgSoundValue + "," + audioManager.normalSoundValue + "," +
                   model.GetDifficulty().ToString();
        return s;
    }

    private void SetEditor(string s)
    {
        model.SetEditor(s);
    }

    private void PushPanel(UIPanelType type)
    {
        view.PushPanel(type);
    }

    private void PopPanel()
    {
        view.PopPanel();
    }

    private void ShowPanel(UIPanelType type)
    {
        view.ShowPanel(type);
    }

    private void HidePanel(UIPanelType type)
    {
        view.HidePanel(type);
    }

    private void StopRoleControl()
    {
        if (role!=null)
        {
            role.GetComponent<PlayerControl>().enabled = false;
        }
   
    }

    private void ControlRole()
    {
        if (role != null)
        {
            role.GetComponent<PlayerControl>().enabled = true;
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }


}
