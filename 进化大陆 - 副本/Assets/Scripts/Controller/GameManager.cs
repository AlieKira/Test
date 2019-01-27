using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 单例模式
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Controller").GetComponent<GameManager>();
            }
            return _instance;
        }
    }
    #endregion


    private Controller controller;


    public Color[] colors;

    private void Awake()
    {
        controller = GetComponent<Controller>();
    }


    public void StartGame()
    {
        controller.StartGame();
    }

    public void PauseGame()
    {
        controller.PauseGame();
    }

    public void ResumeGame()
    {
        controller.ResumeGame();
    }

    public void PauseRole()
    {
        controller.PauseRole();
    }

    public void ResumeRole()
    {
        controller.ResumeRole();
    }

    public void Spawn(RoleType roleType)
    {
        controller.SpawnRole(roleType);
        controller.SpawnMonster();
        UpdateRoleInterface();
    }

    public void StartPlay()
    {
        controller.StartPlay();
    }


    public void MenuEdit(float BG_Volume, float Effectsound_Volume, Difficulty difficulty)
    {
        controller.MenuEdit(BG_Volume, Effectsound_Volume, difficulty);
    }

    public string GetSetting()
    {
        return controller.GetSetting();
    }

    public void RoleTakeDamage()
    {
        controller.RoleTakeDamage();
    }

    public void GameOver()
    {
        controller.GameOver();
    }

    public void MonsterTakeDamage(GameObject target)
    {
        controller.MonsterTakeDamage(target);
    }

    public void MonsterDead(GameObject monster)
    {
        controller.MonsterDead(monster);
    }

    public void ReturnMenuPanel()
    {
        controller.ReturnMenuPanel();
    }

    public int[] GetLevelAndTime()
    {
        return controller.GetLevelAndTime();
    }

    public void ShowToolTip(string text)
    {
        controller.ShowToolTip(text);
    }

    public void HideToolTip()
    {
        controller.HideToolTip();
    }

    public void UpdateAttri(int strength,int intellect,int agility,int stamina,int attack)
    {
        controller.UpdateAttri(strength, intellect, agility, stamina, attack);
    }

    public void RecoverHp(int hp)
    {
        controller.RecoverHp(hp);
    }

    public void UpdateRoleInterface()
    {
        controller.UpdateRoleInterface();
    }

    public void PlayBgSound(string soundName)
    {
        controller.PlayBgSound(soundName);
    }

    public void PlayNormalSound(string soundName)
    {
        controller.PlayNormalSound(soundName);
    }
}
