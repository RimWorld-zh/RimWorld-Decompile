using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class StatPart_IsFlesh : StatPart
	{
		[CompilerGenerated]
		private static Func<Pawn, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache1;

		public StatPart_IsFlesh()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num))
			{
				val *= num;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			float num;
			string result;
			if (this.TryGetIsFleshFactor(req, out num) && num != 1f)
			{
				result = "StatsReport_NotFlesh".Translate() + ": x" + num.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		private bool TryGetIsFleshFactor(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => (!x.RaceProps.IsFlesh) ? 0f : 1f, (ThingDef x) => (!x.race.IsFlesh) ? 0f : 1f, out bodySize);
		}

		[CompilerGenerated]
		private static float <TryGetIsFleshFactor>m__0(Pawn x)
		{
			return (!x.RaceProps.IsFlesh) ? 0f : 1f;
		}

		[CompilerGenerated]
		private static float <TryGetIsFleshFactor>m__1(ThingDef x)
		{
			return (!x.race.IsFlesh) ? 0f : 1f;
		}
	}
}
