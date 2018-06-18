using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F26 RID: 3878
	public static class EloUtility
	{
		// Token: 0x06005CBB RID: 23739 RVA: 0x002EF9D2 File Offset: 0x002EDDD2
		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x002EF9EC File Offset: 0x002EDDEC
		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, teamA / 400f) + Mathf.Pow(10f, teamB / 400f);
			return Mathf.Pow(10f, teamA / 400f) / num;
		}

		// Token: 0x06005CBD RID: 23741 RVA: 0x002EFA38 File Offset: 0x002EDE38
		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (teamRating - referenceRating) / 400f);
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x002EFA64 File Offset: 0x002EDE64
		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400f;
		}

		// Token: 0x04003D8B RID: 15755
		private const float TenFactorRating = 400f;
	}
}
