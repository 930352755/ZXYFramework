using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public  class VibrationManager
    {


        private static VibrationManager instance = null;
        public static VibrationManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new VibrationManager();
                }
                return instance;
            }
        }


        private VibrationManager()
        {
            Vibration.Init();
        }


        public static bool ISVibrate
        {
            get
            {
                return QuickData.GetBool("Game_VibrationManager_ISVibrate", true);
            }
            set
            {
                QuickData.SetBool("Game_VibrationManager_ISVibrate", value);
            }
        }

        public void Vibrate(long milliseconds)
        {
            if (!ISVibrate) return;
            Vibration.Vibrate(milliseconds);
        }
        public void VibratePop()
        {
            if (!ISVibrate) return;
            Vibration.VibratePop();
        }
        public void VibratePeek()
        {
            if (!ISVibrate) return;
            Vibration.VibratePeek();
        }
        public void VibrateNope()
        {
            if (!ISVibrate) return;
            Vibration.VibrateNope();
        }

    }
}