using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F26 RID: 3878
	public static class EloUtility
	{
		// Token: 0x06005CE3 RID: 23779 RVA: 0x002F19FE File Offset: 0x002EFDFE
		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		// Token: 0x06005CE4 RID: 23780 RVA: 0x002F1A18 File Offset: 0x002EFE18
		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, teamA / 400f) + Mathf.Pow(10f, teamB / 400f);
			return Mathf.Pow(10f, teamA / 400f) / num;
		}

		// Token: 0x06005CE5 RID: 23781 RVA: 0x002F1A64 File Offset: 0x002EFE64
		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (teamRating - referenceRating) / 400f);
		}

		// Token: 0x06005CE6 RID: 23782 RVA: 0x002F1A90 File Offset: 0x002EFE90
		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400f;
		}

		// Token: 0x04003D9D RID: 15773
		private const float TenFactorRating = 400f;
	}
}
