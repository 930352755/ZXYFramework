using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

/// <summary>
/// 工具类
/// </summary>
public static class ToolsClass
{

    #region UI UnityUGUI相关拓展

    /// <summary>
    /// 按钮添加点击事件
    /// </summary>
    /// <param name="self"></param>
    /// <param name="action">点击触发事件</param>
    public static void AddClick(this Button self, System.Action action)
    {
        self.onClick.AddListener(() => { action?.Invoke(); });
    }

    /// <summary>
    /// 根据一个屏幕坐标点设置UI位置
    /// 实现拖拽操作
    /// </summary>
    /// <param name="self">UI的Transform</param>
    /// <param name="parentTransform">UI parent object</param>
    /// <param name="screenTargetPos">UI mouse point position</param>
    /// <param name="camera">Camera</param>
    /// <param name="offset">offset</param>
    public static void SetUILocaPos(this Transform self, Transform parentTransform, Vector2 screenTargetPos, Camera camera, Vector2 offset)
    {
        Vector2 locaPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTransform as RectTransform, screenTargetPos, camera, out locaPos);
        self.localPosition = locaPos + offset;
    }

    #endregion

    #region 对列表的拓展

    /// <summary>
    /// 拿到这列表中第一个具体特殊性，符合某个条件的元素。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="func">An element meets a condition</param>
    /// <returns></returns>
    public static T GetOneByList<T>(this List<T> self, System.Func<T, bool> func)
    {
        if (self == null) return default;
        int count = self.Count;
        for (int i = 0; i < count; i++)
        {
            if (func(self[i]))
            {
                return self[i];
            }
        }
        return default;
    }

    /// <summary>
    /// 拿到这列表中所有具有特殊性的元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="func">An element meets a condition</param>
    /// <returns></returns>
    public static List<T> GetAllByList<T>(this List<T> self, System.Func<T, bool> func)
    {
        List<T> ts = new List<T>();
        if (self == null) return ts;
        int count = self.Count;
        for (int i = 0; i < count; i++)
        {
            if (func(self[i]))
            {
                ts.Add(self[i]);
            }
        }
        return ts;
    }

    /// <summary>
    /// 在一个列表中找到一个元素，这个元素在这个列表中有特殊性。
    /// 比如，找到这列表中，最帅的一个元素，就可以用这个
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="func">An element meets a condition</param>
    /// <returns></returns>
    public static T GetOneByList<T>(this List<T> self, System.Func<T, T, bool> func)
    {
        if (self == null || self.Count <= 0) return default;
        T t = self[0];
        int count = self.Count;
        for (int i = 1; i < count; i++)
        {
            if (func(t, self[i]))
            {
                t = self[i];
            }
        }
        return t;
    }

    /// <summary>
    /// 随机打乱数组(很好用的)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    public static void RandomDisturbList<T>(this List<T> self)
    {
        List<T> oldList = new List<T>();

        int count = self.Count;
        for (int i = 0; i < count; i++)
        {
            oldList.Add(self[i]);
        }

        for (int i = 0; i < count; i++)
        {
            int r = UnityEngine.Random.Range(0, oldList.Count);
            self[i] = oldList[r];
            oldList.RemoveAt(r);
        }
    }

    /// <summary>
    /// 移除一个int列表中所有另一个列表中相同的元素
    /// 返回一个全新的列表
    /// </summary>
    /// <param name="sourceList">目标列表</param>
    /// <param name="removeList">需要移除的列表</param>
    /// <returns></returns>
    public static List<int> GetRandomAfterRemoval(this List<int> sourceList, List<int> removeList)
    {
        List<int> filteredList = sourceList.Except(removeList).ToList();
        return filteredList;
    }

    /// <summary>
    /// 拿到这个字符串列表中，这个特定字符串元素的索引
    /// 比如在{a,s,d,f,g,h} 传入 g 返回 4;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static int FindFirstIndexByValue(this List<string> self, string t)
    {
        int count = self.Count;
        for (int i = 0; i < count; i++)
        {
            if (self[i] == t)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// 打印出一个列表所有元素
    /// </summary>
    /// <param name="list"></param>
    public static void Printf<T>(this List<T> list)
    {
        if (list == null) return;
        int count = list.Count;
        string str = "";
        for (int i = 0; i < count; i++)
        {
            if (i == 0)
            {
                str = list[i].ToString();
            }
            else
            {
                str = str + "\t" + list[i].ToString();
            }
        }
        Debug.Log(str);
    }

    #endregion

    #region Other

    /// <summary>
    /// 从剪切板复制到内容
    /// </summary>
    /// <returns></returns>
    public static string GetClipboard()
    {
        return GUIUtility.systemCopyBuffer;
    }

    /// <summary>
    /// 将内容复制到剪切板
    /// </summary>
    /// <returns></returns>
    public static void SetClipboard(this string value)
    {
        GUIUtility.systemCopyBuffer = value;
    }

    /// <summary>
    /// 字符串转换颜色，无法转换默认是白色
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color GetColor(string color)
    {
        ColorUtility.TryParseHtmlString(color, out Color colorGold);
        return colorGold;
    }

    /// <summary>
    /// 霸道的设置UI是否可以被点击，有风险，慎用
    /// </summary>
    /// <param name="isCan"></param>
    public static void SetUIClick(bool isCan)
    {
        GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>().enabled = isCan;
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <returns></returns>
    public static T FromJson<T>(this string self)
    {
        return JsonConvert.DeserializeObject<T>(self);
    }
    /// <summary>
    /// 转化成JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self">对象</param>
    /// <param name="prettyPrint">是否格式化</param>
    /// <returns></returns>
    public static string ToJson<T>(this T self, bool prettyPrint = false)
    {
        return JsonConvert.SerializeObject(self, prettyPrint ? Formatting.Indented : Formatting.None);
    }

    /// <summary>
    /// 深度序列化复制一个对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <returns></returns>
    public static T CopySelf<T>(this T self)
    {
        string s = self.ToJson();
        return s.FromJson<T>();
    }

    #endregion

}
