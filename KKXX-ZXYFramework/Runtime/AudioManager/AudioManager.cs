using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace Game
{

    /// <summary>
    ///  20250308
    ///  音音效管理
    ///  自动加载启动，自动启动项目
    /// </summary>
    public class AudioManager : MonoBehaviour
    {

        #region 初始化

        /// <summary>
        /// 开机自起动
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            Instance.StartAudioSystem();
        }

        /// <summary>
        /// 开始吧
        /// </summary>
        private void StartAudioSystem()
        {
            UpdataManager.Instance.StartCoroutine(AwaitResInit());
        }

        /// <summary>
        /// 等待初始化
        /// </summary>
        /// <returns></returns>
        private IEnumerator AwaitResInit()
        {
            yield return new WaitUntil(() => { return YooResManager.Instance.ISInitial; });
            Debug.Log("<Color=#E60000>声音系统初始化完成</Color>");
        }

        #endregion

        #region 调用处理

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
        /// 背景音乐控制属性（读写）
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
                }
                else
                {
                    StopMusic();
                }
            }
        }

        /// <summary>
        /// 音效控制属性（读写）
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
                if (!value)
                {
                    StopAllLoopSound();
                }
            }
        }

        /// <summary>
        /// 背景音乐大小控制
        /// </summary>
        public float MusicVolume
        {
            get
            {
                return QuickData.GetFloat("Game_AudioManager_MusicVolume", 1f);
            }
            set
            {
                QuickData.SetFloat("Game_AudioManager_MusicVolume", value);
                if (aSMusic != null)
                {
                    aSMusic.volume = value;
                }
            }
        }

        /// <summary>
        /// 音效大小控制
        /// </summary>
        public float SoundVolume
        {
            get
            {
                return QuickData.GetFloat("Game_AudioManager_SoundVolume", 1f);
            }
            set
            {
                QuickData.SetFloat("Game_AudioManager_SoundVolume", value);
            }
        }

        #endregion

        #region 单例

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
                }
                return instance;
            }
        }

        #endregion

        #region 音效数据

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

        #region 音频数据的存储

        /// <summary>
        /// 当前设置的BGM
        /// </summary>
        private string curBGM
        {
            get
            {
                return QuickData.GetString("Game_AudioManager_curBGM", "");
            }
            set
            {
                QuickData.SetString("Game_AudioManager_curBGM",value);
            }
        }

        private bool isPlayMusic
        {
            get
            {
                return QuickData.GetBool("Game_AudioManager_isPlayMusic", true);
            }
            set
            {
                QuickData.SetBool("Game_AudioManager_isPlayMusic", value);
            }
        }

        private bool isPlaySound
        {
            get
            {
                return QuickData.GetBool("Game_AudioManager_isPlaySound", true);
            }
            set
            {
                QuickData.SetBool("Game_AudioManager_isPlaySound", value);
            }
        }

        #endregion

        #region 控制中心

        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        #region 普通音效处理

        /// <summary>
        /// 音效
        /// </summary>
        private List<AudioSource> soundPool = new List<AudioSource>();

        /// <summary>
        /// 音效
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

        #region 循环音频

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
        /// BGM管理
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