using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 工具类
/// </summary>
public static class ToolsClass
{

    #region UI UnityUGUI相关拓展

    #region RectTransform

    #region AnchoredPosition
    /// <summary>
    /// Set anchoredPosition via a vector coordinate
    /// </summary>
    /// <param name="self"></param>
    /// <param name="anchoredPosition">Coordinate position</param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition(this RectTransform self, Vector3 anchoredPosition)
    {
        self.anchoredPosition = anchoredPosition;
        return self;
    }
    /// <summary>
    /// Set anchoredPosition via an X coordinate and a Y coordinate
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition(this RectTransform self, float x, float y)
    {
        Vector2 anchoredPosition = self.anchoredPosition;
        anchoredPosition.x = x;
        anchoredPosition.y = y;
        self.anchoredPosition = anchoredPosition;
        return self;
    }
    /// <summary>
    /// Set anchoredPosition X coordinate value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform AnchoredPositionX(this RectTransform self, float x)
    {
        Vector2 anchoredPosition = self.anchoredPosition;
        anchoredPosition.x = x;
        self.anchoredPosition = anchoredPosition;
        return self;
    }
    /// <summary>
    /// Set anchoredPosition Y coordinate value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y">Y</param>
    /// <returns></returns>
    public static RectTransform AnchoredPositionY(this RectTransform self, float y)
    {
        Vector2 anchoredPosition = self.anchoredPosition;
        anchoredPosition.y = y;
        self.anchoredPosition = anchoredPosition;
        return self;
    }
    #endregion
    #region OffsetMax
    /// <summary>
    /// Set the offset max.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="offsetMax"></param>
    /// <returns></returns>
    public static RectTransform OffsetMax(this RectTransform self, Vector2 offsetMax)
    {
        self.offsetMax = offsetMax;
        return self;
    }
    /// <summary>
    /// Set the offset max.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform OffsetMax(this RectTransform self, float x, float y)
    {
        Vector2 offsetMax = self.offsetMax;
        offsetMax.x = x;
        offsetMax.y = y;
        self.offsetMax = offsetMax;
        return self;
    }
    /// <summary>
    /// Set the offset max x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform OffsetMaxX(this RectTransform self, float x)
    {
        Vector2 offsetMax = self.offsetMax;
        offsetMax.x = x;
        self.offsetMax = offsetMax;
        return self;
    }
    /// <summary>
    /// Set the offset max y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform OffsetMaxY(this RectTransform self, float y)
    {
        Vector2 offsetMax = self.offsetMax;
        offsetMax.y = y;
        self.offsetMax = offsetMax;
        return self;
    }
    #endregion
    #region OffsetMin
    /// <summary>
    /// Set the offset min.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="offsetMin"></param>
    /// <returns></returns>
    public static RectTransform OffsetMin(this RectTransform self, Vector2 offsetMin)
    {
        self.offsetMin = offsetMin;
        return self;
    }
    /// <summary>
    /// Set the offset min.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform OffsetMin(this RectTransform self, float x, float y)
    {
        Vector2 offsetMin = self.offsetMin;
        offsetMin.x = x;
        offsetMin.y = y;
        self.offsetMin = offsetMin;
        return self;
    }
    /// <summary>
    /// Set the offset min x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform OffsetMinX(this RectTransform self, float x)
    {
        Vector2 offsetMin = self.offsetMin;
        offsetMin.x = x;
        self.offsetMin = offsetMin;
        return self;
    }
    /// <summary>
    /// Set the offset min y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform OffsetMinY(this RectTransform self, float y)
    {
        Vector2 offsetMin = self.offsetMin;
        offsetMin.y = y;
        self.offsetMin = offsetMin;
        return self;
    }
    #endregion
    #region AnchoredPosition3D
    /// <summary>
    /// Set the anchored position 3d.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="anchoredPosition3D"></param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition3D(this RectTransform self, Vector2 anchoredPosition3D)
    {
        self.anchoredPosition3D = anchoredPosition3D;
        return self;
    }
    /// <summary>
    /// Set the anchored position 3d.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition3D(this RectTransform self, float x, float y)
    {
        Vector2 anchoredPosition3D = self.anchoredPosition3D;
        anchoredPosition3D.x = x;
        anchoredPosition3D.y = y;
        self.anchoredPosition3D = anchoredPosition3D;
        return self;
    }
    /// <summary>
    /// Set the anchored position 3d x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition3DX(this RectTransform self, float x)
    {
        Vector2 anchoredPosition3D = self.anchoredPosition3D;
        anchoredPosition3D.x = x;
        self.anchoredPosition3D = anchoredPosition3D;
        return self;
    }
    /// <summary>
    /// Set the anchored position 3d y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchoredPosition3DY(this RectTransform self, float y)
    {
        Vector2 anchoredPosition3D = self.anchoredPosition3D;
        anchoredPosition3D.y = y;
        self.anchoredPosition3D = anchoredPosition3D;
        return self;
    }
    #endregion
    #region AnchorMin
    /// <summary>
    /// Set the anchor min.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="anchorMin"></param>
    /// <returns></returns>
    public static RectTransform AnchorMin(this RectTransform self, Vector2 anchorMin)
    {
        self.anchorMin = anchorMin;
        return self;
    }
    /// <summary>
    /// Set the anchor min.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchorMin(this RectTransform self, float x, float y)
    {
        Vector2 anchorMin = self.anchorMin;
        anchorMin.x = x;
        anchorMin.y = y;
        self.anchorMin = anchorMin;
        return self;
    }
    /// <summary>
    /// Set the anchor min x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform AnchorMinX(this RectTransform self, float x)
    {
        Vector2 anchorMin = self.anchorMin;
        anchorMin.x = x;
        self.anchorMin = anchorMin;
        return self;
    }
    /// <summary>
    /// Set the anchor min y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchorMinY(this RectTransform self, float y)
    {
        Vector2 anchorMin = self.anchorMin;
        anchorMin.y = y;
        self.anchorMin = anchorMin;
        return self;
    }
    #endregion
    #region AnchorMax
    /// <summary>
    /// Set the anchor max.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="anchorMax"></param>
    /// <returns></returns>
    public static RectTransform AnchorMax(this RectTransform self, Vector2 anchorMax)
    {
        self.anchorMax = anchorMax;
        return self;
    }
    /// <summary>
    /// Set the anchor max.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchorMax(this RectTransform self, float x, float y)
    {
        Vector2 anchorMax = self.anchorMax;
        anchorMax.x = x;
        anchorMax.y = y;
        self.anchorMax = anchorMax;
        return self;
    }
    /// <summary>
    /// Set the anchor max x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform AnchorMaxX(this RectTransform self, float x)
    {
        Vector2 anchorMax = self.anchorMax;
        anchorMax.x = x;
        self.anchorMax = anchorMax;
        return self;
    }
    /// <summary>
    /// Set the anchor max y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform AnchorMaxY(this RectTransform self, float y)
    {
        Vector2 anchorMax = self.anchorMax;
        anchorMax.y = y;
        self.anchorMax = anchorMax;
        return self;
    }
    #endregion
    #region Pivot
    /// <summary>
    /// Set the pivot.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="pivot"></param>
    /// <returns></returns>
    public static RectTransform Pivot(this RectTransform self, Vector2 pivot)
    {
        self.pivot = pivot;
        return self;
    }
    /// <summary>
    /// Set the pivot.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform Pivot(this RectTransform self, float x, float y)
    {
        Vector2 pivot = self.pivot;
        pivot.x = x;
        pivot.y = y;
        self.pivot = pivot;
        return self;
    }
    /// <summary>
    /// Set the pivot x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform PivotX(this RectTransform self, float x)
    {
        Vector2 pivot = self.pivot;
        pivot.x = x;
        self.pivot = pivot;
        return self;
    }
    /// <summary>
    /// Set the pivot y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform PivotY(this RectTransform self, float y)
    {
        Vector2 pivot = self.pivot;
        pivot.y = y;
        self.pivot = pivot;
        return self;
    }
    #endregion
    #region SizeDelta
    /// <summary>
    /// Set the size delta.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="sizeDelta"></param>
    /// <returns></returns>
    public static RectTransform SizeDelta(this RectTransform self, Vector2 sizeDelta)
    {
        self.sizeDelta = sizeDelta;
        return self;
    }
    /// <summary>
    /// Set the size delta.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform SizeDelta(this RectTransform self, float x, float y)
    {
        Vector2 sizeDelta = self.sizeDelta;
        sizeDelta.x = x;
        sizeDelta.y = y;
        self.sizeDelta = sizeDelta;
        return self;
    }
    /// <summary>
    /// Set the size delta x value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static RectTransform SizeDeltaX(this RectTransform self, float x)
    {
        Vector2 sizeDelta = self.sizeDelta;
        sizeDelta.x = x;
        self.sizeDelta = sizeDelta;
        return self;
    }
    /// <summary>
    /// Set the size delta y value.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static RectTransform SizeDeltaY(this RectTransform self, float y)
    {
        Vector2 sizeDelta = self.sizeDelta;
        sizeDelta.y = y;
        self.sizeDelta = sizeDelta;
        return self;
    }
    #endregion
    #region Size
    /// <summary>
    /// Set width with current anchors.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static RectTransform SetSizeWidth(this RectTransform self, float width)
    {
        self.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        return self;
    }
    /// <summary>
    /// Set height with current anchors.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static RectTransform SetSizeHeight(this RectTransform self, float height)
    {
        self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        return self;
    }

    #endregion
    #endregion

    /// <summary>
    /// 按钮添加点击事件
    /// </summary>
    /// <param name="self"></param>
    /// <param name="action">点击触发事件</param>
    public static void AddClick(this Button self, System.Action action)
    {
        self.onClick.AddListener(() => { action(); });
    }

    /// <summary>
    /// 根据一个屏幕坐标点设置UI位置
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
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="func">An element meets a condition</param>
    /// <returns></returns>
    public static T GetOneByList<T>(this List<T> self, System.Func<T, T, bool> func)
    {
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
    /// 随机打乱数组
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

    ///// <summary>
    ///// 拿到这列表中这个列表元素的第一个索引
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="self"></param>
    ///// <param name="t"></param>
    ///// <returns></returns>
    //public static int FindFirstIndexByValue(this List<string> self, string t)
    //{
    //    int count = self.Count;
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (self[i] == t)
    //        {
    //            return i;
    //        }
    //    }
    //    return -1;
    //}

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
    public static Color GetColor(this string color)
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
    /// 键值对类型，无法被序列化成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <returns></returns>
    public static T FromJson<T>(this string self)
    {
        return JsonUtility.FromJson<T>(self);
    }

    /// <summary>
    /// 转化成JSON
    /// 键值对类型无法使用这个进行序列话
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self">对象</param>
    /// <param name="prettyPrint">是否格式化</param>
    /// <returns></returns>
    public static string ToJson<T>(this T self, bool prettyPrint = false)
    {
        return JsonUtility.ToJson(self, prettyPrint);
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
