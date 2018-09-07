using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class StatPart_NaturalNotMissingBodyPartsCoverage : StatPart
	{
		[CompilerGenerated]
		private static Func<Pawn, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache1;

		public StatPart_NaturalNotMissingBodyPartsCoverage()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val *= num;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetValue(req, out f))
			{
				return "StatsReport_MissingBodyParts".Translate() + ": x" + f.ToStringPercent();
			}
			return null;
		}

		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x.RaceProps.body.corePart), (ThingDef x) => 1f, out value);
		}

		[CompilerGenerated]
		private static float <TryGetValue>m__0(Pawn x)
		{
			return x.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x.RaceProps.body.corePart);
		}

		[CompilerGenerated]
		private static float <TryGetValue>m__1(ThingDef x)
		{
			return 1f;
		}
	}
}
