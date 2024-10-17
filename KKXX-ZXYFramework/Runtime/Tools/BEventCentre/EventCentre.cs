using System.Collections.Generic;

namespace Game
{

    /// <summary>
    /// 简单的事件消息中心
    /// Author:翼小鬼
    /// 2023/11/16
    /// </summary>
    public class EventCentre
    {

        #region 普通单列
        private static EventCentre instance;
        public static EventCentre Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventCentre();
                }
                return instance;
            }
        }
        private EventCentre()
        {
            eventDic = new Dictionary<string, ActionEvent>();
            eventDesDic = new Dictionary<string, string>();
        }
        #endregion

        #region 事件管理
        /// <summary>
        /// 通用万能参数委托
        /// </summary>
        /// <param name="args">万能参数</param>
        public delegate void ActionEvent(params object[] args);
        private Dictionary<string, ActionEvent> eventDic = null;
        private Dictionary<string, string> eventDesDic = null;
        #endregion

        #region 通用API

        /// <summary>
        /// 快捷注册无参数事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="des"></param>
        public void AddEvent(string name, System.Action action, string des)
        {
            AddEvent(name, (o) => { if (action != null) action(); }, des);
        }
        /// <summary>
        /// 添加一个通用参数消息
        /// </summary>
        /// <param name="name">Message name</param>
        /// <param name="action">Specific event</param>
        /// <param name="des">Event information</param>
        public void AddEvent(string name, ActionEvent action, string des)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name] += action;
            }
            else
            {
                eventDic.Add(name, action);
                eventDesDic.Add(name, des);
                Debug("添加事件：" + eventDesDic[name]);
            }
        }
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public void TriggerEvent(string name, params object[] args)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name](args);
                Debug("触发事件：" + eventDesDic[name]);
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogWarning("触发事件：" + name + ". 但是，此事件未注册，不会影响游戏");
#endif
            }
        }
        /// <summary>
        /// Remove message
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="action">Specific event (optional)</param>
        public void RemoveEvent(string name, ActionEvent action = null)
        {
            if (eventDic.ContainsKey(name))
            {
                if (action == null)
                {
                    eventDic.Remove(name);
                    eventDesDic.Remove(name);
                    Debug("Remove Event：" + eventDesDic[name]);
                }
                else
                {
                    eventDic[name] -= action;
                    Debug("Cancel an event：" + eventDesDic[name]);
                }
            }
        }

        #endregion

        #region 破坏破坏

        /// <summary>
        /// 清理所有事件
        /// </summary>
        public void Clear()
        {
            eventDic.Clear();
            eventDesDic.Clear();
        }

        ~EventCentre()
        {
            Clear();
        }

        #endregion

        #region 上帝的窥视

        private void Debug(string str)
        {

#if UNITY_EDITOR
            UnityEngine.Debug.Log(str);
#endif

        }

        public static void DebugAllEvent()
        {
#if UNITY_EDITOR
            foreach (var item in Instance.eventDesDic)
            {
                UnityEngine.Debug.LogError(item.Key + ":\t\t\t\t" + item.Value);
            }
#endif
        }

        #endregion

    }

    /// <summary>
    /// 对象的拓展转换
    /// </summary>
    public static class ObjectTloos
    {
        /// <summary>
        /// object类型 --> int类型
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int ASInt(this object self)
        {
            return (int)self;
        }
        /// <summary>
        /// object类型 --> bool类型
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool ASBool(this object self)
        {
            return (bool)self;
        }
        /// <summary>
        /// object类型 --> float类型
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static float ASFloat(this object self)
        {
            return (float)self;
        }
        /// <summary>
        /// object类型 --> string类型
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ASString(this object self)
        {
            return (string)self;
        }
        /// <summary>
        /// object类型 --> T类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T AS<T>(this object self)
        {
            return (T)self;
        }
    }

}