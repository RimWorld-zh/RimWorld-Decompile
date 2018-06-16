using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E9 RID: 1769
	public static class SkyfallerDrawPosUtility
	{
		// Token: 0x06002682 RID: 9858 RVA: 0x00149C6C File Offset: 0x0014806C
		public static Vector3 DrawPos_Accelerate(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = Mathf.Pow((float)ticksToImpact, 0.95f) * 1.7f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x00149CA8 File Offset: 0x001480A8
		public static Vector3 DrawPos_ConstantSpeed(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)ticksToImpact * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x00149CD4 File Offset: 0x001480D4
		public static Vector3 DrawPos_Decelerate(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)(ticksToImpact * ticksToImpact) * 0.00721f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x00149D08 File Offset: 0x00148108
		private static Vector3 PosAtDist(Vector3 center, float dist, float angle)
		{
			return center + Vector3Utility.FromAngleFlat(angle - 90f) * dist;
		}
	}
}
