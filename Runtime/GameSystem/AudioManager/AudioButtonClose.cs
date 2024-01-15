using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 界面关闭按钮的点击音效
    /// </summary>
    public class AudioButtonClose : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioControl.Instance.PlaySound(AudioControl.ButtonClose);
            });
        }
    }
}