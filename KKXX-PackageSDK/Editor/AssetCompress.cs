
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Text;

public class AssetCompress : MonoBehaviour
{
    public static string[] searchList = new string[]{
        "Texture",
        "AudioClip",
    };

    public static Dictionary<string, bool> ignoreFiles = null;
    public static string[] ignoreList = new string[]
    {
        "Editor",
        ".renderTexture",
    };

    [MenuItem("Game/混淆资源/1.改变图片和音效MD5", false,1997)]
    public static void Compress()
    {
        // DealSpriteAtlas();
        var mapping = new Dictionary<string, bool>();
        
        // 转移文件
        foreach (var filter in searchList)
        {
            Debug.Log("filter: " + filter);
            string[] assetList = AssetDatabase.FindAssets("t:"+filter, new string[]{"Assets"});

            foreach (string asset in assetList)
            {
                string path = AssetDatabase.GUIDToAssetPath(asset);

                bool isIgnore=false;
                foreach(string ign in ignoreList)
                {
                    if(path.Contains(ign))
                    {
                        isIgnore=true;
                        break;
                    } 
                }
                if(isIgnore) continue;

                if (mapping.ContainsKey(path))
                {
                    continue;
                }
                mapping[path] = true;

                if (!File.Exists(path)) {
                    continue;
                }
                Debug.Log("path: " + path);
                ExecuteCommand(path);
            }
        }
    }

    public static void ExecuteCommand(string file)
    {
        var bytes = File.ReadAllBytes(file);
        var newfile = File.OpenWrite(file);
        newfile.Write(bytes, 0, bytes.Length);

        var uuid = Guid.NewGuid().ToByteArray();
        newfile.Write(uuid, 0, uuid.Length);
        newfile.Close();
    }

 
        /// <summary>
        /// 执行批处理命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="workingDirectory"></param>
        public static void ExecuteCommand(string command, string workingDirectory = null)
        {
            var fProgress = .1f;
            EditorUtility.DisplayProgressBar("KEditorUtils.ExecuteCommand", command, fProgress);

            try
            {
                string cmd;
                string preArg;
                var os = Environment.OSVersion;

                UnityEngine.Debug.Log(String.Format("[ExecuteCommand]Command on OS: {0}", os.ToString()));
                if (os.ToString().Contains("Windows"))
                {
                    cmd = "cmd.exe";
                    preArg = "/C ";
                }
                else
                {
                    cmd = "sh";
                    preArg = "-c ";
                }
                UnityEngine.Debug.Log("[ExecuteCommand]" + command);
                
                using (var process = new System.Diagnostics.Process())
                {
                    System.Console.InputEncoding = System.Text.Encoding.UTF8;
                    if (workingDirectory != null)
                        process.StartInfo.WorkingDirectory = workingDirectory;
                    process.StartInfo.FileName = cmd;
                    process.StartInfo.Arguments = preArg + "\"" + command + "\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.StandardOutputEncoding = Encoding.UTF8; //设置标准输出编码
                    process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                    process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(OutputReceived);
                    process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(ErrorReceived);
                    process.Start();
                    
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
    
                    process.WaitForExit();//NOTE CMD执行中会卡住Unity主线程，如果无响应需要结束进程
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static void OutputReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Debug.Log(e.Data);
        }
        
        private static void ErrorReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data!=null&&e.Data!=string.Empty)
            {
                Debug.LogError("Error::" + e.Data);
            }
        }
}