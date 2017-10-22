using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class ColorsFromSpectrum
	{
		public static Color Get(IList<Color> spectrum, float val)
		{
			Color result;
			if (spectrum.Count == 0)
			{
				Log.Warning("Color spectrum empty.");
				result = Color.white;
			}
			else if (spectrum.Count == 1)
			{
				result = spectrum[0];
			}
			else
			{
				val = Mathf.Clamp01(val);
				float num = (float)(1.0 / (float)(spectrum.Count - 1));
				int num2 = (int)(val / num);
				if (num2 == spectrum.Count - 1)
				{
					result = spectrum[spectrum.Count - 1];
				}
				else
				{
					float t = (val - (float)num2 * num) / num;
					result = Color.Lerp(spectrum[num2], spectrum[num2 + 1], t);
				}
			}
			return result;
		}
	}
}
