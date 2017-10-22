using UnityEngine;

namespace RimWorld
{
	public static class GameConditionUtility
	{
		public static float LerpInOutValue(GameCondition gameCondition, float lerpTime, float lerpTarget = 1f)
		{
			return (!gameCondition.Permanent) ? GameConditionUtility.LerpInOutValue((float)gameCondition.TicksPassed, (float)gameCondition.TicksLeft, lerpTime, lerpTarget) : GameConditionUtility.LerpInOutValue((float)gameCondition.TicksPassed, (float)(lerpTime + 1.0), lerpTime, lerpTarget);
		}

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
