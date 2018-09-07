using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class StatPart_BodySize : StatPart
	{
		[CompilerGenerated]
		private static Func<Pawn, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache1;

		public StatPart_BodySize()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetBodySize(req, out f))
			{
				return "StatsReport_BodySize".Translate(new object[]
				{
					f.ToString("F2")
				}) + ": x" + f.ToStringPercent();
			}
			return null;
		}

		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.BodySize, (ThingDef x) => x.race.baseBodySize, out bodySize);
		}

		[CompilerGenerated]
		private static float <TryGetBodySize>m__0(Pawn x)
		{
			return x.BodySize;
		}

		[CompilerGenerated]
		private static float <TryGetBodySize>m__1(ThingDef x)
		{
			return x.race.baseBodySize;
		}
	}
}
