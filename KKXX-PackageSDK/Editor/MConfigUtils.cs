using System;
using System.Collections.Generic;
using UnityEditor;

namespace Game 
{
    public static class MSymbol
    {
        public static string DEBUG = "DEBUG";
    }
    public static class MConfigUtils 
    {

        public static string GetBuildTargetName(BuildTarget target) 
        {
            return Enum.GetName(typeof(BuildTarget), target);
        }

        /// <summary>
        /// 是否启用这宏
        /// </summary>
        /// <param name="symble">宏</param>
        /// <param name="enable">是否添加</param>
        public static void SymbleUpdate(string symble, bool enable)
        {
            BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            List<string> symbolArray = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));

            if (enable)
            {
                if (!symbolArray.Contains(symble))
                {
                    symbolArray.Add(symble);
                }
            }
            else
            {
                symbolArray.Remove(symble);
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, String.Join(";", symbolArray));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 将一个字符串列表 每一个都移除，添加一个宏
        /// </summary>
        /// <param name="symble">宏</param>
        /// <param name="choice">字符串列表</param>
        public static void SymbleChoice(string symble, List<string> choice) {
            BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            List<string> symbolArray = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));

            foreach (var item in choice)
            {
                if (symbolArray.Contains(item)) {
                    symbolArray.Remove(item);
                }
            }
            symbolArray.Add(symble);
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, String.Join(";", symbolArray));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 拿到一个字符串列表中第一个在宏里的字符串。
        /// </summary>
        /// <param name="choice">字符串列表</param>
        /// <returns></returns>
        public static string GetSymbleWithChoice(List<string> choice) {
            BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            List<string> symbolArray = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
            foreach (string item in symbolArray)
            {
                if (choice.Contains(item)) {
                    return item;
                }
            }
            return null;
        }



        /// <summary>
        /// 添加宏
        /// </summary>
        /// <param name="macro">要添加的宏</param>
        public static void AddMacro(string macro)
        {
            //开发平台
            BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            //所有的宏
            string allMacro = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            List<string> macroList = new List<string>(allMacro.Split(';'));

            if (macroList.Contains(macro)) return;
            macroList.Add(macro);

            string str = String.Join(";", macroList);//将所有宏编辑成数组
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, str);//写入宏
            AssetDatabase.SaveAssets();//（保存必不可少）
            AssetDatabase.Refresh();//（刷新）
        }
        /// <summary>
        /// 移除宏
        /// </summary>
        /// <param name="macro"></param>
        public static void RemoveMacro(string macro)
        {
            //开发平台
            BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            //所有的宏
            string allMacro = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            List<string> macroList = new List<string>(allMacro.Split(';'));

            if (!macroList.Contains(macro)) return;
            macroList.Remove(macro);

            string str = String.Join(";", macroList);//将所有宏编辑成数组
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, str);//写入宏
            AssetDatabase.SaveAssets();//（保存必不可少）
            AssetDatabase.Refresh();//（刷新）
        }
        /// <summary>
        /// 检测是否有某个宏
        /// </summary>
        /// <param name="macro">宏</param>
        /// <returns></returns>
        public static bool CheckMacro(string macro)
        {
            //开发平台
            BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            //所有的宏
            string allMacro = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            List<string> macroList = new List<string>(allMacro.Split(';'));
            return macroList.Contains(macro);
        }
        /// <summary>
        /// 检测是否有某个宏
        /// </summary>
        /// <param name="symble">宏</param>
        /// <returns></returns>
        public static bool SymbleCheck(string symble)
        {
            BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            List<string> symbolArray = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
            foreach (string item in symbolArray)
            {
                if (symble == item)
                {
                    return true;
                }
            }
            return false;
        }

    }
}