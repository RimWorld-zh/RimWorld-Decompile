using UnityEngine;

namespace Verse.Sound
{
	public static class SoundFilterUtility
	{
		public static void DisableAllFiltersOn(AudioSource source)
		{
			SoundFilterUtility.DisableFilterOn<AudioLowPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioHighPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioEchoFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioReverbFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioDistortionFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioChorusFilter>(source);
		}

		private static void DisableFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T component = ((Component)source).GetComponent<T>();
			if ((Object)(object)component != (Object)null)
			{
				component.enabled = false;
			}
		}
	}
}
