using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SAVE
{
    public static string shotPath = $"{Application.persistentDataPath}/Shot";

    static string GetPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    #region PlayerPrefs
    public static void PlayerPrefsSave(string key, object data)
    {
        //将各种数据类型存储为String
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }
    public static string PlayerPrefsLoad(string key)
    {
        //参数2 null 为没有数据是默认返回值
        return PlayerPrefs.GetString(key, null);
    }
    #endregion


    #region JSON
    public static void JsonSave(string fileName, object data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath(fileName), json);
#if UNITY_EDITOR
        Debug.Log($"已保存至{GetPath(fileName)}");
#endif
    }

    public static T JsonLoad<T>(string fileName)
    {
        string path = GetPath(fileName);
        //如果文件存在就读取
        if (File.Exists(path))
        {
            string json = File.ReadAllText(GetPath(fileName));
            var data = JsonUtility.FromJson<T>(json);
            Debug.Log($"读取{fileName}");
            return data;
        }
        else
        {
            Debug.Log($"Return Default");
            return default;
        }
    }

    public static void JsonDelete(string fileName)
    {
        File.Delete(GetPath(fileName));
    }
    #endregion


    #region 截图

    public static void CameraCapture(int i, Camera camera, Rect rect)
    {
        //不存在文件夹就新建
        if (!Directory.Exists(SAVE.shotPath))
            Directory.CreateDirectory(SAVE.shotPath);
        string path = Path.Combine(SAVE.shotPath, $"{i}.png");

        int w = (int)rect.width;
        int h = (int)rect.height;

        RenderTexture rt = new RenderTexture(w, h, 0);
        //将相机渲染的内容存到指定的RenderTexture
        camera.targetTexture = rt;
        camera.Render();

        ////多相机测试
        //Camera c2 = camera.GetUniversalAdditionalCameraData().cameraStack[0];
        //c2.targetTexture=rt;
        //c2.Render();


        //激活指定RenderTexture
        RenderTexture.active = rt;

        //参数4：mipChain多级渐远纹理
        Texture2D t2D = new Texture2D(w, h, TextureFormat.RGB24, true);

        //防止截黑屏,但可能会导致截错(?)
        //yield return new WaitForEndOfFrame();
        //把RenderTexture的像素读到Texture2D
        t2D.ReadPixels(rect, 0, 0);
        t2D.Apply();

        //存成PNG
        byte[] bytes = t2D.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        //用完重置、销毁    
        camera.targetTexture = null;
        //c2.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);
    }


    public static Sprite LoadShot(int i)
    {
        var path = Path.Combine(shotPath, $"{i}.png");

        Texture2D t = new Texture2D(640, 360);
        t.LoadImage(GetImgByte(path));
        return Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
    }


    static byte[] GetImgByte(string path)
    {
        FileStream s = new FileStream(path, FileMode.Open);
        byte[] imgByte = new byte[s.Length];
        s.Read(imgByte, 0, imgByte.Length);
        s.Close();
        return imgByte;
    }

    public static void DeleteShot(int i)
    {
        var path = Path.Combine(shotPath, $"{i}.png");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"删除截图{i}");
        }
    }

    #endregion
}
