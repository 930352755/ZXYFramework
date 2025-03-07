using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AudioControl
    {

        private static AudioControl instance = null;
        public static AudioControl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioControl();
                }
                return instance;
            }
        }
        private AudioControl() { }

        #region 快速数据处理

        /// <summary>
        /// 按钮点击音效声音的名称
        /// </summary>
        public readonly static string ButtonAudio = "ButtonClick";
        /// <summary>
        /// 关闭按钮点击音效声音的名称
        /// </summary>
        public readonly static string ButtonClose = "ButtonClose";


        /// <summary>
        /// 音效资源路径
        /// </summary>
        public const string AudioSoundPath = "Audios/Sounds/";
        /// <summary>
        /// BGM资源路径
        /// </summary>
        public const string AudioMusicPath = "Audios/BGMS/";

        #endregion

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="key">声音效果的Key，或者只是简单地写下声音的名称，应该与路径相匹配</param>
        /// <param name="isLoop">是否循环</param>
        public void PlaySound(string key, bool isLoop = false)
        {
            AudioManager.Instance.PlaySound(key, isLoop);
        }
        /// <summary>
        /// 停止一个循环音效
        /// </summary>
        /// <param name="key"></param>
        public void StopSound(string key)
        {
            AudioManager.Instance.StopSound(key);
        }
        /// <summary>
        /// 切换一个BGM/播放BGM
        /// </summary>
        /// <param name="key"></param>
        public void ChangeBGM(string key)
        {
            AudioManager.Instance.ChangeBGM(key);
        }


        /// <summary>
        /// BGM播放控制
        /// </summary>
        public bool ISPlayMusic
        {
            get
            {
                return AudioManager.Instance.ISPlayMusic;
            }
            set
            {
                AudioManager.Instance.ISPlayMusic = value;
            }
        }

        /// <summary>
        /// 音效播放控制
        /// </summary>
        public bool ISPlaySound
        {
            get
            {
                return AudioManager.Instance.ISPlaySound;
            }
            set
            {
                AudioManager.Instance.ISPlaySound = value;
            }
        }

        /// <summary>
        /// BGM大小控制
        /// </summary>
        public float MusicVolume
        {
            get
            {
                return AudioManager.Instance.MusicVolume;
            }
            set
            {
                AudioManager.Instance.MusicVolume = value;
            }
        }

        /// <summary>
        /// 音效大小控制
        /// </summary>
        public float SoundVolume
        {
            get
            {
                return AudioManager.Instance.SoundVolume;
            }
            set
            {
                AudioManager.Instance.SoundVolume = value;
            }
        }

    }
}