using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000309 RID: 777
	public static class GameConditionUtility
	{
		// Token: 0x06000D04 RID: 3332 RVA: 0x00071570 File Offset: 0x0006F970
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

		// Token: 0x06000D05 RID: 3333 RVA: 0x000715C0 File Offset: 0x0006F9C0
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
