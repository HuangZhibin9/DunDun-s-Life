using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDoManager : MonoBehaviour
{
    public bool IsOpenUI = false;
    public GameObject TabCanvas;
    public GameObject TabPanel;
    public GameObject SavePanel;
    public GameObject setPanel;
    public GameObject Buttons;
    public GameObject List;

    #region 单例
    public static ToDoManager Instance;
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

    private void Update()
    {
        TabCanvas = GameObject.Find("TabCanvas");
        TabPanel = TabCanvas.transform.GetChild(0).gameObject;
        SavePanel = TabCanvas.transform.GetChild(1).gameObject;
        Buttons = TabCanvas.transform.GetChild(3).gameObject;
        List = TabPanel.transform.GetChild(2).gameObject;
        if (!IsOpenUI && Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log($"打开任务清单");
            Time.timeScale = 0;
            TabPanel.SetActive(true);
            SavePanel.SetActive(false);
            setPanel.SetActive(false);
            Buttons.SetActive(true);
            List.SetActive(true);
            TabCanvas.GetComponent<RecordPanel>().reset();
            IsOpenUI = true;
        }
        else if (IsOpenUI && Input.GetKeyDown(KeyCode.Tab))
        {
            closeTabPanel();
        }
    }

    public void closeTabPanel()
    {
        Debug.Log($"关闭任务清单");
        Time.timeScale = 1;
        TabPanel.SetActive(false);
        SavePanel.SetActive(false);
        Buttons.SetActive(false);
        setPanel.SetActive(false);
        IsOpenUI = false;
    }
}
