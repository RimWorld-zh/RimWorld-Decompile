using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200030B RID: 779
	public static class GameConditionUtility
	{
		// Token: 0x06000D07 RID: 3335 RVA: 0x0007177C File Offset: 0x0006FB7C
		public static float LerpInOutValue(GameCondition gameCondition, float lerpTime, float lerpTarget = 1f)
		{
			float result;
			if (gameCondition.Permanent)
			{
				result = GameConditionUtility.LerpInOutValue((float)gameCondition.TicksPassed, lerpTime + 1f, lerpTime, lerpTarget);
			}
			else
			{
				result = GameConditionUtility.LerpInOutValue((float)gameCondition.TicksPassed, (float)gameCondition.TicksLeft, lerpTime, lerpTarget);
			}
			return result;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x000717CC File Offset: 0x0006FBCC
		public static float LerpInOutValue(float timePassed, float timeLeft, float lerpTime, float lerpTarget = 1f)
		{
			float num = 1f;
			if (timePassed < lerpTime)
			{
				num = timePassed / lerpTime;
			}
			if (timeLeft < lerpTime)
			{
				num = Mathf.Min(num, timeLeft / lerpTime);
			}
			return Mathf.Lerp(0f, lerpTarget, num);
		}
	}
}
