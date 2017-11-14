using UnityEngine;

namespace Verse
{
	public static class EloUtility
	{
		private const float TenFactorRating = 400f;

		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, (float)(teamA / 400.0)) + Mathf.Pow(10f, (float)(teamB / 400.0));
			return Mathf.Pow(10f, (float)(teamA / 400.0)) / num;
		}

		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (float)((teamRating - referenceRating) / 400.0));
		}

		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return (float)(referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400.0);
		}
	}
}
