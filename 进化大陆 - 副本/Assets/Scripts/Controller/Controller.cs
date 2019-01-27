using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    [HideInInspector] public Model model;
    [HideInInspector] public View view;
    [HideInInspector] public CameraManager cameraManager;
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public AudioManager audioManager;

    private FSMSystem fsm;

    private GameObject role;

    private MonsterTypeInfo monsterTypeInfo;

    private RoleTypeInfo roletypeInfo;

    private int gameLevel;

    private List<GameObject> monsterList = new List<GameObject>();

    private int time = 0;

    private Thread timer;

    private bool isPlaySound = false;

    private void Awake()
    {
        model = GameObject.Find("Model").GetComponent<Model>();
        view = GameObject.Find("View").GetComponent<View>();
        view.model = model;
        //cameraManager = GetComponent<CameraManager>();
        //audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        cameraManager = new CameraManager();
        audioManager = new AudioManager();

        LoadData();
        cameraManager.OnInit();
        audioManager.OnInit();

    }

    private void Update()
    {
        audioManager.OnUpdate();
    }

    public void SpawnRole(RoleType roleType)
    {

        roletypeInfo = model.GetRoleTypeInfo(roleType);
        GameObject temp = Resources.Load<GameObject>(roletypeInfo.prefabPath);
        role = Instantiate(temp, Vector3.zero, Quaternion.identity);
        role.GetComponent<RoleData>().SetAttri(roletypeInfo);
        model.roleData = role.transform.GetComponent<RoleData>();
        view.SetHeadPortrait();
        cameraManager.FollowRole(role);
    }

    public void StartGame()
    {
        gameLevel = 1;
        cameraManager.ShowMainCamera();
        view.uiMng.gamePanel = view.PushPanel(UIPanelType.GamePanel) as GamePanel;
        view.UpdateGameTimer(time);
        view.PushPanel(UIPanelType.CareerPanel);

    }

    public void PauseGame()
    {
        //isPause = true;
        if (timer!=null)
        {
            timer.Abort();
        }
        Time.timeScale = 0;
    }

    public void PauseRole()
    {
        if (role != null)
        {
            role.GetComponent<PlayerControl>().enabled = false;
        }

    }

    public void ResumeRole()
    {
        if (role != null)
        {
            role.GetComponent<PlayerControl>().enabled = true;
        }
    }

    public void ResumeGame()
    {
        //isPause = false;
        timer = new Thread(TimerThread);
        timer.Start();
        Time.timeScale = 1;
    }

    public void StartPlay()
    {
        role.GetComponent<PlayerControl>().enabled = true;
        RunGameTimer();
        GameManager.Instance.UpdateRoleInterface();
    }

    public void SpawnMonster()
    {
        int a = (gameLevel + 5) / 5;
        int degree = a * model.GetDifficultyDegree();
        int monsterType = (gameLevel - 1) % 5;
        monsterTypeInfo = model.GetMonsterTypeInfo((MonsterType) monsterType);
        GameObject monsterPre = Resources.Load<GameObject>(monsterTypeInfo.prefabPath);
        for (int i = 0; i < 1; i++)
        {
            Vector3 pos = new Vector3();
            while (true)
            {
                pos = new Vector3(Random.Range(-12, 12), 0, Random.Range(-12, 12));
                if (Vector3.Distance(pos, role.transform.position) > 5)
                {
                    break;
                }
            }

            GameObject monster = Instantiate(monsterPre, pos, Quaternion.identity);
            monster.AddComponent<NPCControl>();
            monster.AddComponent<MonsterData>();
            monster.GetComponent<MonsterData>().SetAttri(monsterTypeInfo, degree);
            monsterList.Add(monster);
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
        GameObject.Destroy(monster, 3f);
        view.StroeItem(model.GetRandomItem(monsterTypeInfo.monsterType));
        monsterList.Remove(monster);
        if (monsterList.Count <= 0)
        {
            gameLevel++;
            SpawnMonster();
        }
    }

    public void GameOver()
    {
        int minute = time / 60;
        int second = time % 60;
        string gameTimeString = minute.ToString("00") + ":" + second.ToString("00");
        int i = model.UpdateRecord(gameLevel, minute, second);
        view.GameOver(gameLevel.ToString(), gameTimeString, i);
        timer.Abort();
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
            Thread.Sleep(1000);
            time++;
            view.UpdateGameTimer(time);
        }
    }

    public void ReturnMenuPanel()
    {
        Time.timeScale = 1;
        cameraManager.StopFollowRole();
        cameraManager.ShowTimeLine();
        GameObject.Destroy(role);
        foreach (GameObject mons in monsterList)
        {
            Destroy(mons);
        }

        monsterList = new List<GameObject>();
        time = 0;
    }

    public int[] GetLevelAndTime()
    {
        int[] temp = new int[] {gameLevel, time};
        return temp;
    }

    private void OnApplicationQuit()
    {
        if (timer!=null)
        {
            timer.Abort();
        }

    }

    public void ShowToolTip(string text)
    {
        view.ShowToolTip(text);
    }

    public void HideToolTip()
    {
        view.HideToolTip();
    }

    public void UpdateAttri(int strength, int intellect, int agility, int stamina, int attack)
    {
        role.GetComponent<RoleData>().UpdateAttri(strength, intellect, agility, stamina, attack);

    }

    public void UpdateRoleInterface()
    {
        int hp = role.GetComponent<RoleData>().currentHP;
        int maxHP = role.GetComponent<RoleData>().MaxHP;
        int exp = role.GetComponent<RoleData>().exp;
        int lv = role.GetComponent<RoleData>().Lv;
        view.UpdateRoleInterface(hp, maxHP, exp, lv);
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
        string s= model.LoadSetting();
        audioManager.bgSoundValue = float.Parse(s.Split(',')[0]);
        audioManager.normalSoundValue=  float.Parse(s.Split(',')[1]);
    }

    public string GetSetting()
    {
        string s = audioManager.bgSoundValue + "," + audioManager.normalSoundValue + "," +
                   model.GetDifficulty().ToString();
        return s;
    }
}
