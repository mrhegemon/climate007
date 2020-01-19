﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Maestro.Haptics.Vibration
{
    public class MiddleVibrateOnCollideBehaviour : VibrateOnCollideBehaviour
    {
        [DllImport("MaestroAPI")]
        public static extern void set_middle_vibration_effect(IntPtr maestroPtr, byte effect);

        protected override VibrationMutator getVibrationMutator()
        {
            return set_middle_vibration_effect;
        }
    }
}
