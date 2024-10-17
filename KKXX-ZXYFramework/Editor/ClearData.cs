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
        [UnityEditor.MenuItem("Game/清理所有存档信息", false, 2000)]
        private static void ClearAllData()
        {
            PPData.DeleteAll();
            if (Directory.Exists(Application.persistentDataPath)) Directory.Delete(Application.persistentDataPath, true);
            EditorUtility.DisplayDialog("清理数据", "清理完成，已经全部清理", "确定");
        }
    }
}
