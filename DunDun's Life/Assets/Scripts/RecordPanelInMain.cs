using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RecordPanelInMain : MonoBehaviour
{
    public Transform grid;               //档位父对象
    public GameObject recordPrefab;      //档位预制体
    public GameObject recordPanel;      //存档面板【控制显示/隐藏】

    [Header("存档详情")]
    public GameObject detail;//存档详情
    //public Camera camera_main;
    public Image screenShot;            //截图
    public Text gameTime;               //时长

    //Key：存档文件名     Value：存档序号
    Dictionary<string, int> RecordInGrid = new Dictionary<string, int>();
    bool isLoad = true;     //正在读档

    private void Start()
    {
        //生成指定数量存档位
        for (int i = 0; i < RecordData.recordNum; i++)
        {
            GameObject obj = Instantiate(recordPrefab, grid);
            //改序号
            obj.name = (i + 1).ToString();
            obj.GetComponent<RecordUI>().SetID(i + 1);

            //如果该档位有存档，就改名，默认名为空档
            if (RecordData.Instance.recordName[i] != "")
            {
                obj.GetComponent<RecordUI>().SetName(i);
                //存到字典里
                RecordInGrid.Add(RecordData.Instance.recordName[i], i);
            }
        }

        #region 监听
        RecordUI.OnLeftClick += LeftClickGrid;
        RecordUI.OnRightClick += RightClickGrid;
        RecordUI.OnEnter += ShowDetails;
        RecordUI.OnExit += HideDetails;
        #endregion

        //设置时间
        TimeMgr.SetOriTime();
    }

    private void OnDestroy()
    {
        RecordUI.OnLeftClick -= LeftClickGrid;
        RecordUI.OnRightClick -= RightClickGrid;
        RecordUI.OnEnter -= ShowDetails;
        RecordUI.OnExit -= HideDetails;
    }

    private void Update()
    {
        TimeMgr.SetCurTime();
    }


    //RecordUI.OnEnter调用
    void ShowDetails(int i)
    {
        //读取存档，但不改变玩家数据，仅用于显示
        var data = Data.Instance.ReadForShow(i);
        gameTime.text = $"游戏时长  {TimeMgr.GetFormatTime((int)data.gameTime)}";
        screenShot.sprite = SAVE.LoadShot(i);
        //显示详情
        detail.SetActive(true);
    }

    //RecordUI.OnExit调用
    void HideDetails()
    {
        //隐藏详情
        detail.SetActive(false);
    }

    void _Return()
    {

    }

    //左击
    void LeftClickGrid(int ID)
    {

        //读档
        if (isLoad)
        {
            //空档什么都不做
            if (RecordData.Instance.recordName[ID] == "")
                return;
            else
            {
                //读取该存档，更新玩家数据
                Data.Instance.Load(ID);
                //更新当前存档ID，保存到存档数据库
                RecordData.Instance.lastID = ID;
                RecordData.Instance.Save();
                //修改时间
                TimeMgr.SetOriTime();
            }
        }
    }

    //右击删除
    void RightClickGrid(int gridID)
    {
        if (RecordData.Instance.recordName[gridID] == "")
            return;

        //存档不为空就删除
        else
            DeleteRecord(gridID, false);

    }

    private void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }





    //true为覆盖模式，false为纯删除模式
    void DeleteRecord(int i, bool isCover = true)
    {
        //删除存档文件
        Data.Instance.Delete(i);
        //删字典
        RecordInGrid.Remove(RecordData.Instance.recordName[i]);

        if (!isCover)
        {
            //清空存档名
            RecordData.Instance.recordName[i] = "";
            //更新UI
            grid.GetChild(i).GetComponent<RecordUI>().SetName(i);
            //删除截图
            SAVE.DeleteShot(i);
            //隐藏详情
            HideDetails();
        }
    }
}
