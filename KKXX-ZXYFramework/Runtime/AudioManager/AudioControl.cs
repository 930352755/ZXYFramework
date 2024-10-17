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

        #region Fast data processing

        /// <summary>
        /// BGM Initial play of BGM
        /// </summary>
        public readonly static string BGM = "BGM";
        /// <summary>
        /// The name of the button sound
        /// </summary>
        public readonly static string ButtonAudio = "ButtonClick";
        /// <summary>
        /// Turn off button sound effects
        /// </summary>
        public readonly static string ButtonClose = "ButtonClose";
        /// <summary>
        /// The name of the button event
        /// </summary>
        public readonly static string ButtonEvent = "ButtonClick";

        /// <summary>
        /// Audio path
        /// </summary>
        public const string AudioSoundPath = "Audios/";
        /// <summary>
        /// Path of music
        /// </summary>
        public const string AudioMusicPath = "Audios/BGM/";

        #endregion

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="key">The Key of the sound effect, or simply write the name of the sound, should match the path</param>
        /// <param name="isLoop">Loop or not</param>
        public void PlaySound(string key, bool isLoop = false)
        {
            AudioManager.Instance.PlaySound(key, isLoop);
        }
        /// <summary>
        /// Stop a loop sound effect
        /// </summary>
        /// <param name="key"></param>
        public void StopSound(string key)
        {
            AudioManager.Instance.StopSound(key);
        }
        /// <summary>
        /// Change a BGM
        /// </summary>
        /// <param name="key"></param>
        public void ChangeBGM(string key)
        {
            AudioManager.Instance.ChangeBGM(key);
        }


        /// <summary>
        /// BGM control properties (read-write)
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
        /// Audio control properties (read-write)
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
        /// Background music size control
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
        /// Control of sound effect size
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