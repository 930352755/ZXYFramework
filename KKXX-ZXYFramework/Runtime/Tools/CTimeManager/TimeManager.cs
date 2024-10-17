using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Better timer function
    /// Automatic generation, timing, timing, rotation training can be
    /// 2023 11 16
    /// Author:Ghost
    /// </summary>
    public class TimeManager : MonoBehaviour
    {

        #region Automatic mount singleton, automatic start
        private static TimeManager instance = null;
        public static TimeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject ins = new GameObject("TimeManager");
                    GameObject.DontDestroyOnLoad(ins);
                    instance = ins.AddComponent<TimeManager>();
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

        //-------------------------Event-------------------------
        private Dictionary<string, Coroutine> allTimerEvent;
        private void AAwake()
        {
            allTimerEvent = new Dictionary<string, Coroutine>();
        }

        /// <summary>
        /// Event counter
        /// </summary>
        private int count = 0;
        /// <summary>
        /// Add delay event
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time">The time will not be less than one render frame</param>
        /// <returns></returns>
        public string AddDelayedEvent(System.Action action, float time)
        {
            count += 1;
            string key = "Time" + count + TimeStamp.GetTimeStamp();
            Coroutine coroutine = StartCoroutine(DelayedCall(action, time, key));
            allTimerEvent.Add(key, coroutine);
            return key;
        }

        /// <summary>
        /// Add an interval trigger event
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public string AddIntervalEvent(System.Action action, float time)
        {
            count += 1;
            string key = "Interval" + count + TimeStamp.GetTimeStamp();
            Coroutine coroutine = StartCoroutine(IntervalCall(action, time));
            allTimerEvent.Add(key, coroutine);
            return key;
        }
        /// <summary>
        /// Stop a time event
        /// </summary>
        /// <param name="key"></param>
        public void StopTimerEvent(string key)
        {
            if (allTimerEvent.ContainsKey(key))
            {
                Coroutine coroutine = allTimerEvent[key];
                allTimerEvent.Remove(key);
                StopCoroutine(coroutine);
            }
        }

        private IEnumerator DelayedCall(System.Action action, float delayed, string key)
        {
            if (delayed < 0.02)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(delayed);
            }

            if (allTimerEvent.ContainsKey(key))
            {
                if (allTimerEvent.TryGetValue(key, out Coroutine coroutine))
                {
                    StopCoroutine(coroutine);
                }
                allTimerEvent.Remove(key);
                action?.Invoke();
            }
        }
        private IEnumerator IntervalCall(System.Action action, float delayed)
        {
            while (true)
            {
                yield return new WaitForSeconds(delayed);
                action?.Invoke();
            }
        }
        //------------------------------//------------------------

    }
}