using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class SkyfallerDrawPosUtility
	{
		public static Vector3 DrawPos_Accelerate(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)(Mathf.Pow((float)ticksToImpact, 0.95f) * 1.7000000476837158 * speed);
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		public static Vector3 DrawPos_ConstantSpeed(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)ticksToImpact * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		public static Vector3 DrawPos_Decelerate(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)((float)(ticksToImpact * ticksToImpact) * 0.0072099999524652958 * speed);
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		private static Vector3 PosAtDist(Vector3 center, float dist, float angle)
		{
			return center + Vector3Utility.FromAngleFlat((float)(angle - 90.0)) * dist;
		}
	}
}
