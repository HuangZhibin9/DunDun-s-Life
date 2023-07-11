using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordData : MonoBehaviour
{
    #region 单例
    public static RecordData Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        Load();
    }



    public const int recordNum = 3;             //存档数量
    public const string NAME = "RecordData";    //存档列表名

    public string[] recordName = new string[recordNum];     //存档文件名（不是全路径名）
    public int lastID;                                      //最新的存档名，用于自动存档

    class SaveData
    {
        public string[] recordName = new string[recordNum];
        public int lastID;
    }

    SaveData ForSave()
    {
        var saveData = new SaveData();
        for (int i = 0; i < recordNum; i++)
        {
            saveData.recordName[i] = recordName[i];
        }
        saveData.lastID = lastID;
        return saveData;
    }

    void ForLoad(SaveData saveData)
    {
        lastID = saveData.lastID;
        for (int i = 0; i < recordNum; i++)
        {
            recordName[i] = saveData.recordName[i];
        }
    }

    public void Save()
    {
        SAVE.JsonSave(NAME, ForSave());
    }
    public void Load()
    {
        ForLoad(SAVE.JsonLoad<SaveData>(NAME));
    }

}
