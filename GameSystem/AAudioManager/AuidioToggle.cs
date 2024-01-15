using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 对Toggle挂载，点击音效，与数据打点采集。
    /// </summary>
    public class AuidioToggle : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Toggle>().onValueChanged.AddListener((b) =>
            {
                if (b)
                {
                    AudioControl.Instance.PlaySound(AudioControl.ButtonAudio);
                    EventCentre.Instance.TriggerEvent(AudioControl.ButtonEvent, transform.name);
                }
            });
        }
    }
}