using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using YooAsset;
using System.Threading.Tasks;

namespace Game
{

    /// <summary>
    /// UI管理器 
    /// 资源没加载好，不能结束Loading界面
    /// 作者：翼小鬼
    /// 2022.11.08
    /// </summary>
    public class UIManager
    {

        #region 资源路径，初始化信息

        /// <summary>
        /// 资源路径JSON位置
        /// </summary>
        public static string UIJsonPathDir = YooResManager.YooAssetResPath + "/Config/JsonData";
        /// <summary>
        /// 存放 UI所有文件的路径 的文件路径
        /// </summary>
        public static string UIJsonPath = UIJsonPathDir + "/GameUIPathInfo";


        /// <summary>
        /// UI系统是否已经初始化完成了
        /// </summary>
        private static bool _isLoadOver = false;
        public static bool IsLoadOver => _isLoadOver;

        #region 起点，设置画布

        private Dictionary<UILayers, Transform> allUILayers;

        /// <summary>
        /// 切换场景后，您需要将新场景的Canvas设置为UI的父对象
        /// </summary>
        /// <param name="uiCanvas"></param>
        public void SetCanvas(Transform uiCanvas)
        {
            uiViewDic.Clear();
            allUILayers.Clear();
            allCurOpenPanel.Clear();
            for (int i = 0; i < (int)UILayers.UILayersCount; i++)
            {
                RectTransform uiRoot = new GameObject(((UILayers)i).ToString(), typeof(RectTransform)).transform as RectTransform;
                uiRoot.SetParent(uiCanvas);
                uiRoot.localScale = Vector3.one;
                uiRoot.anchorMin = Vector2.zero;
                uiRoot.anchorMax = Vector2.one;
                uiRoot.sizeDelta = Vector2.zero;
                uiRoot.localPosition = Vector3.zero;
                Canvas ca = uiRoot.gameObject.AddComponent<Canvas>();
                ca.overrideSorting = true;
                ca.sortingOrder = 1 + 3 * i;
                uiRoot.gameObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
                allUILayers.Add((UILayers)i, uiRoot);
                allCurOpenPanel.Add((UILayers)i, null);
            }
            LoadResources();
        }

        #endregion

        #region 初始化加载

        /// <summary>
        /// 所有的预表都已加载
        /// </summary>
        private GameObject[] allUIPanel;

        /// <summary>
        /// 加载操作是异步执行的
        /// </summary>
        /// <returns></returns>
        private async void LoadResources()
        {
            Log("开始加载UI");
            _isLoadOver = false;
            TextAsset jsonUI = await YooResManager.Instance.LoadAssetAsync<TextAsset>(UIJsonPath);
            AllUIPathInfo allUIPathInfo = jsonUI.text.FromJson<AllUIPathInfo>();
            int count = allUIPathInfo.allPath.Count;
            List<GameObject> gs = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                string path = allUIPathInfo.allPath[i];
                GameObject goUI = await YooResManager.Instance.LoadAssetAsync<GameObject>(path);
                gs.Add(goUI);
            }
            allUIPanel = gs.ToArray();
            int length = allUIPanel.Length;
            for (int i = 0; i < length; i++)
            {
                GameObject prefab = allUIPanel[i];
                GameObject UIPrefab = GameObject.Instantiate(prefab);
                Transform uiType;
                UIView uiView = UIPrefab.GetComponent<UIView>();
                if (uiView == null)
                {
                    Debug.LogError("预制件不是UIView所以加载失败 +" + prefab.name);
                    continue;
                }
                UILayers uiLayer = uiView.selfLayer;
                allUILayers.TryGetValue(uiLayer, out uiType);
                if (uiType == null)
                {
                    Debug.LogError("没有Canvas 画布，这些应该都生成在画布层级之下。");
                    return;
                }
                UIPrefab.transform.SetParent(uiType);

                UIPrefab.name = prefab.name;
                UIPrefab.transform.localScale = Vector3.one;
                RectTransform rt = UIPrefab.transform as RectTransform;
                rt.localPosition = Vector3.zero;
                rt.anchoredPosition = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.offsetMin = Vector2.zero;
                uiViewDic.Add(prefab.name, uiView);
                uiView.OnStart();
                await Task.Yield();
            }
            _isLoadOver = true;
            Log("UI初始化完成");
        }

        #endregion

        #endregion

        #region 普通单利

        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }
        private UIManager()
        {
            _isLoadOver = false;
            allUILayers = new Dictionary<UILayers, Transform>();
            allCurOpenPanel = new Dictionary<UILayers, UIView>();
            uiViewDic = new Dictionary<string, UIView>();
        }

        #endregion

        #region 初始化逻辑 内部处理

        #region 池子

        private Dictionary<string, UIView> uiViewDic = new Dictionary<string, UIView>();

        #endregion

        #region 展示UI面板

        /// <summary>
        /// 禁止被外界调用！！！！！！！！！！！！！！！
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hideCallBack"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public UIView ShowUIView(string name, params object[] msg)
        {
            UIView uiView;
            if (!uiViewDic.TryGetValue(name, out uiView))
            {
                Debug.LogError("加载失败" + name);
                return null;
            }
            UIView curPanel = allCurOpenPanel[uiView.selfLayer];
            if (curPanel != null)
            {
                DelayUI delayUI = new DelayUI(name, msg);
                curPanel.panelStack.Push(delayUI);
                return null;
            }
            allCurOpenPanel[uiView.selfLayer] = uiView;
            uiView.Show(msg);
            return uiView;
        }

        #endregion

        #region hide

        /// <summary>
        /// 禁止被外界调用！！！！！！！！！！！！！！！
        /// </summary>
        /// <param name="uiView"></param>
        public void HideUIView(UIView uiView)
        {
            uiView.Hide();
        }

        #endregion

        #region Find

        private UIView FindView(string name)
        {
            UIView uiView;
            if (!uiViewDic.TryGetValue(name, out uiView))
            {
                return null;
            }
            return uiView;
        }

        #endregion

        #region Permanently destroy a panel

        private void DestroyUIView(string name)
        {
            UIView uiView;
            if (!uiViewDic.TryGetValue(name, out uiView))
            {
                Debug.LogError("面板没有" + name);
                return;
            }
            uiViewDic.Remove(name);
            GameObject.Destroy(uiView.gameObject);
        }

        #endregion

        #endregion

        #region 对外部使用的一些方法和判断

        #region 一UI级别的面板只能存在一个

        public Dictionary<UILayers, UIView> allCurOpenPanel;

        /// <summary>
        /// 获取到当时某个层级展示的唯一 UIView 面板
        /// 如果是空，就是这个层级没有面板展示
        /// </summary>
        /// <param name="layers">层级</param>
        /// <returns></returns>
        public UIView GetCurUIViewLayersIn(UILayers layers)
        {
            if (allCurOpenPanel.ContainsKey(layers))
            {
                return allCurOpenPanel[layers];
            }
            return null;
        }

        #endregion

        #region 展示

        /// <summary>
        /// 显示面板
        /// </summary>
        public void ShowUI(string name, params object[] msg)
        {
            ShowUIView(name, msg);
        }

        /// <summary>
        /// 显示面板
        /// </summary>
        public void ShowUI<T>(params object[] msg)
        {
            string name = typeof(T).Name;
            ShowUIView(name.ToString(), msg);
        }

        #endregion

        #region 隐藏
        /// <summary>
        /// 隐藏面版
        /// </summary>
        public void HideUI(string name)
        {
            UIView uiView = null;
            if (!uiViewDic.TryGetValue(name, out uiView))
            {
                Debug.LogError("没有面板" + name);
                return;
            }
            HideUIView(uiView);
        }
        /// <summary>
        /// 隐藏面版
        /// </summary>
        public void HideUI<T>()
        {
            string name = typeof(T).Name;
            HideUI(name.ToString());
        }
        #endregion

        #region 查找

        /// <summary>
        /// 查找某个面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FindUI<T>() where T : UIView
        {
            string name = typeof(T).Name;
            UIView uIView = FindView(name);
            if (uIView == null) return null;
            return uIView as T;
        }

        #endregion

        #region 毁灭
        /// <summary>
        /// 彻底销毁某个面板
        /// </summary>
        /// <param name="name"></param>
        public void DestroyUI(string name)
        {
            DestroyUIView(name);
        }

        #endregion

        #endregion

        #region 日志信息
        /// <summary>
        /// 日志封装
        /// </summary>
        /// <param name="msg"></param>
        private void Log(object msg)
        {
#if DEBUG
            Debug.Log("<Color=#E60000>" + msg + "</Color>");
#endif
        }
        #endregion

    }

    /// <summary>
    /// 记录所有的面板路径
    /// </summary>
    [System.Serializable]
    public class AllUIPathInfo
    {
        public List<string> allPath;
    }

}