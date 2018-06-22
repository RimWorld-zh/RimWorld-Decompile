using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9F RID: 3743
	public static class Pulser
	{
		// Token: 0x0600586A RID: 22634 RVA: 0x002D5168 File Offset: 0x002D3568
		public static float PulseBrightness(float frequency, float amplitude)
		{
			return Pulser.PulseBrightness(frequency, amplitude, Time.realtimeSinceStartup);
		}

		// Token: 0x0600586B RID: 22635 RVA: 0x002D518C File Offset: 0x002D358C
		public static float PulseBrightness(float frequency, float amplitude, float time)
		{
			float num = time * 6.28318548f;
			num *= frequency;
			float t = (1f - Mathf.Cos(num)) * 0.5f;
			return Mathf.Lerp(1f - amplitude, 1f, t);
		}
	}
}
