using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 飞奖励2.0版本
    /// 极大程度的优化
    /// </summary>
    public class FlyReward
    {

        #region 需要具体配置

        /// <summary>
        /// 飞行音效
        /// </summary>
        private void PlayFlyAudio()
        {
            //AudioManager.Instance.PlaySound("FlyReward");
        }

        #endregion


        #region 池子
        private Dictionary<string, Reward> poolDic = new Dictionary<string, Reward>();
        /// <summary>
        /// 某一种需要飞的奖励
        /// </summary>
        public class Reward
        {
            /// <summary>
            /// 奖励类型名字
            /// </summary>
            public string name;
            /// <summary>
            /// 这个奖励的目标位置
            /// </summary>
            public Transform targetTr;
            /// <summary>
            /// 这个奖励的父物体
            /// </summary>
            public Transform parentTr;
            /// <summary>
            /// 这个奖励飞到目标后的回调
            /// </summary>
            public System.Action<int> addReward;
            public Reward(string name, Transform targetTr, Transform parentTr, System.Action<int> addReward)
            {
                this.name = name;
                this.targetTr = targetTr;
                this.parentTr = parentTr;
                this.addReward = addReward;
            }
        }
        #endregion
        #region 资源
        private GameObject rewardPrefab = null;
        private GameObject RewardPrefab
        {
            get
            {
                if (rewardPrefab == null)
                {
                    rewardPrefab = Resources.Load<GameObject>("Prefab/FlyReward/Reward");
                }
                return rewardPrefab;
            }
        }
        private SpriteAtlas rewardAtlas = null;
        public SpriteAtlas RewardAtlas
        {
            get
            {
                if (rewardAtlas == null)
                {
                    rewardAtlas = Resources.Load<SpriteAtlas>("Atlas/FlyReward");
                }
                return rewardAtlas;
            }
        }
        #endregion
        #region 单例化
        private static FlyReward instance = null;
        public static FlyReward Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FlyReward();
                }
                return instance;
            }
        }
        private FlyReward() { }
        #endregion


        /// <summary>
        /// 添加某个需要飞的奖励
        /// 注册要飞的奖励的意思
        /// 重复注册也没关系，不会重复。
        /// 移不移除其实无所谓。
        /// </summary>
        /// <param name="name">图片的名字</param>
        /// <param name="targerTr">目标点的Tr</param>
        /// <param name="parentTr">父物体Tr</param>
        /// <param name="flyFinishCallBack">飞到目标点之后的回调</param>
        public void AddReward(string name, Transform targerTr, Transform parentTr, System.Action<int> flyFinishCallBack)
        {
            Reward reward;
            if (poolDic.TryGetValue(name, out reward))
            {
                return;
            }
            reward = new Reward(name, targerTr, parentTr, flyFinishCallBack);
            poolDic.Add(name, reward);
        }

        /// <summary>
        /// 飞这个奖励
        /// </summary>
        /// <param name="name">奖励图片的名字</param>
        /// <param name="num">需要飞的数量</param>
        public void Fly(string name, int num = 1, Vector3 startPos = default, bool isShow = true, bool isAudio = true)
        {
            if (startPos == default)
            {
                startPos = Vector3.zero;
            }
            Reward reward;
            if (!poolDic.TryGetValue(name, out reward))
            {
                Debug.LogWarning("没有这个奖励类型");
                return;
            }

            Transform targetPos = reward.targetTr;
            Transform parentPos = reward.parentTr;
            int rewardNum = num;
            if (num > 9)
            {
                rewardNum = 9;
            }

            GameObject prefab = RewardPrefab;
            prefab.gameObject.SetActive(isShow);
            for (int i = 0; i < rewardNum; i++)
            {
                GameObject go = GameObject.Instantiate(prefab, new Vector3(startPos.x, startPos.y, parentPos.position.z), Quaternion.identity, parentPos);
                string icon = name;
                if (name == "Cash") icon = "Cash";
                go.GetComponent<Image>().sprite = RewardAtlas.GetSprite(icon);
                go.transform.localScale = Vector3.one;
                Sequence sequence = DOTween.Sequence();
                if (num > 1)
                {
                    sequence.Append(go.transform.DOMove(go.transform.position + new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0), Random.Range(0.1f, 0.8f)));
                }
                sequence.Append(go.transform.DOScale(Vector3.one, 0.3f));
                sequence.Append(go.transform.DOMove(targetPos.position, Random.Range(0.1f, 1.5f)));

                sequence.AppendCallback(() =>
                {
                    GameObject.Destroy(go);
                    int rn = num / (rewardNum);
                    reward.addReward(rn);
                });
                if (i == rewardNum - 1)
                {
                    if (isShow && isAudio) PlayFlyAudio();
                    sequence.AppendCallback(() =>
                    {
                        int rn = num / (rewardNum);
                        rn = (rewardNum) * rn;
                        reward.addReward(num - rn);
                    });
                }
            }
        }

    }
}