using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 生成UI引用脚本
    /// </summary>
    public class CreateUICS
    {

        /// <summary>
        /// 固定检测文件夹
        /// </summary>
        private const string UIDirectories = "UIPrefabsPanel";
        /// <summary>
        /// 生成的脚本在Assets下的位置Path
        /// </summary>
        private static string UICSPath = "Game/Scripts/GameUI/Other_UI";

        #region UI引用生成

        [MenuItem("Game/GameUI/生成UI资源引用", false, 1000)]
        private static void Start()
        {
            if (Selection.gameObjects.Length == 1)
            {
                GameObject ui = Selection.gameObjects[0];
                if (ui.GetComponent<UIView>() != null)
                {
                    bool b = SetContentCS(ui, ui.name);
                    if (b)
                    {
                        SetUIConfig();
                        EditorUtility.DisplayDialog("UI生成完成了", "UI加载路径Json和当前面板引用全部生成完成", "确定");
                    }
                }
                else
                {
                    LogError("选择对象的问题，没有UIView组件");
                }
            }
            else
            {
                LogError("未选择要生成的UI");
            }
        }

        public static bool SetContentCS(GameObject go, string uiName)
        {
            List<GameObject> allUI = GetAllUI(go);
            if (ISSameName(allUI))
            {
                LogError("注意具有相同名称的UI");
                return false;
            }
            uiName += "_UI";
            string path = Application.dataPath + "/" + UICSPath + "/" + uiName + ".cs";

            StringBuilder areaString;
            areaString = new StringBuilder();
            //命名空间导入
            areaString.Append("using UnityEngine;\n");
            areaString.Append("using UnityEngine.UI;\n");
            areaString.Append("using Game;\n");
            areaString.Append("using Spine.Unity;\n");
            //写入类
            areaString.Append("public class " + uiName + " \n{");

            for (int i = 0; i < allUI.Count; i++)
            {
                string[] info = allUI[i].name.Split('#');
                string name = info[0];
                string ui = info[1];
                areaString.Append("\n\tpublic " + ui + " " + name + ";");
            }
            areaString.Append("\n\tpublic " + uiName + " (GameObject go)\n\t{");
            int count = allUI.Count;
            for (int i = 0; i < count; i++)
            {
                string[] info = allUI[i].name.Split('#');
                string name = info[0];
                string ui = info[1];
                if (ui == "GameObject")
                {
                    areaString.Append("\n\t\t" + name + " = UIView.FindChildTransformByName(go.transform,\"" + allUI[i].name + "\").gameObject;");
                }
                else
                {
                    areaString.Append("\n\t\t" + name + " = UIView.FindChindComponentByName<" + ui + ">(go.transform,\"" + allUI[i].name + "\");");
                }
            }
            areaString.Append("\n\t}");
            string endContent = "\n}";
            areaString.Append(endContent);

            if (!Directory.Exists(Application.dataPath + "/" + UICSPath))
            {
                Directory.CreateDirectory(Application.dataPath + "/" + UICSPath);
            }
            File.WriteAllText(path, areaString.ToString());
            AssetDatabase.Refresh();
            return true;
        }

        public static bool ISSameName(List<GameObject> allUI)
        {
            int count = allUI.Count;
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (allUI[i].name == allUI[j].name)
                    {
                        Debug.LogError(allUI[j].name);
                        return true;
                    }
                }
            }
            return false;
        }

        public static List<GameObject> GetAllUI(GameObject go)
        {
            List<GameObject> allUI = new List<GameObject>();
            int count = go.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject goc = go.transform.GetChild(i).gameObject;
                if (goc.name.Contains("#"))
                {
                    allUI.Add(goc);
                }
                allUI.AddRange(GetAllUI(goc));
            }
            return allUI;
        }

        private static void LogError(string msg)
        {
            Debug.LogError(msg);
            EditorUtility.DisplayDialog("生成失败", "！！！！！！具体请看控制台！！！！！！", "确定");
        }



        #endregion

        #region UIView 加载路径生成

        private static bool SetUIConfig()
        {
            string uiPath = Application.dataPath + "/" + YooResManager.YooAssetResPath;
            string JsonPathDir = Application.dataPath + "/" + UIManager.UIJsonPathDir;
            string JsonPath = Application.dataPath + "/" + UIManager.UIJsonPath + ".json";
            string[] uiPaths = Directory.GetDirectories(uiPath);
            List<string> allPath = new List<string>();
            for (int i = 0; i < uiPaths.Length; i++)
            {
                string sameUIPath = uiPaths[i] + "/" + UIDirectories;
                if (!Directory.Exists(sameUIPath)) continue;
                string[] sameUI = Directory.GetFiles(sameUIPath);
                for (int j = 0; j < sameUI.Length; j++)
                {
                    string aUIPath = sameUI[j];
                    if (aUIPath.Contains(".meta")) continue;
                    string value = aUIPath.Substring(uiPath.Length + 1, aUIPath.Length - uiPath.Length - 8);
                    allPath.Add(value);
                }
            }
            AllUIPathInfo allUIPathInfo = new AllUIPathInfo();
            allUIPathInfo.allPath = allPath;
            if (!Directory.Exists(JsonPathDir))
            {
                Directory.CreateDirectory(JsonPathDir);
            }
            string json = allUIPathInfo.ToJson(true);
            File.WriteAllText(JsonPath, json);
            AssetDatabase.Refresh();
            return true;
        }

        #endregion

        #region 构建项目文件夹
        [MenuItem("Game/GameUI/构建项目开发特定文件夹！", false, 1001)]
        private static void Create()
        {
            string resPath = Application.dataPath + "/" + YooResManager.YooAssetResPath;
            if (!Directory.Exists(resPath))
            {
                Directory.CreateDirectory(resPath);
            }
            string uiDirectories = Application.dataPath + "/" + YooResManager.YooAssetResPath + "/GameRes/" + UIDirectories;
            if (!Directory.Exists(uiDirectories))
            {
                Directory.CreateDirectory(uiDirectories);
            }
            string allUIScriptsPath = Application.dataPath + "/Game/Scripts/GameUI/AllUIScripts";
            if (!Directory.Exists(allUIScriptsPath))
            {
                Directory.CreateDirectory(allUIScriptsPath);
            }
            string audioDirectories = Application.dataPath + "/" + YooResManager.YooAssetResPath + "/Audios/BGM";
            if (!Directory.Exists(audioDirectories))
            {
                Directory.CreateDirectory(audioDirectories);
            }

            AssetDatabase.Refresh();
        }
        #endregion



    }
}


