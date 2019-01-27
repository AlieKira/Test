using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RecordData
{
    private string[] easyRecords = new string[3] { "0,00:00", "0,00:00", "0,00:00" };

    private string[] commonRecords = new string[3] { "0,00:00", "0,00:00", "0,00:00" };

    private string[] difficultRecords = new string[3] { "0,00:00", "0,00:00", "0,00:00" };

    private Dictionary<Difficulty, string[]> recordDic = new Dictionary<Difficulty, string[]>();

    private Difficulty recoreType = Difficulty.Easy;

    private float m_BG_Volume;

    private float m_Effectsound_Volume;

    public void OnStart()
    {
        recordDic.Add(Difficulty.Easy, easyRecords);
        recordDic.Add(Difficulty.Common, commonRecords);
        recordDic.Add(Difficulty.Difficult, difficultRecords);
    }

    /// <summary>
    /// 对记录进行排序
    /// </summary>
    /// <param name="i"></param>
    /// <param name="level"></param>
    /// <param name="minute"></param>
    /// <param name="second"></param>
    /// <param name="record"></param>
    private void Sorting(int i, int level, int minute, int second, string[] records)
    {
        Debug.Log("sort");
        string str = level + "," + minute.ToString("00") + ":" + second.ToString("00");
        for (int j = 2; j > i; j--)
        {
            if (i!=records.Length)
            {
                records[j] = records[j - 1];
            }
        }
        records[i] = str;
        SaveRecord();
    }

    /// <summary>
    /// 更新记录
    /// </summary>
    /// <param name="recordType"></param>
    /// <param name="level"></param>
    /// <param name="minute"></param>
    /// <param name="second"></param>
    public int UpdateRecord( int level, int minute, int second)
    {
        string[] records = new string[3]{"","",""};
        recordDic.TryGetValue(recoreType, out records);
        for (int i = 0; i < records.Length; i++)
        {
            string[] strs = records[i].Split(',');
            if (level >= int.Parse(strs[0]))
            {
                //若关卡更多，则排序。若关卡相同则继续对比
                if (level == int.Parse(strs[0]))
                {
                    //若分钟数更少，则排序。若分钟数相同则继续对比
                    string[] strs2 = strs[1].Split(':');
                    if (minute == int.Parse(strs2[0]))
                    {
                        //若秒数更少，则排序。若秒数相同则从后一名开始排序
                        if (second == int.Parse(strs2[1]))
                        {
                            Sorting(i , level, minute, second, records);
                            return i + 1;
                        }
                        else if (second < int.Parse(strs2[1]))
                        {
                            Sorting(i, level, minute, second, records);
                            return i + 1;
                        }
                    }
                    else if (minute < int.Parse(strs2[0]))
                    {
                        Sorting(i, level, minute, second, records);
                        return i + 1;
                    }
                }
                else if (level > int.Parse(strs[0]))
                {
                    Sorting(i, level, minute, second, records);
                    return i+1;
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// 保存记录
    /// </summary>
    public void SaveRecord()
    {
        StringBuilder sb = new StringBuilder();
        foreach (string temp in easyRecords)
        {
            sb.Append(temp.ToString());
            sb.Append("-");
        }

        sb.Remove(sb.Length - 1, 1);
        sb.Append("|");
        foreach (string temp in commonRecords)
        {
            sb.Append(temp);
            sb.Append("-");
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append("|");
        foreach (string temp in difficultRecords)
        {
            sb.Append(temp);
            sb.Append("-");
        }
        sb.Remove(sb.Length - 1, 1);
        PlayerPrefs.SetString("record", sb.ToString());
        Debug.Log("save");
    }

    /// <summary>
    /// 加载记录
    /// </summary>
    public void LoadRecord()
    {
        string str = PlayerPrefs.GetString("record");
        if (str=="")
        {
            return;
        }
        string[] strs = str.Split('|');
        string[] ezStrs = strs[0].Split('-');
        string[] cmStrs = strs[1].Split('-');
        string[] dfStrs = strs[2].Split('-');
        for (int i = 0; i < ezStrs.Length; i++)
        {
            easyRecords[i] = ezStrs[i];
        }
        for (int i = 0; i < cmStrs.Length; i++)
        {
            commonRecords[i] = cmStrs[i];
        }
        for (int i = 0; i < dfStrs.Length; i++)
        {
            difficultRecords[i] = dfStrs[i];
        }
    }

    /// <summary>
    /// 保存游戏设置
    /// </summary>
    public void SaveSetting()
    {
        string s = m_BG_Volume + "," + m_Effectsound_Volume + "," + (int) recoreType;
        PlayerPrefs.SetString("Editor", s);
    }

    /// <summary>
    /// 加载游戏设置
    /// </summary>
    public string LoadSetting()
    {
        Debug.Log("loadsetting");
        string s = PlayerPrefs.GetString("Editor");
        if (s=="")
        {
            return "1,1,Easy";
        }
        m_BG_Volume = float.Parse(s.Split(',')[0]);
        m_Effectsound_Volume = float.Parse(s.Split(',')[1]);
        recoreType = (Difficulty) Enum.Parse(typeof(Difficulty), s.Split(',')[2]);
        return s;
    }

    public string[] GetEasyRecords()
    {
        return easyRecords;
    }

    public string[] GetCommonRecords()
    {
        return commonRecords;
    }

    public string[] GetDifficultRecords()
    {
        return difficultRecords;
    }

    public void SetEditor(string s)
    {
        m_BG_Volume = float.Parse(s.Split(',')[0]);
        m_Effectsound_Volume = float.Parse(s.Split(',')[1]);
        recoreType = (Difficulty)Enum.Parse(typeof(Difficulty), s.Split(',')[2]);
        SaveSetting();
    }

    public int GetDifficultyDegree()
    {
        switch (recoreType)
        {
            case Difficulty.Easy:
                return 1;
            case Difficulty.Common:
                return 2;
            case Difficulty.Difficult:
                return 3;
        }

        return 1;
    }

    public Difficulty GetDifficulty()
    {
        return recoreType;
    }
}

