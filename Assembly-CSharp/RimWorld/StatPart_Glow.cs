using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StatPart_Glow : StatPart
	{
		private bool humanlikeOnly = false;

		private SimpleCurve factorFromGlowCurve = null;

		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromGlowCurve != null)
				yield break;
			yield return "factorFromLightCurve is null.";
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val *= this.FactorFromGlow(req.Thing);
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			return (!req.HasThing || !this.ActiveFor(req.Thing)) ? null : ("StatsReport_LightMultiplier".Translate(this.GlowLevel(req.Thing).ToStringPercent()) + ": x" + this.FactorFromGlow(req.Thing).ToStringPercent());
		}

		private bool ActiveFor(Thing t)
		{
			bool result;
			if (this.humanlikeOnly)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && !pawn.RaceProps.Humanlike)
				{
					result = false;
					goto IL_003e;
				}
			}
			result = t.Spawned;
			goto IL_003e;
			IL_003e:
			return result;
		}

		private float GlowLevel(Thing t)
		{
			return t.Map.glowGrid.GameGlowAt(t.Position, false);
		}

		private float FactorFromGlow(Thing t)
		{
			return this.factorFromGlowCurve.Evaluate(this.GlowLevel(t));
		}
	}
}
