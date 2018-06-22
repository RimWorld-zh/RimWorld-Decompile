using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099F RID: 2463
	public class StatPart_AddedBodyPartsMass : StatPart
	{
		// Token: 0x06003745 RID: 14149 RVA: 0x001D8B34 File Offset: 0x001D6F34
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x001D8B58 File Offset: 0x001D6F58
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

		// Token: 0x06003747 RID: 14151 RVA: 0x001D8BA8 File Offset: 0x001D6FA8
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => this.GetAddedBodyPartsMass(x), (ThingDef x) => 0f, out value);
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x001D8BF0 File Offset: 0x001D6FF0
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

		// Token: 0x04002393 RID: 9107
		private const float AddedBodyPartMassFactor = 0.9f;
	}
}
