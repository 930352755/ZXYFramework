using System;
using UnityEngine;
namespace Game
{
    /// <summary>
    /// 本地数据持久化
    /// 非常轻便简洁，使用过快捷
    /// Author:翼小鬼
    /// 2021/10/20
    /// </summary>
    public static class PPData
    {

        /// <summary>
        /// 保存Int类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">Int数据</param>
        public static void SetInt(string key, int value)
        {
            PPDataSave.Instance.SetData(key, value);
        }
        /// <summary>
        /// 获取Int类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一/param>
        /// <returns>Int数据</returns>
        public static int GetInt(string key, int defaultValue = default)
        {
            return PPDataSave.Instance.GetData(key, default).ASInt();
        }

        /// <summary>
        /// 保存Float类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">Float数据</param>
        public static void SetFloat(string key, float value)
        {
            PPDataSave.Instance.SetData(key, value);
        }
        /// <summary>
        /// 获取Float类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <returns>Float数据</returns>
        public static float GetFloat(string key, float defaultValue = default)
        {
            return PPDataSave.Instance.GetData(key, default).ASFloat();
        }

        /// <summary>
        /// 保存String类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">String数据</param>
        public static void SetString(string key, string value)
        {
            PPDataSave.Instance.SetData(key, value);
        }
        /// <summary>
        /// 获取String类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <returns>String数据</returns>
        public static string GetString(string key, string defaultValue = default)
        {
            return PPDataSave.Instance.GetData(key, default).ASString();
        }

        /// <summary>
        /// 保存Bool类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">Bool数据</param>
        public static void SetBool(string key, bool value)
        {
            PPDataSave.Instance.SetData(key, value);
        }
        /// <summary>
        /// 获取Bool类型的数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <returns>Bool数据</returns>
        public static bool GetBool(string key, bool defaultValue = default)
        {
            return PPDataSave.Instance.GetData(key, default).ASBool();
        }

        /// <summary>
        /// 保存Enum类型的数据
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">Enum数据</param>
        public static void SetEnum<T>(string key, T value) where T : Enum
        {
            PPDataSave.Instance.SetData(key, value);
        }
        /// <summary>
        /// 获取Enum类型的数据
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="key">Key值：唯一</param>
        /// <returns>Enum数据</returns>
        public static T GetEnum<T>(string key, T defaultValue = default) where T : Enum
        {
            return PPDataSave.Instance.GetData(key, default).AS<T>();
        }

        /// <summary>
        /// 保存Object类型的数据(Object可序列化)
        /// </summary>
        /// <typeparam name="T">Object类型</typeparam>
        /// <param name="key">Key值：唯一</param>
        /// <param name="value">Object数据</param>
        public static void SetObject<T>(string key, T value)
        {
            PPDataSave.Instance.SetData(key, value.ToJson());
        }
        /// <summary>
        /// 获取Object类型的数据(Object可序列化)
        /// </summary>
        /// <typeparam name="T">Object类型</typeparam>
        /// <param name="key">Key值：唯一</param>
        /// <returns>Object数据</returns>
        public static T GetObject<T>(string key, T defaultValue = default)
        {
            return PPDataSave.Instance.GetData(key, defaultValue.ToJson()).ASString().FromJson<T>();
        }

        /// <summary>
        /// 是否存在某个数据
        /// </summary>
        /// <param name="key">Key值：唯一</param>
        /// <returns>存在与否</returns>
        public static bool HasKey(string key)
        {
            return PPDataSave.Instance.ISHaveKey(key);
        }

    }
}