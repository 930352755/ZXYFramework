using System.Collections.Generic;

namespace Game
{

    /// <summary>
    /// 快速数据储取
    /// 对Unity List 的处理，实现可以快速存储的操作
    /// </summary>
    [System.Serializable]
    public class QuickData : DataBase
    {

        private static QuickData _instance = null;
        private static QuickData Instance
        {
            get
            {
                if (_instance == null) _instance = new QuickData();
                return _instance;
            }
        }

        private QuickData()
        {
            LoadData();
        }

        public SaveString saveString = new SaveString();
        public SaveInt saveInt = new SaveInt();
        public SaveBool saveBool = new SaveBool();
        public SaveFloat saveFloat = new SaveFloat();
        public SaveDouble saveDouble = new SaveDouble();

        /// <summary>
        /// 保存的所有字符串元素
        /// </summary>
        [System.Serializable]
        public class SaveString
        {
            public List<string> allKey = new List<string>();
            public List<string> allValue = new List<string>();
            public string GetValue(string key,string defaultValue)
            {

                if (!allKey.Contains(key)) return defaultValue;

                int index = FindFirstIndexByValue(allKey, key);
                return allValue[index];

            }
            public void SetValue(string key ,string value)
            {
                if (allKey.Contains(key))
                {
                    int index = FindFirstIndexByValue(allKey, key);
                    allValue[index] = value;
                }
                else
                {
                    allKey.Add(key);
                    allValue.Add(value);
                }
            }
        }
        [System.Serializable]
        public class SaveInt
        {
            public List<string> allKey = new List<string>();
            public List<int> allValue = new List<int>();
            public int GetValue(string key, int defaultValue)
            {
                if (!allKey.Contains(key)) return defaultValue;
                int index = FindFirstIndexByValue(allKey, key);
                return allValue[index];
            }
            public void SetValue(string key, int value)
            {
                if (allKey.Contains(key))
                {
                    int index = FindFirstIndexByValue(allKey, key);
                    allValue[index] = value;
                }
                else
                {
                    allKey.Add(key);
                    allValue.Add(value);
                }
            }
        }
        [System.Serializable]
        public class SaveBool
        {
            public List<string> allKey = new List<string>();
            public List<bool> allValue = new List<bool>();
            public bool GetValue(string key, bool defaultValue)
            {
                if (!allKey.Contains(key)) return defaultValue;
                int index = FindFirstIndexByValue(allKey, key);
                return allValue[index];
            }
            public void SetValue(string key, bool value)
            {
                if (allKey.Contains(key))
                {
                    int index = FindFirstIndexByValue(allKey, key);
                    allValue[index] = value;
                }
                else
                {
                    allKey.Add(key);
                    allValue.Add(value);
                }
            }
        }
        [System.Serializable]
        public class SaveFloat
        {
            public List<string> allKey = new List<string>();
            public List<float> allValue = new List<float>();
            public float GetValue(string key, float defaultValue)
            {
                if (!allKey.Contains(key)) return defaultValue;
                int index = FindFirstIndexByValue(allKey, key);
                return allValue[index];
            }
            public void SetValue(string key, float value)
            {
                if (allKey.Contains(key))
                {
                    int index = FindFirstIndexByValue(allKey, key);
                    allValue[index] = value;
                }
                else
                {
                    allKey.Add(key);
                    allValue.Add(value);
                }
            }
        }
        [System.Serializable]
        public class SaveDouble
        {
            public List<string> allKey = new List<string>();
            public List<double> allValue = new List<double>();
            public double GetValue(string key, double defaultValue)
            {
                if (!allKey.Contains(key)) return defaultValue;
                int index = FindFirstIndexByValue(allKey, key);
                return allValue[index];
            }
            public void SetValue(string key, double value)
            {
                if (allKey.Contains(key))
                {
                    int index = FindFirstIndexByValue(allKey, key);
                    allValue[index] = value;
                }
                else
                {
                    allKey.Add(key);
                    allValue.Add(value);
                }
            }
        }

        /// <summary>
        /// 保存String类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">String数据</param>
        public static void SetString(string key,string value)
        {
            QuickData.Instance.saveString.SetValue(key, value);
            QuickData.Instance.SaveData();
        }
        /// <summary>
        /// 获取String类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <returns>String数据</returns>
        public static string GetString(string key, string defaultValue = default)
        {
            return QuickData.Instance.saveString.GetValue(key, defaultValue);
        }

        /// <summary>
        /// 保存Int类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">Int数据</param>
        public static void SetInt(string key, int value)
        {
            QuickData.Instance.saveInt.SetValue(key, value);
            QuickData.Instance.SaveData();
        }
        /// <summary>
        /// 获取Int类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一/param>
        /// <returns>Int数据</returns>
        public static int GetInt(string key, int defaultValue = default)
        {
            return QuickData.Instance.saveInt.GetValue(key, defaultValue);
        }

        /// <summary>
        /// 保存Bool类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">Bool数据</param>
        public static void SetBool(string key, bool value)
        {
            QuickData.Instance.saveBool.SetValue(key, value);
            QuickData.Instance.SaveData();
        }
        /// <summary>
        /// 获取Bool类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <returns>Bool数据</returns>
        public static bool GetBool(string key, bool defaultValue = default)
        {
            return QuickData.Instance.saveBool.GetValue(key, defaultValue);
        }

        /// <summary>
        /// 保存Float类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">Float数据</param>
        public static void SetFloat(string key, float value)
        {
            QuickData.Instance.saveFloat.SetValue(key, value);
            QuickData.Instance.SaveData();
        }
        /// <summary>
        /// 获取Float类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <returns>Float数据</returns>
        public static float GetFloat(string key, float defaultValue = default)
        {
            return QuickData.Instance.saveFloat.GetValue(key, defaultValue);
        }

        /// <summary>
        /// 保存Double类型的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetDouble(string key, double value)
        {
            QuickData.Instance.saveDouble.SetValue(key, value);
            QuickData.Instance.SaveData();
        }
        /// <summary>
        /// 获取Double类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <returns>Float数据</returns>
        public static double GetDouble(string key, double defaultValue = default)
        {
            return QuickData.Instance.saveDouble.GetValue(key, defaultValue);
        }

        /// <summary>
        /// 保存Enum类型的数据
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">Enum数据</param>
        public static void SetEnum<T>(string key, T value) where T : System.Enum
        {
            QuickData.Instance.saveString.SetValue(key, value.ToString());
            QuickData.Instance.SaveData();
        }
        /// <summary>
        /// 获取Enum类型的数据
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="key">Key值：唯一</param>
        /// <returns>Enum数据</returns>
        public static T GetEnum<T>(string key, T defaultValue = default) where T : System.Enum
        {
            return (T)System.Enum.Parse(typeof(T), QuickData.Instance.saveString.GetValue(key, defaultValue.ToString()));
        }



        /// <summary>
        /// 拿到这列表中这个列表元素的第一个索引
        /// </summary>
        /// <param name="self">List<string>字符串列表</param>
        /// <param name="t">字符串</param>
        /// <returns>这个字符串所在索引</returns>
        private static int FindFirstIndexByValue(List<string> self, string t)
        {
            int count = self.Count;
            for (int i = 0; i < count; i++)
            {
                if (self[i] == t)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}