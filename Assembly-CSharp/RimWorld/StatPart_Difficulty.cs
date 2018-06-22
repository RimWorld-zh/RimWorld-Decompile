using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A6 RID: 2470
	public class StatPart_Difficulty : StatPart
	{
		// Token: 0x06003767 RID: 14183 RVA: 0x001D94F4 File Offset: 0x001D78F4
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficulty);
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x001D950C File Offset: 0x001D790C
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficulty).ToStringPercent();
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x001D954C File Offset: 0x001D794C
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

		// Token: 0x0400239C RID: 9116
		private List<float> factorsPerDifficulty = new List<float>();
	}
}
