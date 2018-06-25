using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A1 RID: 2465
	public class StatPart_AddedBodyPartsMass : StatPart
	{
		// Token: 0x0400239B RID: 9115
		private const float AddedBodyPartMassFactor = 0.9f;

		// Token: 0x06003749 RID: 14153 RVA: 0x001D8F48 File Offset: 0x001D7348
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x001D8F6C File Offset: 0x001D736C
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

		// Token: 0x0600374B RID: 14155 RVA: 0x001D8FBC File Offset: 0x001D73BC
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => this.GetAddedBodyPartsMass(x), (ThingDef x) => 0f, out value);
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x001D9004 File Offset: 0x001D7404
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
	}
}
