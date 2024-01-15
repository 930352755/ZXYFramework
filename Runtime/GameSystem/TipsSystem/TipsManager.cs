using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Game
{
    /// <summary>
    /// 简易的提示系统
    /// </summary>
    public class TipsManager
    {
        private static TipsManager instance;
        public static TipsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TipsManager();
                }
                return instance;
            }
        }

        private GameObject tipsManager;
        private GameObject mask;
        private Text tipText;
        private CanvasGroup cgTips;
        private TipsManager()
        {
            GameObject go = Resources.Load<GameObject>("Prefabs/Effects/TipsCanvas");
            tipsManager = GameObject.Instantiate(go);
            GameObject.DontDestroyOnLoad(tipsManager);
            tipsManager.name = "TipsManager";

            mask = tipsManager.transform.GetChild(0).gameObject;
            tipText = mask.transform.GetChild(0).GetComponent<Text>();
            cgTips = mask.GetComponent<CanvasGroup>();
            cgTips.alpha = 0;

            mask.gameObject.SetActive(false);
        }

        private List<Transform> allTips = new List<Transform>();

        private Transform GetATips()
        {
            return GameObject.Instantiate(mask, tipsManager.transform).transform;
        }

        public void ShowTips(string content, float durationTime = 2f)
        {
            Transform tr = GetATips();
            allTips.Add(tr);
            tr.gameObject.SetActive(true);
            tr.transform.localPosition = Vector3.zero;
            tr.GetComponent<CanvasGroup>().alpha = 0;

            tr.DOKill();
            tr.GetChild(0).GetComponent<Text>().text = content;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(tr.GetComponent<CanvasGroup>().DOFade(1, 0.3f));
            for (int i = 0; i < allTips.Count - 1; i++)
            {
                allTips[i].DOLocalMoveY(((((mask.transform as RectTransform).rect.height) + 7)) * (allTips.Count - 1 - i), 0.3f);
            }

            DOVirtual.DelayedCall(durationTime, () =>
            {
                Hide(tr);
            });
        }
        private void Hide(Transform tr)
        {
            tr.GetComponent<CanvasGroup>().DOFade(0, 0.5f).onComplete += () =>
            {
                allTips.Remove(tr);
                GameObject.Destroy(tr.gameObject);
            };
        }
    }
}
