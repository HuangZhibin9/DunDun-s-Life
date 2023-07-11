using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public SaveData _SaveData_for_sceneloaded;

    public class SaveData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public string SceneName;
        public float gameTime;
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    SaveData ForSave()
    {
        var saveData = new SaveData();
        saveData.Position = dog.position;
        saveData.Rotation = dog.rotation;
        saveData.gameTime = gameTime;
        saveData.SceneName = SceneManager.GetActiveScene().name;
        return saveData;
    }

    void ForLoad(SaveData saveData)
    {
        _SaveData_for_sceneloaded = saveData;
        if (SceneManager.GetActiveScene().name == saveData.SceneName)
        {
            dog = GameObject.Find("dog").transform;
            dog.position = _SaveData_for_sceneloaded.Position;
            dog.rotation = _SaveData_for_sceneloaded.Rotation;
            gameTime = _SaveData_for_sceneloaded.gameTime;
        }
        else
        {
            SceneManager.LoadScene(saveData.SceneName);
        }
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "FirstScene")
        {
            dog = GameObject.Find("dog").transform;
            dog.position = _SaveData_for_sceneloaded.Position;
            dog.rotation = _SaveData_for_sceneloaded.Rotation;
            gameTime = _SaveData_for_sceneloaded.gameTime;
        }
    }
}
