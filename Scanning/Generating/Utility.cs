using System.Drawing;
using System.Threading.Tasks;

namespace Generation
{
    class Utility
    {
        /// <summary>
        /// 
        /// </summary>
        public const int BYTES_PER_PIXEL = 4;

        /// <summary>
        /// 
        /// </summary>
        public static Color[] GetColorsFromRGBA(byte[] colorValues)
        {
            Color[] colors = new Color[colorValues.Length/Utility.BYTES_PER_PIXEL];
            int k = 0;
            for (int i = 0; i < colorValues.Length; i += Utility.BYTES_PER_PIXEL)
            {
                colors[k] = Color.FromArgb(colorValues[i + 3], colorValues[i], colorValues[i + 1], colorValues[i + 2]);
                k++;
            }

            return colors;
        }
    }
}
