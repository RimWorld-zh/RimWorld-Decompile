using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F2A RID: 3882
	public static class EloUtility
	{
		// Token: 0x04003DA0 RID: 15776
		private const float TenFactorRating = 400f;

		// Token: 0x06005CED RID: 23789 RVA: 0x002F207E File Offset: 0x002F047E
		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		// Token: 0x06005CEE RID: 23790 RVA: 0x002F2098 File Offset: 0x002F0498
		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, teamA / 400f) + Mathf.Pow(10f, teamB / 400f);
			return Mathf.Pow(10f, teamA / 400f) / num;
		}

		// Token: 0x06005CEF RID: 23791 RVA: 0x002F20E4 File Offset: 0x002F04E4
		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (teamRating - referenceRating) / 400f);
		}

		// Token: 0x06005CF0 RID: 23792 RVA: 0x002F2110 File Offset: 0x002F0510
		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400f;
		}
	}
}
