using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E7 RID: 1767
	public static class SkyfallerDrawPosUtility
	{
		// Token: 0x0600267F RID: 9855 RVA: 0x0014A238 File Offset: 0x00148638
		public static Vector3 DrawPos_Accelerate(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = Mathf.Pow((float)ticksToImpact, 0.95f) * 1.7f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x0014A274 File Offset: 0x00148674
		public static Vector3 DrawPos_ConstantSpeed(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)ticksToImpact * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x0014A2A0 File Offset: 0x001486A0
		public static Vector3 DrawPos_Decelerate(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)(ticksToImpact * ticksToImpact) * 0.00721f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x0014A2D4 File Offset: 0x001486D4
		private static Vector3 PosAtDist(Vector3 center, float dist, float angle)
		{
			return center + Vector3Utility.FromAngleFlat(angle - 90f) * dist;
		}
	}
}
