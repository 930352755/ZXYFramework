using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace Game
{

    /// <summary>
    ///  20220922
    ///  Simple sound management system
    ///  No mount, called when Loading is loaded.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {

        #region Initial

        /// <summary>
        /// For external use
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            Instance.StartAudioSystem();
        }

        /// <summary>
        /// Initial point
        /// Startup script
        /// </summary>
        private void StartAudioSystem()
        {
            UpdataManager.Instance.StartCoroutine(AwaitResInit());
        }

        private IEnumerator AwaitResInit()
        {
            yield return new WaitUntil(() => { return YooResManager.Instance.ISInitial; });
            ChangeBGM(AudioControl.BGM);
            Debug.Log("<Color=#E60000>声音系统初始化完成</Color>");
        }

        #endregion

        #region For external use

        public void PlaySound(string key, bool isLoop = false)
        {
            AudioData audioData = GetAudioData(key);
            string name = audioData.name;
            if (isLoop)
            {
                PlayLoopSound(name, SoundVolume * audioData.volume);
            }
            else
            {
                PlaySound(name, SoundVolume * audioData.volume);
            }
        }

        public void StopSound(string key)
        {
            AudioData audioData = GetAudioData(key);
            string name = audioData.name;
            StopLoopSound(name);
        }

        public void ChangeBGM(string key)
        {

            AudioData audioData = GetAudioData(key);
            string name = audioData.name;
            curBGM = name;
            if (ISPlayMusic)
            {
                ISPlayMusic = false;
                ISPlayMusic = true;
            }

        }

        /// <summary>
        /// BGM control properties (read-write)
        /// </summary>
        public bool ISPlayMusic
        {
            get
            {
                return isPlayMusic;
            }
            set
            {
                isPlayMusic = value;
                if (value)
                {
                    PlayMusic(curBGM, MusicVolume);
                    PlayerPrefs.SetInt("isPlayMusic", 1);
                }
                else
                {
                    StopMusic();
                    PlayerPrefs.SetInt("isPlayMusic", 0);
                }
            }
        }

        /// <summary>
        /// Audio control properties (read-write)
        /// </summary>
        public bool ISPlaySound
        {
            get
            {
                return isPlaySound;
            }
            set
            {
                isPlaySound = value;
                if (value)
                {
                    PlayerPrefs.SetInt("isPlaySound", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("isPlaySound", 0);
                    StopAllLoopSound();
                }
            }
        }

        /// <summary>
        /// Background music size control
        /// </summary>
        public float MusicVolume
        {
            get
            {
                return PlayerPrefs.GetFloat("MusicVolume", 1f);
            }
            set
            {
                PlayerPrefs.SetFloat("MusicVolume", value);
                if (aSMusic != null)
                {
                    aSMusic.volume = value;
                }
            }
        }

        /// <summary>
        /// Control of sound effect size
        /// </summary>
        public float SoundVolume
        {
            get
            {
                return PlayerPrefs.GetFloat("SoundVolume", 1f);
            }
            set
            {
                PlayerPrefs.SetFloat("SoundVolume", value);
            }
        }

        #endregion

        #region singleton

        private static AudioManager instance = null;

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject ins = new GameObject("AudioManager");
                    GameObject.DontDestroyOnLoad(ins);
                    instance = ins.AddComponent<AudioManager>();
                    instance.AAwake();
                }
                return instance;
            }
        }

        #endregion

        #region Sound data sheet

        public class AudioData
        {
            public string name;
            public string des;
            public float volume;
        }

        public AudioData GetAudioData(string key)
        {
            AudioData audioData = new AudioData();
            audioData.name = key;
            audioData.volume = 1;
            return audioData;
        }

        #endregion

        #region Data

        /// <summary>
        /// The current BGM
        /// </summary>
        private string curBGM;

        private bool isPlayMusic;

        private bool isPlaySound;

        #endregion

        #region initialize

        /// <summary>
        /// initialize
        /// </summary>
        private void AAwake()
        {
            isPlayMusic = PlayerPrefs.GetInt("isPlayMusic", 1) == 1;
            isPlaySound = PlayerPrefs.GetInt("isPlaySound", 1) == 1;
        }

        #endregion

        #region Controls

        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        #region General sound effect

        /// <summary>
        /// Non-loop sound pool
        /// </summary>
        private List<AudioSource> soundPool = new List<AudioSource>();

        /// <summary>
        /// Play a non-loop sound effect
        /// </summary>
        private void PlaySound(string name, float volume = 1f)
        {
            if (!audioClips.ContainsKey(name))
            {
                YooResManager.Instance.LoadAssetAsync<AudioClip>(AudioControl.AudioSoundPath + name,(ac)=>
                {
                    if (!audioClips.ContainsKey(name))
                    {
                        if (ac == null)
                        {
                            Debug.LogError(AudioControl.AudioSoundPath + "下没有资源" + name);
                            return;
                        }
                        audioClips.Add(name, ac);
                    }
                    PlaySound(name, volume);
                });
                return;
            };
            if (isPlaySound)
            {
                AudioSource aS = GetASound();
                aS.clip = audioClips[name];
                aS.volume = volume;
                aS.loop = false;
                aS.Play();
                RecycleSound(aS, audioClips[name].length);
            }
        }

        private AudioSource GetASound()
        {
            if (soundPool.Count > 0)
            {
                AudioSource aS = soundPool[0];
                soundPool.RemoveAt(0);
                aS.gameObject.SetActive(true);
                return aS;
            }
            else
            {
                AudioSource aS = (new GameObject("ASound")).AddComponent<AudioSource>();
                aS.transform.parent = transform;
                return aS;
            }
        }

        private void RecycleSound(AudioSource aS, float time)
        {
            StartCoroutine(DelayedCall(() =>
            {
                aS.gameObject.SetActive(false);
                soundPool.Add(aS);
            }, time));
        }

        private IEnumerator DelayedCall(System.Action action, float time)
        {
            yield return new WaitForSecondsRealtime(time);
            if (action != null)
            {
                action();
            }
        }

        #endregion

        #region Loop sound

        private Dictionary<string, AudioSource> soundLoopPool = new Dictionary<string, AudioSource>();

        /// <summary>
        /// Play a loop sound effect
        /// </summary>
        private void PlayLoopSound(string name, float volume = 1f)
        {
            if (!audioClips.ContainsKey(name))
            {
                YooResManager.Instance.LoadAssetAsync<AudioClip>(AudioControl.AudioSoundPath + name,(ac)=>
                {
                    if (ac == null)
                    {
                        Debug.LogError(AudioControl.AudioSoundPath + "下没有资源" + name);
                        return;
                    }
                    audioClips.Add(name, ac);
                    PlayLoopSound(name, volume);
                });
                return;
            };
            if (isPlaySound)
            {

                if (soundLoopPool.TryGetValue(name, out AudioSource aS))
                {
                    if (aS.isPlaying) return;
                    aS.gameObject.SetActive(true);
                    aS.clip = audioClips[name];
                    aS.volume = volume;
                    aS.loop = true;
                    aS.Play();
                }
                else
                {
                    AudioSource aSM = (new GameObject("LoopSound")).AddComponent<AudioSource>();
                    aSM.transform.parent = transform;
                    aSM.clip = audioClips[name];
                    aSM.volume = volume;
                    aSM.loop = true;
                    aSM.Play();
                    soundLoopPool.Add(name, aSM);
                }
            }
        }

        /// <summary>
        /// Stop a loop sound effect
        /// </summary>
        /// <param name="name"></param>
        private void StopLoopSound(string name)
        {
            if (soundLoopPool.TryGetValue(name, out AudioSource aS))
            {
                aS.Stop();
                aS.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Stop all looping sounds
        /// </summary>
        private void StopAllLoopSound()
        {
            foreach (AudioSource aS in soundLoopPool.Values)
            {
                aS.Stop();
                aS.gameObject.SetActive(false);
            }
        }

        #endregion

        #region BGM

        /// <summary>
        /// BGM
        /// </summary>
        private AudioSource aSMusic = null;

        private void PlayMusic(string name, float volume = 1f)
        {
            if (!audioClips.ContainsKey(name))
            {
                YooResManager.Instance.LoadAssetAsync<AudioClip>(AudioControl.AudioMusicPath + name,(ac)=>
                {
                    if (ac == null)
                    {
                        Debug.LogError(AudioControl.AudioMusicPath + "下没有资源" + name);
                        return;
                    }
                    audioClips.Add(name, ac);
                    PlayMusic(name, volume);
                });
                return;
            };
            if (aSMusic != null)
            {
                if (aSMusic.isPlaying) return;
                aSMusic.gameObject.SetActive(true);
                aSMusic.clip = audioClips[name];
                aSMusic.volume = volume;
                aSMusic.loop = true;
                aSMusic.Play();
            }
            else
            {
                aSMusic = (new GameObject("Music")).AddComponent<AudioSource>();
                aSMusic.transform.parent = transform;
                aSMusic.clip = audioClips[name];
                aSMusic.volume = volume;
                aSMusic.loop = true;
                aSMusic.Play();
            }
        }

        private void StopMusic()
        {
            if (aSMusic != null)
            {
                aSMusic.Stop();
                aSMusic.gameObject.SetActive(false);
            };
        }

        #endregion

        #endregion
    }
}