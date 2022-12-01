using UnityEngine;

namespace GameSettings.Audio
{
    public static class SoundUtilities
    {
        public static float FloatToDB(float value)
        {
            return 20f * Mathf.Log10(value);
        }

        public static float DBToFloat(float db)
        {
            return Mathf.Pow(10, db / 20f);
        }
    }
}

