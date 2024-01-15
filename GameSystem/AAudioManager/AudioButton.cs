using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class AudioButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioControl.Instance.PlaySound(AudioControl.ButtonAudio);
                EventCentre.Instance.TriggerEvent(AudioControl.ButtonEvent, transform.name);
            });
        }
    }
}