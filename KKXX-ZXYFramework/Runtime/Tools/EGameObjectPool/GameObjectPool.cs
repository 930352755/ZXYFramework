using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    /// <summary>
    /// 游戏物体对象池 
    /// 作者：翼小鬼
    /// 2020.11.01
    /// </summary>
    public class GameObjectPool : MonoBehaviour
    {

        #region 单例自动放到游戏里，自动启动
        private static GameObjectPool instance = null;
        public static GameObjectPool Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject ins = new GameObject("GameObjectPool");
                    GameObject.DontDestroyOnLoad(ins);
                    instance = ins.AddComponent<GameObjectPool>();
                    instance.cache = new Dictionary<string, List<GameObject>>();
#if DEBUG
                    Debug.Log("<Color=#E60000>对象池系统初始化完成！</Color>");
#endif
                }
                return instance;
            }
        }
        /// <summary>
        /// 每次场景加载（无论是首次加载还是重新加载）时，该方法都会执行。
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            Instance.AEmpty();
        }
        private void AEmpty(){}
        #endregion

        #region 使用方法
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="key">对象的唯一KEY，不可重复</param>
        /// <param name="prefab">对象预制体模版</param>
        /// <param name="parent">父物体</param>
        /// <param name="pos">初始位置</param>
        /// <param name="dir">初始方向</param>
        /// <returns>生成的对象</returns>
        public GameObject CreateObejct(string key, GameObject prefab, Transform parent = null, Vector3 pos = default, Quaternion dir = default)
        {
            if (string.IsNullOrEmpty(key)) key = prefab.name;
            GameObject go = null;
            if (cache.ContainsKey(key))
            {
                if (cache[key].Count > 0)
                {
                    go = cache[key][0];
                    cache[key].RemoveAt(0);
                }
            }
            if (go == null)
            {
                go = GameObject.Instantiate(prefab);
                go.name = key;
            }

            go.transform.position = pos;
            go.transform.rotation = dir;
            go.SetActive(true);

            if (parent == null) parent = prefab.transform.parent;
            go.transform.SetParent(parent);
            foreach (IResetable item in go.GetComponents<IResetable>())
            {
                item.OnReset();
            }
            return go;
        }
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="key">对象的唯一KEY，不可重复</param>
        /// <param name="go">对象预制体</param>
        /// <param name="delay">延时时间</param>
        public void CollectObejct(string key, GameObject go, float delay = 0)
        {
            if (delay <= 0)
                CollectObject(key, go);
            else
                StartCoroutine(DelayCollectObject(key, go, delay));
        }
        /// <summary>
        /// 清理对象池
        /// </summary>
        public void ClearPool()
        {
            foreach (List<GameObject> value in cache.Values)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Destroy(value[i]);
                }
            }
            cache.Clear();
        }
        #endregion

        #region 内部实现
        private Dictionary<string, List<GameObject>> cache;
        private void CollectObject(string key, GameObject go)
        {
            if (string.IsNullOrEmpty(key)) key = go.name;
            if (!cache.ContainsKey(key)) cache.Add(key, new List<GameObject>());
            cache[key].Add(go);
            go.SetActive(false);
            go.transform.SetParent(transform);
        }
        private IEnumerator DelayCollectObject(string key, GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            CollectObject(key, go);
        }
        #endregion

    }

    /// <summary>
    /// 重置接口
    /// 需要信息重置的一些对象可以继承这个接口，实现重置。
    /// </summary>
    public interface IResetable
    {
       public void OnReset();
    }

}