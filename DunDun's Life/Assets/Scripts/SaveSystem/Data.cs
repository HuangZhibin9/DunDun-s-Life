using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    #region 单例
    public static Data Instance;
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
    public Transform dog;
    public float gameTime;

    public class SaveData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float gameTime;
    }

    SaveData ForSave()
    {
        var saveData = new SaveData();
        saveData.Position = dog.position;
        saveData.Rotation = dog.rotation;
        saveData.gameTime = gameTime;
        return saveData;
    }

    void ForLoad(SaveData saveData)
    {
        dog.position = saveData.Position;
        dog.rotation = saveData.Rotation;
        gameTime = saveData.gameTime;
    }

    public void Save(int ID)
    {
        SAVE.JsonSave(RecordData.Instance.recordName[ID], ForSave());
    }
    public void Load(int ID)
    {
        var saveData = SAVE.JsonLoad<SaveData>(RecordData.Instance.recordName[ID]);
        ForLoad(saveData);
    }
    public SaveData ReadForShow(int id)
    {
        return SAVE.JsonLoad<SaveData>(RecordData.Instance.recordName[id]);
    }
    public void Delete(int id)
    {
        SAVE.JsonDelete(RecordData.Instance.recordName[id]);
    }
}
