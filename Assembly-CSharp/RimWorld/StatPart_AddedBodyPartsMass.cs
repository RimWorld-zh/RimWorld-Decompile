using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StatPart_AddedBodyPartsMass : StatPart
	{
		private const float AddedBodyPartMassFactor = 0.9f;

		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = default(float);
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			float num = default(float);
			if (this.TryGetValue(req, out num) && num != 0.0)
			{
				return "StatsReport_AddedBodyPartsMass".Translate() + ": " + num.ToStringMassOffset();
			}
			return null;
		}

		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Func<Pawn, float>)((Pawn x) => this.GetAddedBodyPartsMass(x)), (Func<ThingDef, float>)((ThingDef x) => 0f), out value);
		}

		private float GetAddedBodyPartsMass(Pawn p)
		{
			float num = 0f;
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_AddedPart hediff_AddedPart = hediffs[i] as Hediff_AddedPart;
				if (hediff_AddedPart != null && hediff_AddedPart.def.spawnThingOnRemoved != null)
				{
					num = (float)(num + hediff_AddedPart.def.spawnThingOnRemoved.GetStatValueAbstract(StatDefOf.Mass, null) * 0.89999997615814209);
				}
			}
			return num;
		}
	}
}
