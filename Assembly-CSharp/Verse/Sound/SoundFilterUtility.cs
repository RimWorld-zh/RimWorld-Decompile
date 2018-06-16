using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB6 RID: 3510
	public static class SoundFilterUtility
	{
		// Token: 0x06004E5B RID: 20059 RVA: 0x0028EC7D File Offset: 0x0028D07D
		public static void DisableAllFiltersOn(AudioSource source)
		{
			SoundFilterUtility.DisableFilterOn<AudioLowPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioHighPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioEchoFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioReverbFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioDistortionFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioChorusFilter>(source);
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x0028ECA4 File Offset: 0x0028D0A4
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
