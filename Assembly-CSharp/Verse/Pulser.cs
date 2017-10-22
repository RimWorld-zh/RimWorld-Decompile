using UnityEngine;

namespace Verse
{
	public static class Pulser
	{
		public static float PulseBrightness(float frequency, float amplitude)
		{
			return Pulser.PulseBrightness(frequency, amplitude, Time.realtimeSinceStartup);
		}

		public static float PulseBrightness(float frequency, float amplitude, float time)
		{
			float num = (float)(time * 6.2831854820251465);
			num *= frequency;
			float t = (float)((1.0 - Mathf.Cos(num)) * 0.5);
			return Mathf.Lerp((float)(1.0 - amplitude), 1f, t);
		}
	}
}
