using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PPDataSave : DataBase
    {
        private static PPDataSave instance;
        public static PPDataSave Instance
        {
            get
            {
                if (instance == null) instance = new PPDataSave();
                return instance;
            }
        }
        private PPDataSave()
        {
            LoadData();
            allData = new Dictionary<string, object>();
            int count1 = allKey.Count;
            int count2 = allValue.Count;
            if (count1 == count2)
            {
                for (int i = 0; i < count1; i++)
                {
                    allData.Add(allKey[i], allValue[i]);
                }
            }
            else
            {
                Debug.LogError("存档出现大问题了");
            }
        }
        public void Save()
        {
            allKey = new List<string>();
            allValue = new List<object>();
            foreach (var item in allData)
            {
                allKey.Add(item.Key);
                allValue.Add(item.Value);
            }
            SaveData();
        }
        public List<string> allKey;
        public List<object> allValue;

        private Dictionary<string, object> allData;

        public void SetData(string key, object value)
        {
            if (allData.ContainsKey(key))
            {
                allData[key] = value;
            }
            else
            {
                allData.Add(key, value);
            }
            Save();
        }
        public object GetData(string key, object o)
        {
            if (allData.ContainsKey(key))
            {
                return allData[key];
            }
            return o;
        }

        public bool ISHaveKey(string key)
        {
            return allData.ContainsKey(key);
        }

    }
}
