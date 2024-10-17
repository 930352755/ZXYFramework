using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// UI面板和UI视图显示控件
    /// 作者：翼小鬼
    /// 2022.11.29
    /// </summary>
    public abstract class UIView : UIAnimation
    {
        /// <summary>
        /// 自己的UI层次结构
        /// </summary>
        [Tooltip("UI 层次结构")]
        public UILayers selfLayer = UILayers.Zero;

        /// <summary>
        /// 面板的状态
        /// </summary>
        [HideInInspector]
        public UIState selfState = UIState.Hide;

        /// <summary>
        /// 是否在此级别使用动画
        /// </summary>
        protected bool isUseAnima = true;

        #region 在管理器中使用的需要继承但不需要调用的参数

        /// <summary>
        /// 要显示的面板的队列
        /// </summary>
        [HideInInspector]
        public Stack<DelayUI> panelStack = new Stack<DelayUI>();

        /// <summary>
        /// 队列依次展示
        /// </summary>
        private void QueueShow()
        {
            UIManager.Instance.allCurOpenPanel[selfLayer] = null;
            if (panelStack.Count > 0)
            {
                DelayUI panel = panelStack.Pop();
                UIView uiView = UIManager.Instance.ShowUIView(panel.panelName, panel.panelParameter);
                for (int i = 0; i < panelStack.Count; i++)
                {
                    uiView.panelStack.Push(panelStack.Pop());
                }
            }
        }

        /// <summary>
        /// CanvasGroup用于整个UI
        /// </summary>
        private CanvasGroup cg = null;
        /// <summary>
        /// 用户界面点击掩码
        /// </summary>
        private CanvasGroup cgUI = null;

        [Tooltip("UI 刘海屏适配，弹板层级默认不做，其他默认作适配")]
        public UIPanelFit isPanelFitNotchScreen =  UIPanelFit.Default;

        public void OnStart()
        {

            InitialUIAnima();
            OnAStart();
            if (cg == null)
            {
                cg = gameObject.GetComponent<CanvasGroup>();
                if (cg == null)
                {
                    cg = gameObject.AddComponent<CanvasGroup>();
                }
            }
            cg.alpha = 0;
            cg.blocksRaycasts = false;
            selfState = UIState.Hide;

            
            PanelFitNotchScreen panelFitNotchScreen = GetComponent<PanelFitNotchScreen>();
            if (panelFitNotchScreen == null)
            {
                if(isPanelFitNotchScreen == UIPanelFit.Default)
                {
                    switch (selfLayer)
                    {
                        case UILayers.Pop1:
                        case UILayers.Pop2:
                        case UILayers.Pop3:
                        case UILayers.Pop4:
                        case UILayers.Pop5:
                        case UILayers.Pop6:
                        case UILayers.Pop7:
                        case UILayers.Pop8:
                            break;
                        default:
                            gameObject.AddComponent<PanelFitNotchScreen>();
                            break;
                    }
                }
                else if(isPanelFitNotchScreen == UIPanelFit.Fix)
                {
                    gameObject.AddComponent<PanelFitNotchScreen>();
                }
            }

        }

        public void Show(params object[] msg)
        {
            OnShowBefore(msg);
            OnShowStart();
        }
        public void Hide()
        {
            OnHideBefore();
            OnHideStart();
        }

        private void OnShowBefore(params object[] msg)
        {
            selfState = UIState.Showing;
            if (cg == null)
            {
                cg = gameObject.GetComponent<CanvasGroup>();
                if (cg == null)
                {
                    cg = gameObject.AddComponent<CanvasGroup>();
                }
            }
            cg.alpha = 1;
            cg.blocksRaycasts = true;


            if (cgUI == null)
            {
                cgUI = transform.GetChild(1).GetComponent<CanvasGroup>();
                if (cgUI == null)
                {
                    cgUI = transform.GetChild(1).gameObject.AddComponent<CanvasGroup>();
                }
            }
            cgUI.blocksRaycasts = false;

            OnBShowBefore(msg);
        }
        private void OnShowStart()
        {
            OpenAnima(selfLayer, OnShowLater, isUseAnima);
        }
        private void OnShowLater()
        {
            cgUI.blocksRaycasts = true;
            selfState = UIState.Show;
            OnBShowLater();
        }

        private void OnHideBefore()
        {
            selfState = UIState.Hideing;
            if (cg == null)
            {
                cg = gameObject.GetComponent<CanvasGroup>();
                if (cg == null)
                {
                    cg = gameObject.AddComponent<CanvasGroup>();
                }
            }
            cg.alpha = 1;

            if (cgUI == null)
            {
                cgUI = transform.GetChild(1).GetComponent<CanvasGroup>();
                if (cgUI == null)
                {
                    cgUI = transform.GetChild(1).gameObject.AddComponent<CanvasGroup>();
                }
            }
            cgUI.blocksRaycasts = false;
            OnCHideBefore();
        }
        private void OnHideStart()
        {
            CloseAnima(selfLayer, OnHideLater, isUseAnima);
        }
        private void OnHideLater()
        {
            selfState = UIState.Hide;
            cg.blocksRaycasts = false;
            cgUI.blocksRaycasts = true;
            cg.alpha = 0;
            OnCHideLater();
            QueueShow();
        }

        #endregion

        #region 生命周期

        /// <summary>
        /// 面板的初始化
        /// </summary>
        protected abstract void OnAStart();
        /// <summary>
        /// 面板的显示调用该方法
        /// 面板的显示前触发
        /// </summary>
        /// <param name="msg"></param>
        protected abstract void OnBShowBefore(params object[] msg);
        /// <summary>
        /// 面板的显示后触发
        /// </summary>
        protected virtual void OnBShowLater()
        {
            EventCentre.Instance.TriggerEvent("OpenUI", transform.name);
        }
        /// <summary>
        /// 面板关闭前触发
        /// </summary>
        protected virtual void OnCHideBefore()
        {
            EventCentre.Instance.TriggerEvent("CloseUI", transform.name);
        }
        /// <summary>
        /// 面板的关闭调用方法
        /// 面板关闭后触发
        /// </summary>
        protected abstract void OnCHideLater();

        #endregion

        /// <summary>
        /// 关闭自己
        /// </summary>
        protected virtual void CloseSelf()
        {
            if (selfState == UIState.Show)
            {
                UIManager.Instance.HideUIView(this);
            }
        }

        public static Transform FindChildTransformByName(Transform self, string childName)
        {
            Transform c = self.Find(childName);
            if (c != null) return c;
            for (int i = 0; i < self.childCount; i++)
            {
                c = FindChildTransformByName(self.GetChild(i), childName);
                if (c != null) return c;
            }
            return null;
        }

        public static T FindChindComponentByName<T>(Transform self, string childName) where T : class
        {
            Transform tr = FindChildTransformByName(self, childName);
            T t = tr.GetComponent<T>();
            if (t == null)
            {
                Debug.LogError(string.Format("未找到组件 {0} {1}", self.name, typeof(T)));
                return null;
            }
            return t;
        }
    }

    /// <summary>
    /// UI面板层次结构
    /// 最后一个是层数
    /// 零一二三.........储备层级以备不时之需。
    /// </summary>
    public enum UILayers
    {
        Zero,
        One,
        Two,
        Scenes,
        Three,
        TopResources,
        Four,
        Pop1,
        Pop2,
        Pop3,
        Pop4,
        Pop5,
        Pop6,
        Pop7,
        Pop8,
        Five,
        Six,
        Guide,
        Seven,
        Mask,
        Eight,
        UILayersCount
    }
    public class DelayUI
    {
        public string panelName;
        public object[] panelParameter;

        public DelayUI(string panelName, object[] panelParameter)
        {
            this.panelName = panelName;
            this.panelParameter = panelParameter;
        }
    }
    public enum UIState
    {
        Hide,
        Show,
        Hideing,
        Showing
    }
    public enum UIPanelFit
    {
        Default,
        Fix,
        Not,
    }

}