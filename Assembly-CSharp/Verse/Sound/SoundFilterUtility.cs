using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB5 RID: 3509
	public static class SoundFilterUtility
	{
		// Token: 0x06004E59 RID: 20057 RVA: 0x0028EC5D File Offset: 0x0028D05D
		public static void DisableAllFiltersOn(AudioSource source)
		{
			SoundFilterUtility.DisableFilterOn<AudioLowPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioHighPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioEchoFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioReverbFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioDistortionFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioChorusFilter>(source);
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x0028EC84 File Offset: 0x0028D084
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
