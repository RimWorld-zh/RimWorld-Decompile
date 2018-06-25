using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AB RID: 2475
	public class StatPart_Glow : StatPart
	{
		// Token: 0x040023A4 RID: 9124
		private bool humanlikeOnly = false;

		// Token: 0x040023A5 RID: 9125
		private SimpleCurve factorFromGlowCurve = null;

		// Token: 0x06003779 RID: 14201 RVA: 0x001D9984 File Offset: 0x001D7D84
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromGlowCurve == null)
			{
				yield return "factorFromLightCurve is null.";
			}
			yield break;
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x001D99AE File Offset: 0x001D7DAE
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val *= this.FactorFromGlow(req.Thing);
			}
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x001D99E4 File Offset: 0x001D7DE4
		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				result = "StatsReport_LightMultiplier".Translate(new object[]
				{
					this.GlowLevel(req.Thing).ToStringPercent()
				}) + ": x" + this.FactorFromGlow(req.Thing).ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x001D9A60 File Offset: 0x001D7E60
		private bool ActiveFor(Thing t)
		{
			if (this.humanlikeOnly)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && !pawn.RaceProps.Humanlike)
				{
					return false;
				}
			}
			return t.Spawned;
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x001D9AAC File Offset: 0x001D7EAC
		private float GlowLevel(Thing t)
		{
			return t.Map.glowGrid.GameGlowAt(t.Position, false);
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x001D9AD8 File Offset: 0x001D7ED8
		private float FactorFromGlow(Thing t)
		{
			return this.factorFromGlowCurve.Evaluate(this.GlowLevel(t));
		}
	}
}
