using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class StatPart_AddedBodyPartsMass : StatPart
	{
		private const float AddedBodyPartMassFactor = 0.9f;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache0;

		public StatPart_AddedBodyPartsMass()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

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

		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => this.GetAddedBodyPartsMass(x), (ThingDef x) => 0f, out value);
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
					num += hediff_AddedPart.def.spawnThingOnRemoved.GetStatValueAbstract(StatDefOf.Mass, null) * 0.9f;
				}
			}
			return num;
		}

		[CompilerGenerated]
		private float <TryGetValue>m__0(Pawn x)
		{
			return this.GetAddedBodyPartsMass(x);
		}

		[CompilerGenerated]
		private static float <TryGetValue>m__1(ThingDef x)
		{
			return 0f;
		}
	}
}
