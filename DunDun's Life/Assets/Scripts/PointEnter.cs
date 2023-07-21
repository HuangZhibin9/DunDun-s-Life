
using UnityEngine;
using UnityEngine.EventSystems;
public class PointEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DetailManager manager;
    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.isUsing = true;
        GameObject.Find("dog").GetComponent<PlayerController>().noOnUI = false;
    }
    /// <summary>
    /// 鼠标离开调用
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        manager.isUsing = false;
        GameObject.Find("dog").GetComponent<PlayerController>().noOnUI = true;
    }
}