using UnityEngine;

namespace RimWorld
{
	public static class GameConditionUtility
	{
		public static float LerpInOutValue(float timePassed, float timeLeft, float lerpTime, float lerpTarget = 1)
		{
			float t = (float)((!(timePassed < lerpTime)) ? ((!(timeLeft < lerpTime)) ? 1.0 : (timeLeft / lerpTime)) : (timePassed / lerpTime));
			return Mathf.Lerp(0f, lerpTarget, t);
		}
	}
}
