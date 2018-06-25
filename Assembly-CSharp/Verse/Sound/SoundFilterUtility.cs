using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB4 RID: 3508
	public static class SoundFilterUtility
	{
		// Token: 0x06004E72 RID: 20082 RVA: 0x00290339 File Offset: 0x0028E739
		public static void DisableAllFiltersOn(AudioSource source)
		{
			SoundFilterUtility.DisableFilterOn<AudioLowPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioHighPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioEchoFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioReverbFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioDistortionFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioChorusFilter>(source);
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x00290360 File Offset: 0x0028E760
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
