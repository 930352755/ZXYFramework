using UnityEngine;
using System.IO;
using UnityEngine.Android;

namespace Game
{

    /// <summary>
    /// 保存到安卓手机相册
    /// </summary>
    public static class SaveToDCIM
    {
        /// <summary>
        /// 将一张图片保存到安卓手机相册
        /// </summary>
        /// <param name="imageToSave"></param>
        public static void SaveImageToDCIM(Texture2D imageToSave)
        {
            GetUserAuthorize();

            // 检查 Android 平台
            if (Application.platform != RuntimePlatform.Android)
            {
                Debug.LogError("此功能仅适用于 Android 平台。");
                return;
            }

            if (imageToSave == null)
            {
                Debug.LogError("Image is null!");
                return;
            }

            // 获取 DCIM 文件夹路径
            string dcimPath = GetDCIMPath();
            if (string.IsNullOrEmpty(dcimPath))
            {
                Debug.LogError("Failed to get DCIM directory path.");
                return;
            }

            // 确保目标文件夹存在
            string folderPath = Path.Combine(dcimPath, "WaterSort");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 生成文件路径
            string fileName = $"ImageZXY_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
            string filePath = Path.Combine(folderPath, fileName);

            // 将 Texture2D 转换为 PNG 格式并保存
            byte[] imageBytes = imageToSave.EncodeToPNG();
            File.WriteAllBytes(filePath, imageBytes);

            Debug.Log($"Image saved to: {filePath}");

            // 通知图库刷新文件
            UpdateGallery(filePath);
        }
        private static string GetDCIMPath()
        {
            try
            {
                // 调用 Android 的 Environment 类获取 DCIM 路径
                AndroidJavaClass environment = new AndroidJavaClass("android.os.Environment");
                AndroidJavaObject dcimDirectory = environment.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", "DCIM");
                return dcimDirectory.Call<string>("getAbsolutePath");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error getting DCIM path: {ex.Message}");
                return null;
            }
        }
        private static void UpdateGallery(string filePath)
        {
            try
            {
                // 通知安卓系统刷新媒体库
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                AndroidJavaClass mediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection");
                AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

                mediaScannerConnection.CallStatic("scanFile", context, new string[] { filePath }, null, null);

                Debug.Log("Gallery updated.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error updating gallery: {ex.Message}");
            }
        }
        private static void GetUserAuthorize()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }
        }

    }

}