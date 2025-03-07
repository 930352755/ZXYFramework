using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace Game
{
    /// <summary>
    /// 清理数据
    /// </summary>
    public class ClearData
    {
        [UnityEditor.MenuItem("Game/清理所有存档信息", false, 3000)]
        private static void ClearAllData()
        {
            //清理这个数据，建议以后不要用这个了
            PlayerPrefs.DeleteAll();
            //所有可读写目录下的路径
            if (Directory.Exists(Application.persistentDataPath)) Directory.Delete(Application.persistentDataPath, true);

            EditorUtility.DisplayDialog("清理数据", "清理完成，已经全部清理", "确定");
        }
    }
}
