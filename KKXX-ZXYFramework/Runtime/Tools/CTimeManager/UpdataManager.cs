using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Unity MonoBehaviour All event management。
    /// </summary>
    public class UpdataManager : MonoBehaviour
    {
        #region Automatic mount singleton, automatic start
        private static UpdataManager instance = null;
        public static UpdataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject ins = new GameObject("UpdataManager");
                    GameObject.DontDestroyOnLoad(ins);
                    instance = ins.AddComponent<UpdataManager>();
                    instance.AAwake();
                }
                return instance;
            }
        }
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            Instance.StartSystem();
        }
        private void StartSystem() { }
        #endregion

        private void AAwake()
        {
            allUpdataEvent = new Dictionary<string, System.Action>();
            allFixedUpdataEvent = new Dictionary<string, System.Action>();
            allOnApplicationQuitEvent = new Dictionary<string, System.Action>();
            allOnApplicationPauseEvent = new Dictionary<string, System.Action<bool>>();
        }

        #region Render frame event
        private Dictionary<string, System.Action> allUpdataEvent;
        /// <summary>
        /// Event counter
        /// </summary>
        private int updataEventCount = 0;
        /// <summary>
        /// Adds a render frame event
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddUptataEvent(System.Action action)
        {
            updataEventCount++;
            string key = "Uptata" + updataEventCount + TimeStamp.GetTimeStamp();
            return AddUptataEvent(key, action);
        }
        /// <summary>
        /// Adds a render frame event
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddUptataEvent(string key, System.Action action)
        {
            if (allUpdataEvent.ContainsKey(key))
            {
                return key;
            }
            allUpdataEvent.Add(key, action);
            return key;
        }
        /// <summary>
        /// Removes a render frame event
        /// </summary>
        /// <param name="key">Corresponding Key</param>
        public void RomoveUptataEvent(string key)
        {
            if (allUpdataEvent.ContainsKey(key))
            {
                allUpdataEvent.Remove(key);
            }
        }
        private void Update()
        {
            foreach (System.Action action in allUpdataEvent.Values)
            {
                action?.Invoke();
            }
        }
        #endregion

        #region Physical character event
        private Dictionary<string, System.Action> allFixedUpdataEvent;
        /// <summary>
        /// Event counter
        /// </summary>
        private int fixedUpdataCount = 0;
        /// <summary>
        /// Adds a physical frame event
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddFixedUpdataEvent(System.Action action)
        {
            fixedUpdataCount += 1;
            string key = "FixedUpdata" + fixedUpdataCount + TimeStamp.GetTimeStamp();
            return AddFixedUpdataEvent(key, action);
        }
        /// <summary>
        /// Adds a physical frame event
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddFixedUpdataEvent(string key, System.Action action)
        {
            if (allFixedUpdataEvent.ContainsKey(key))
            {
                return key;
            }
            allFixedUpdataEvent.Add(key, action);
            return key;
        }
        /// <summary>
        /// Removes a physical frame event
        /// </summary>
        /// <param name="key">Corresponding Key</param>
        public void RomoveFixedUpdataEvent(string key)
        {
            if (allFixedUpdataEvent.ContainsKey(key))
            {
                allFixedUpdataEvent.Remove(key);
            }
        }
        private void FixedUpdate()
        {
            foreach (System.Action action in allFixedUpdataEvent.Values)
            {
                action?.Invoke();
            }
        }
        #endregion

        #region Game exit event
        private Dictionary<string, System.Action> allOnApplicationQuitEvent;
        /// <summary>
        /// Event counter
        /// </summary>
        private int onApplicationQuitCount = 0;
        /// <summary>
        /// Add a game exit event
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddOnApplicationQuitEvent(System.Action action)
        {
            onApplicationQuitCount += 1;
            string key = "OnApplicationQuit" + onApplicationQuitCount + TimeStamp.GetTimeStamp();
            return AddOnApplicationQuitEvent(key, action);
        }
        /// <summary>
        /// Add a game exit event
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddOnApplicationQuitEvent(string key, System.Action action)
        {
            if (allOnApplicationQuitEvent.ContainsKey(key))
            {
                return key;
            }
            allOnApplicationQuitEvent.Add(key, action);
            return key;
        }
        /// <summary>
        /// Removes a game exit event
        /// </summary>
        /// <param name="key">Corresponding Key</param>
        public void RomoveOnApplicationQuitEvent(string key)
        {
            if (allOnApplicationQuitEvent.ContainsKey(key))
            {
                allOnApplicationQuitEvent.Remove(key);
            }
        }
        private void OnApplicationQuit()
        {
            foreach (System.Action action in allOnApplicationQuitEvent.Values)
            {
                action?.Invoke();
            }
        }
        #endregion

        #region The game pauses to continue the event

        private Dictionary<string, System.Action<bool>> allOnApplicationPauseEvent;
        /// <summary>
        /// Event counter
        /// </summary>
        private int onApplicationPauseCount = 0;
        /// <summary>
        /// Adds a game pause to continue event
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddOnApplicationPauseEvent(System.Action<bool> action)
        {
            onApplicationPauseCount += 1;
            string key = "OnApplicationPause" + onApplicationPauseCount + TimeStamp.GetTimeStamp();
            return AddOnApplicationPauseEvent(key, action);
        }
        /// <summary>
        /// Adds a game pause to continue event
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddOnApplicationPauseEvent(string key, System.Action<bool> action)
        {
            if (allOnApplicationPauseEvent.ContainsKey(key))
            {
                return key;
            }
            allOnApplicationPauseEvent.Add(key, action);
            return key;
        }
        /// <summary>
        /// Removes a game pause Continue event
        /// </summary>
        /// <param name="key">Corresponding Key</param>
        public void RomoveOnApplicationPauseEvent(string key)
        {
            if (allOnApplicationPauseEvent.ContainsKey(key))
            {
                allOnApplicationPauseEvent.Remove(key);
            }
        }
        private void OnApplicationPause(bool pause)
        {
            foreach (System.Action<bool> action in allOnApplicationPauseEvent.Values)
            {
                action?.Invoke(pause);
            }
        }

        #endregion

        #region Expand the save operation for game exit
        /// <summary>
        /// Usually used for game exit, save data
        /// </summary>
        /// <param name="save"></param>
        public void AddSaveEvent(System.Action save)
        {
            AddOnApplicationQuitEvent(save);
            AddOnApplicationPauseEvent((b) => { if (b) save?.Invoke(); });
        }
        #endregion

    }
}
