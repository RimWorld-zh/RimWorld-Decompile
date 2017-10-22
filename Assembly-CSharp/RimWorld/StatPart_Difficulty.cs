using System;
using Verse;

namespace RimWorld
{
	public class StatPart_Difficulty : StatPart
	{
		private float factorRelax = 1f;

		private float factorBasebuilder = 1f;

		private float factorRough = 1f;

		private float factorChallenge = 1f;

		private float factorExtreme = 1f;

		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficulty);
		}

		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficulty).ToStringPercent();
		}

		private float Multiplier(DifficultyDef d)
		{
			switch (d.index)
			{
			case (ushort)0:
			{
				return this.factorRelax;
			}
			case (ushort)1:
			{
				return this.factorBasebuilder;
			}
			case (ushort)2:
			{
				return this.factorRough;
			}
			case (ushort)3:
			{
				return this.factorChallenge;
			}
			case (ushort)4:
			{
				return this.factorExtreme;
			}
			default:
			{
				throw new ArgumentOutOfRangeException();
			}
			}
		}
	}
}
