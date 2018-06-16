using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DAE RID: 3502
	public static class AudioSourceUtility
	{
		// Token: 0x06004E1A RID: 19994 RVA: 0x0028D4A0 File Offset: 0x0028B8A0
		public static float GetSanitizedVolume(float volume, object debugInfo)
		{
			float result;
			if (float.IsNegativeInfinity(volume))
			{
				Log.ErrorOnce("Volume was negative infinity (" + debugInfo + ")", 863653423, false);
				result = 0f;
			}
			else if (float.IsPositiveInfinity(volume))
			{
				Log.ErrorOnce("Volume was positive infinity (" + debugInfo + ")", 954354323, false);
				result = 1f;
			}
			else if (float.IsNaN(volume))
			{
				Log.ErrorOnce("Volume was NaN (" + debugInfo + ")", 231846572, false);
				result = 1f;
			}
			else
			{
				result = Mathf.Clamp(volume, 0f, 1000f);
			}
			return result;
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x0028D55C File Offset: 0x0028B95C
		public static float GetSanitizedPitch(float pitch, object debugInfo)
		{
			float result;
			if (float.IsNegativeInfinity(pitch))
			{
				Log.ErrorOnce("Pitch was negative infinity (" + debugInfo + ")", 546475990, false);
				result = 0.0001f;
			}
			else if (float.IsPositiveInfinity(pitch))
			{
				Log.ErrorOnce("Pitch was positive infinity (" + debugInfo + ")", 309856435, false);
				result = 1f;
			}
			else if (float.IsNaN(pitch))
			{
				Log.ErrorOnce("Pitch was NaN (" + debugInfo + ")", 800635427, false);
				result = 1f;
			}
			else if (pitch < 0f)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Pitch was negative ",
					pitch,
					" (",
					debugInfo,
					")"
				}), 384765707, false);
				result = 0.0001f;
			}
			else
			{
				result = Mathf.Clamp(pitch, 0.0001f, 1000f);
			}
			return result;
		}
	}
}
