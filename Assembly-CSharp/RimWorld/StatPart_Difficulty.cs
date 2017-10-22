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
			float result;
			switch (d.index)
			{
			case (ushort)0:
			{
				result = this.factorRelax;
				break;
			}
			case (ushort)1:
			{
				result = this.factorBasebuilder;
				break;
			}
			case (ushort)2:
			{
				result = this.factorRough;
				break;
			}
			case (ushort)3:
			{
				result = this.factorChallenge;
				break;
			}
			case (ushort)4:
			{
				result = this.factorExtreme;
				break;
			}
			default:
			{
				throw new ArgumentOutOfRangeException();
			}
			}
			return result;
		}
	}
}
