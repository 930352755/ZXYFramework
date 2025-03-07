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

        #region 自动初始化 单例

        /// <summary>
        /// 开机自起动
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnRuntimeMethodLoad()
        {
            AudioManager _ = Instance;
        }

        private static AudioManager _instance = null;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject ins = new GameObject("AudioManager");
                    GameObject.DontDestroyOnLoad(ins);
                    _instance = ins.AddComponent<AudioManager>();
                }
                return _instance;
            }
        }

        #endregion

        #region 属性控制

        /// <summary>
        /// 储存 当前设置的BGM
        /// </summary>
        public string CurrentBGM
        {
            get => QuickData.GetString("Game_AudioManager_CurrentBGM", "");
            private set => QuickData.SetString("Game_AudioManager_CurrentBGM", value);
        }

        /// <summary>
        /// 背景音乐控制属性（读写）
        /// </summary>
        public bool IsMusicEnabled
        {
            get => QuickData.GetBool("Game_AudioManager_isPlayMusic", true);
            set
            {
                QuickData.SetBool("Game_AudioManager_isPlayMusic", value);
                if (value && !string.IsNullOrEmpty(CurrentBGM))
                {
                    PlayMusic(CurrentBGM);
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
        public bool IsSoundEnabled
        {
            get => QuickData.GetBool("Game_AudioManager_isPlaySound", true);
            set
            {
                QuickData.SetBool("Game_AudioManager_isPlaySound", value);
                if (!value) StopAllLoopSounds();
            }
        }

        /// <summary>
        /// 背景音乐大小控制
        /// </summary>
        public float MusicVolume
        {
            get => QuickData.GetFloat("Game_AudioManager_MusicVolume", 1f);
            set
            {
                QuickData.SetFloat("Game_AudioManager_MusicVolume", Mathf.Clamp01(value));
                if (_musicSource != null) _musicSource.volume = value;
            }
        }

        /// <summary>
        /// 音效大小控制
        /// </summary>
        public float SoundVolume
        {
            get => QuickData.GetFloat("Game_AudioManager_SoundVolume", 1f);
            set => QuickData.SetFloat("Game_AudioManager_SoundVolume", Mathf.Clamp01(value));
        }

        #endregion

        #region 调用处理

        public void ChangeBGM(string key)
        {
            if (!IsMusicEnabled) return;
            AudioData audioData = GetAudioData(key);
            string name = audioData.name;
            CurrentBGM = name;
            IsMusicEnabled = false;
            IsMusicEnabled = true;
        }

        public void PlaySound(string key, bool isLoop = false)
        {
            if (!IsSoundEnabled) return;

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

        #endregion

        #region 音效数据，可配合读表，这是一个拓展

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

        #region 控制中心

        private readonly Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        #region 普通音效处理

        /// <summary>
        /// 音效
        /// </summary>
        private readonly List<AudioSource> soundPool = new List<AudioSource>();

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

            AudioSource aS = GetASound();
            aS.clip = audioClips[name];
            aS.volume = volume;
            aS.loop = false;
            aS.Play();
            RecycleSound(aS, audioClips[name].length);
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
                aS.clip = null;
                if (soundPool.Count < 10)
                {
                    aS.gameObject.SetActive(false);
                    soundPool.Add(aS);
                }
                else
                {
                    GameObject.Destroy(aS.gameObject);
                }
                
            }, time));
        }

        private IEnumerator DelayedCall(System.Action action, float time)
        {
            yield return new WaitForSecondsRealtime(time);
            action?.Invoke();
        }

        #endregion

        #region 循环音频

        private Dictionary<string, AudioSource> soundLoopPool = new Dictionary<string, AudioSource>();

        /// <summary>
        /// 播放一个循环音效
        /// 每一个音效都是独立的AudioSource
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
                AudioSource aSM = (new GameObject("LoopSound"+ name)).AddComponent<AudioSource>();
                aSM.transform.parent = transform;
                aSM.clip = audioClips[name];
                aSM.volume = volume;
                aSM.loop = true;
                aSM.Play();
                soundLoopPool.Add(name, aSM);
            }
        }

        /// <summary>
        /// 停止一个循环音效
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
        /// 停止所有循环音效
        /// </summary>
        private void StopAllLoopSounds()
        {
            foreach (AudioSource aS in soundLoopPool.Values)
            {
                aS.Stop();
                aS.gameObject.SetActive(false);
            }
        }

        #endregion

        #region 背景音乐

        /// <summary>
        /// BGM管理
        /// </summary>
        private AudioSource _musicSource = null;

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
            if (_musicSource != null)
            {
                if (_musicSource.isPlaying) return;
                _musicSource.gameObject.SetActive(true);
                _musicSource.clip = audioClips[name];
                _musicSource.volume = volume;
                _musicSource.loop = true;
                _musicSource.Play();
            }
            else
            {
                _musicSource = (new GameObject("Music")).AddComponent<AudioSource>();
                _musicSource.transform.parent = transform;
                _musicSource.clip = audioClips[name];
                _musicSource.volume = volume;
                _musicSource.loop = true;
                _musicSource.Play();
            }
        }

        private void StopMusic()
        {
            if (_musicSource != null)
            {
                _musicSource.Stop();
                _musicSource.gameObject.SetActive(false);
            };
        }

        #endregion

        #endregion

    }
}