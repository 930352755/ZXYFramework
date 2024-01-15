using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 按钮挂载，点击音效，数据打点采集，与点击动画
    /// </summary>
    public class AudioButtonAndAnima : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioControl.Instance.PlaySound(AudioControl.ButtonAudio);
                EventCentre.Instance.TriggerEvent(AudioControl.ButtonEvent, transform.name);
            });
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(0.97f, 0.1f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(1, 0.1f);
        }
    }
}