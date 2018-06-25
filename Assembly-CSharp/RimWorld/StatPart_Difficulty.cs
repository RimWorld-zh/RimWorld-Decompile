using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A8 RID: 2472
	public class StatPart_Difficulty : StatPart
	{
		// Token: 0x040023A4 RID: 9124
		private List<float> factorsPerDifficulty = new List<float>();

		// Token: 0x0600376B RID: 14187 RVA: 0x001D9908 File Offset: 0x001D7D08
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficulty);
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x001D9920 File Offset: 0x001D7D20
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficulty).ToStringPercent();
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x001D9960 File Offset: 0x001D7D60
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
