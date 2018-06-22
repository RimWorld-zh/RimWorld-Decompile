using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C6 RID: 2502
	public class StatWorker_ShootingAccuracy : StatWorker
	{
		// Token: 0x0600381C RID: 14364 RVA: 0x001DE910 File Offset: 0x001DCD10
		public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetExplanationFinalizePart(req, numberSense, finalVal));
			stringBuilder.AppendLine();
			for (int i = 5; i <= 45; i += 5)
			{
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					"distance".Translate().CapitalizeFirst(),
					" ",
					i.ToString(),
					": ",
					Mathf.Pow(finalVal, (float)i).ToStringPercent()
				}));
			}
			return stringBuilder.ToString();
		}
	}
}
