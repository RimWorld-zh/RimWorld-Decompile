using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A8 RID: 2472
	public class StatPart_Difficulty : StatPart
	{
		// Token: 0x0400239D RID: 9117
		private List<float> factorsPerDifficulty = new List<float>();

		// Token: 0x0600376B RID: 14187 RVA: 0x001D9634 File Offset: 0x001D7A34
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficulty);
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x001D964C File Offset: 0x001D7A4C
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficulty).ToStringPercent();
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x001D968C File Offset: 0x001D7A8C
		private float Multiplier(DifficultyDef d)
		{
			int num = d.difficulty;
			if (num < 0 || num > this.factorsPerDifficulty.Count - 1)
			{
				Log.ErrorOnce("Not enough difficulty offsets defined for StatPart_Difficulty", 3598689, false);
				num = Mathf.Clamp(num, 0, this.factorsPerDifficulty.Count - 1);
			}
			return this.factorsPerDifficulty[num];
		}
	}
}
