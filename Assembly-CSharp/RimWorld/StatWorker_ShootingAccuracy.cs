using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StatWorker_ShootingAccuracy : StatWorker
	{
		public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetExplanationFinalizePart(req, numberSense, finalVal));
			stringBuilder.AppendLine();
			for (int num = 5; num <= 45; num += 5)
			{
				stringBuilder.AppendLine("distance".Translate().CapitalizeFirst() + " " + num.ToString() + ": " + Mathf.Pow(finalVal, (float)num).ToStringPercent());
			}
			return stringBuilder.ToString();
		}
	}
}
