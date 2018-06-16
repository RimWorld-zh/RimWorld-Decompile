using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A3 RID: 2467
	public class StatPart_AddedBodyPartsMass : StatPart
	{
		// Token: 0x0600374A RID: 14154 RVA: 0x001D8864 File Offset: 0x001D6C64
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x001D8888 File Offset: 0x001D6C88
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			string result;
			if (this.TryGetValue(req, out num) && num != 0f)
			{
				result = "StatsReport_AddedBodyPartsMass".Translate() + ": " + num.ToStringMassOffset();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x001D88D8 File Offset: 0x001D6CD8
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => this.GetAddedBodyPartsMass(x), (ThingDef x) => 0f, out value);
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x001D8920 File Offset: 0x001D6D20
		private float GetAddedBodyPartsMass(Pawn p)
		{
			float num = 0f;
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_AddedPart hediff_AddedPart = hediffs[i] as Hediff_AddedPart;
				if (hediff_AddedPart != null && hediff_AddedPart.def.spawnThingOnRemoved != null)
				{
					num += hediff_AddedPart.def.spawnThingOnRemoved.GetStatValueAbstract(StatDefOf.Mass, null) * 0.9f;
				}
			}
			return num;
		}

		// Token: 0x04002395 RID: 9109
		private const float AddedBodyPartMassFactor = 0.9f;
	}
}
