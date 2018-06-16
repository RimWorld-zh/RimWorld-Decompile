using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F27 RID: 3879
	public static class EloUtility
	{
		// Token: 0x06005CBD RID: 23741 RVA: 0x002EF8F6 File Offset: 0x002EDCF6
		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x002EF910 File Offset: 0x002EDD10
		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, teamA / 400f) + Mathf.Pow(10f, teamB / 400f);
			return Mathf.Pow(10f, teamA / 400f) / num;
		}

		// Token: 0x06005CBF RID: 23743 RVA: 0x002EF95C File Offset: 0x002EDD5C
		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (teamRating - referenceRating) / 400f);
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x002EF988 File Offset: 0x002EDD88
		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400f;
		}

		// Token: 0x04003D8C RID: 15756
		private const float TenFactorRating = 400f;
	}
}
