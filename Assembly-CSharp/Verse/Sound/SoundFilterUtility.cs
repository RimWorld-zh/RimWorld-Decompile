using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB2 RID: 3506
	public static class SoundFilterUtility
	{
		// Token: 0x06004E6E RID: 20078 RVA: 0x0029020D File Offset: 0x0028E60D
		public static void DisableAllFiltersOn(AudioSource source)
		{
			SoundFilterUtility.DisableFilterOn<AudioLowPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioHighPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioEchoFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioReverbFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioDistortionFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioChorusFilter>(source);
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x00290234 File Offset: 0x0028E634
		private static void DisableFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T component = source.GetComponent<T>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}
}
