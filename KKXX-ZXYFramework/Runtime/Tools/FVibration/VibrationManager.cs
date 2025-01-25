using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 震动控制模块
    /// </summary>
    public  class VibrationManager
    {

        /// <summary>
        /// 震动开关
        /// </summary>
        public static bool ISVibrate
        {
            get
            {
                return PPData.GetBool("Game_VibrationManager_ISVibrate", true);
            }
            set
            {
                PPData.SetBool("Game_VibrationManager_ISVibrate", value);
            }
        }

        /// <summary>
        /// 触发一次震动
        /// </summary>
        /// <param name="milliseconds">持续时间</param>
        public static void Vibrate(long milliseconds)
        {
            if (!ISVibrate) return;
            Vibration.Vibrate(milliseconds);
        }
        /// <summary>
        /// 一次弹板的小行震动
        /// </summary>
        public static void VibratePop()
        {
            if (!ISVibrate) return;
            Vibration.VibratePop();
        }
        /// <summary>
        /// 一次蜜蜂行震动
        /// </summary>
        public static void VibratePeek()
        {
            if (!ISVibrate) return;
            Vibration.VibratePeek();
        }
        /// <summary>
        /// 一次提示性震动
        /// </summary>
        public static void VibrateNope()
        {
            if (!ISVibrate) return;
            Vibration.VibrateNope();
        }

    }
}