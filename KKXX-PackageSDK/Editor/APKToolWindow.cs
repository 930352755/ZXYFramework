using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using YooAsset.Editor;
using YooAsset;
using System.Threading.Tasks;

namespace Game
{

    /// <summary>
    /// Keystore data
    /// </summary>
    [System.Serializable]
    public class ConfigKeystore
    {
        public string store_path = "";
        public string store_password = "";
        public string key_alias = "";
        public string key_password = "";
    }

    public class APKToolWindow : EditorWindow
    {

        private static EditorWindow _window;

        [UnityEditor.MenuItem("Game/打包工具", false, 1999)]
        private static void OpenConfigWindow()
        {
            string targetName = EditorUserBuildSettings.activeBuildTarget.ToString();
            _window = (APKToolWindow)GetWindow(typeof(APKToolWindow), false, $"Packaging configuration({targetName})", true);
            _window.minSize = new Vector2(800, 250);
            _window.Show();
            isFixkeyStore = false;
            isSeekeyStore = false;
            key = keyStorePath;
            if (string.IsNullOrEmpty(keyStorePath))
            {
                EditorUtility.DisplayDialog("Set Keystore", "Please set Keystore", "OK");
            }
            else
            {
                string keyPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), keyStorePath);
                string keyInfoPath = keyPath.Substring(0, keyPath.Length - 8) + "json";
                if (!File.Exists(keyPath) || !File.Exists(keyInfoPath))
                {
                    EditorUtility.DisplayDialog("Set the Keystore as required", $"Keystore path : ${keyPath}\n$\"Keystore Info Path : ${keyInfoPath}", "OK");
                }
                else
                {
                    string keyStoreInfo = File.ReadAllText(keyInfoPath);
                    if (string.IsNullOrEmpty(keyStoreInfo))
                    {
                        EditorUtility.DisplayDialog("The Keystore configuration is faulty. Procedure", $"Keystore path : ${keyPath}\n$\"Keystore Info Path : ${keyInfoPath}", "OK");
                    }
                    else
                    {
                        ConfigKeystore configData = keyStoreInfo.FromJson<ConfigKeystore>();
                        key_password = configData.store_password;
                        key_alias = configData.key_alias;
                        store_password = configData.key_password;
                    }
                }
            }
        }
        private static bool isFixkeyStore;
        private static bool isSeekeyStore;
        private void OnGUI()
        {

#if UNITY_ANDROID
            GUILayout.Label($"Android configuration description: Automatically set Android version A24-A34, automatically set IL2CPP. And set only ARM64");
            GUILayout.Label($"Android packaging instructions: Please set the Keystore before packaging.");

            GUILayout.BeginHorizontal();
            {

                GUILayout.BeginVertical();
                GUILayout.Label($"Version");
                PlayerSettings.bundleVersion = GUILayout.TextField(Application.version, GUILayout.Width(100));

                GUILayout.Label($"Product name");
                PlayerSettings.productName = EditorGUILayout.TextField(PlayerSettings.productName, GUILayout.Width(300));


                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label($"bundleVersionCode");
                PlayerSettings.Android.bundleVersionCode = EditorGUILayout.IntField(PlayerSettings.Android.bundleVersionCode, GUILayout.Width(100));
                GUILayout.Label($"ApplicationIdentifier");
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android,EditorGUILayout.TextField(PlayerSettings.applicationIdentifier, GUILayout.Width(300)));
                GUILayout.EndVertical();


                GUILayout.BeginVertical();
                if (GUILayout.Button("Debug the Apk"))
                {
                    SetAndroidConfigInfo();
                    BuildApk(true);
                }
                if (GUILayout.Button("Release the Apk"))
                {
                    SetAndroidConfigInfo();
                    BuildApk(false);
                }
                if (GUILayout.Button("Release the AAB"))
                {
                    SetAndroidConfigInfo();
                    BuildApk(false, true);
                }
                if (GUILayout.Button("Android project"))
                {
                    SetAndroidConfigInfo();
                    BuildApk(false, false, true);
                }
                GUILayout.EndVertical();

            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Modify or set Keystore information"))
            {
                isFixkeyStore = !isFixkeyStore;
            }
            if (isFixkeyStore)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"The path of the Keystore in the project");
                key = GUILayout.TextField(key, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Password");
                key_password = GUILayout.TextField(key_password, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Alias");
                key_alias = GUILayout.TextField(key_alias, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Alias Password");
                store_password = GUILayout.TextField(store_password, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Generate Keystore data"))
                {
                    string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "keyPath.json");
                    File.WriteAllText(path, key);
                    AddKeyStore();
                    isFixkeyStore = false;
                }
            }
            if (isSeekeyStore)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"The path of the Keystore in the project");
                GUILayout.TextField(keyStorePath, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Password");
                GUILayout.TextField(key_password, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Alias");
                GUILayout.TextField(key_alias, GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Alias Password");
                GUILayout.TextField(store_password, GUILayout.Width(300));
                GUILayout.EndHorizontal();
            }
#endif

        }

        #region Attribute information configuration

        private void SetAndroidConfigInfo()
        {

#if UNITY_ANDROID
            PlayerSettings.Android.minSdkVersion = (AndroidSdkVersions)24;
            PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)34;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
#endif

        }

        #endregion

        #region Pack

        /// <summary>
        /// Fast and secure packing
        /// </summary>
        /// <param name="isDev"></param>
        /// <param name="isBuildAppBundle"></param>
        private async void BuildApk(bool isDev, bool isBuildAppBundle = false, bool isAndroid = false)
        {

            bool isCanApk = true;

            #region Download and update the Keystore
            if (!IsConfigKeystore())
            {
                isCanApk = false;
            }
            #endregion

            if (isBuildAppBundle && PlayerSettings.applicationIdentifier.Contains("com.test"))
            {
                if (EditorUtility.DisplayDialog("Please double check the package name", PlayerSettings.applicationIdentifier, "OK", "OK")){}
                return;
            }

            if (isDev) MConfigUtils.AddMacro(MSymbol.DEBUG);
            else MConfigUtils.RemoveMacro(MSymbol.DEBUG);

            bool isSc =  BuildInternal(EditorUserBuildSettings.activeBuildTarget);
            if (!isSc) return;

            AssetDatabase.Refresh();
            await Task.Delay(100);
            AssetDatabase.Refresh();

            if (isCanApk)
            {
                Build(isDev, isBuildAppBundle, isAndroid);
            }

        }


        private static bool BuildInternal(BuildTarget buildTarget)
        {
            Debug.Log($"Start Building : {buildTarget}");
            var buildoutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
            var streamingAssetsRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
            long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            BuiltinBuildParameters buildParameters = new BuiltinBuildParameters();
            buildParameters.BuildOutputRoot = buildoutputRoot;
            buildParameters.BuildinFileRoot = streamingAssetsRoot;
            buildParameters.BuildPipeline = EBuildPipeline.BuiltinBuildPipeline.ToString();
            buildParameters.BuildTarget = buildTarget;
            buildParameters.BuildMode = EBuildMode.ForceRebuild;
            buildParameters.PackageName = "DefaultPackage";
            buildParameters.PackageVersion = timestamp.ToString();
            buildParameters.VerifyBuildingResult = true;
            buildParameters.EnableSharePackRule = true;
            buildParameters.FileNameStyle = EFileNameStyle.HashName;
            buildParameters.BuildinFileCopyOption = EBuildinFileCopyOption.ClearAndCopyAll;
            buildParameters.BuildinFileCopyParams = string.Empty;
            buildParameters.EncryptionServices = CreateEncryptionInstance();
            buildParameters.CompressOption = ECompressOption.LZ4;
            BuiltinBuildPipeline pipeline = new BuiltinBuildPipeline();
            var buildResult = pipeline.Run(buildParameters, true);
            if (buildResult.Success)
            {
                Debug.Log($"Successful Building : {buildResult.OutputPackageDirectory}");
            }
            else
            {
                Debug.LogError($"Build failure : {buildResult.ErrorInfo}");
            }
            return buildResult.Success;
        }

        private static YooAsset.IEncryptionServices CreateEncryptionInstance()
        {
            return new FileOffsetEncryption();
        }

        /// <summary>
        /// File offset encryption mode
        /// </summary>
        public class FileOffsetEncryption : IEncryptionServices
        {
            public EncryptResult Encrypt(EncryptFileInfo fileInfo)
            {
                int offset = (int)FileOffsetDecryption.GetFileOffset();
                byte[] fileData = File.ReadAllBytes(fileInfo.FilePath);
                var encryptedData = new byte[fileData.Length + offset];
                Buffer.BlockCopy(fileData, 0, encryptedData, offset, fileData.Length);
                EncryptResult result = new EncryptResult();
                result.Encrypted = true;
                result.EncryptedData = encryptedData;
                return result;
            }
        }

        #region Package check check information

        private bool IsConfigKeystore()
        {
            if (!PlayerSettings.Android.useCustomKeystore ||
               string.IsNullOrEmpty(PlayerSettings.Android.keystoreName) ||
               string.IsNullOrEmpty(PlayerSettings.Android.keystorePass) ||
               string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName) ||
               string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass))
            {
                return ConfigKeystore();
            }
            return true;
        }

        #endregion

        #region Build

        private void Build(bool isDevBuild, bool isBuildAppBundle, bool isAndroiud)
        {
            AssetDatabase.Refresh();
            if (BuildPipeline.isBuildingPlayer) return;
            string apkName = GetBuildPath(isDevBuild, isBuildAppBundle, isAndroiud);
            if (EditorUtility.DisplayDialog("Start Packing", apkName, "verify", "Cancel"))
            {
                AssetDatabase.Refresh();
                BuildTarget target = BuildTarget.Android;
                string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
                EditorUserBuildSettings.buildAppBundle = isBuildAppBundle;
                EditorUserBuildSettings.exportAsGoogleAndroidProject = isAndroiud;
                BuildPipeline.BuildPlayer(scenes, apkName, target, BuildOptions.None);
            }

        }

        private static string GetBuildPath(bool isDevBuild, bool isBuildAppBundle, bool isAndroid)
        {
            string path = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Build"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            if (isAndroid) return Path.Combine(path, "AndroidPro" + PlayerSettings.bundleVersion + timestamp);

            if (isBuildAppBundle) return Path.Combine(path, "AAB" + PlayerSettings.bundleVersion + "(" + PlayerSettings.Android.bundleVersionCode + ")" + "-" + timestamp + "-" + "Release.aab");
            else return Path.Combine(path, "APK" + PlayerSettings.bundleVersion + "(" + PlayerSettings.Android.bundleVersionCode + ")" + "-" + timestamp + "-" + (isDevBuild ? "Debug" : "Release") + ".apk");
        }

        #endregion

        #endregion

        #region Keystore setting
        /// <summary>
        /// Keystore setting
        /// </summary>
        /// <returns></returns>
        public bool ConfigKeystore()
        {
            string keyPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), keyStorePath);
            string keyInfoPath = keyPath.Substring(0, keyPath.Length - 8) + "json";
            if (!File.Exists(keyPath) || !File.Exists(keyInfoPath))
            {
                EditorUtility.DisplayDialog("Set the Keystore as required", $"Keystore path : ${keyPath}\n$\"Keystore Info Path : ${keyInfoPath}", "OK");
                return false;
            }
            string keyStoreInfo = File.ReadAllText(keyInfoPath);
            if (string.IsNullOrEmpty(keyStoreInfo))
            {
                return false;
            }
            if (!PlayerSettings.Android.useCustomKeystore ||
                string.IsNullOrEmpty(PlayerSettings.Android.keystoreName) ||
                string.IsNullOrEmpty(PlayerSettings.Android.keystorePass) ||
                string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName) ||
                string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass))
            {
                ConfigKeystore configData = keyStoreInfo.FromJson<ConfigKeystore>();
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = Path.Combine(Path.GetDirectoryName(Application.dataPath), configData.store_path);
                PlayerSettings.Android.keystorePass = configData.key_password;
                PlayerSettings.Android.keyaliasName = configData.key_alias;
                PlayerSettings.Android.keyaliasPass = configData.store_password;
                AssetDatabase.SaveAssets();
                EditorUtility.DisplayDialog("Signature configuration succeeded", $"Keystore path : ${keyPath}", "OK");
            }
            return true;
        }

        private static string keyStorePath
        {
            get
            {
                string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "keyPath.json");
                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }
                return "";
            }
        }
        private static string key;
        private static string key_password;
        private static string key_alias;
        private static string store_password;

        /// <summary>
        /// No server, manual configuration
        /// Generate Json that can be read
        /// </summary>
        private static void AddKeyStore()
        {
            string keyPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), keyStorePath);
            if (!File.Exists(keyPath))
            {
                EditorUtility.DisplayDialog("请按照要求设置密钥", $"密钥位置: ${keyPath}", "OK");
                return;
            }
            string keyInfoPath = keyPath.Substring(0, keyPath.Length - 8) + "json";
            ConfigKeystore configKeystore = new ConfigKeystore();
            configKeystore.store_path = keyStorePath;
            configKeystore.key_password = key_password;
            configKeystore.key_alias = key_alias;
            configKeystore.store_password = store_password;
            string json = configKeystore.ToJson();
            File.WriteAllText(keyInfoPath, json);
            EditorUtility.DisplayDialog("The signature configuration is generated successfully. Procedure", $"Keystore configuration location : ${keyInfoPath}", "OK");
        }

        #endregion

    }
}