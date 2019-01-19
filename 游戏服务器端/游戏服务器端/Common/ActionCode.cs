using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ActionCode
    {
        none,
        Login,
        Register,
        ListRoom,
        CreateRoom,
        JoinRoom,
        UpdateRoom,
        ExitRoom,
        StartGame,
        ShowTimer,
        StartPlay,
        ChooseCareer,
        Move,
        Attack,
        BeAttack,
        MonsterTakeDamage,
        RoleTakeDamage,
        MonsterDead,
        RoleDead,
        LevelUp,
        GameOver,
        UpdateResult,
        QuitBattle,
        SetHeadPortrait,
        PrepareGame,
        ShowCareerTimer,
        SpawnMonster,
        MonsterReborn,
        SyncAnimation,
        MonsterMove,
        MonsterAttack
    }
}
