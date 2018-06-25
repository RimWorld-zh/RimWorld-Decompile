using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AB RID: 2475
	public class StatPart_Glow : StatPart
	{
		// Token: 0x040023AB RID: 9131
		private bool humanlikeOnly = false;

		// Token: 0x040023AC RID: 9132
		private SimpleCurve factorFromGlowCurve = null;

		// Token: 0x06003779 RID: 14201 RVA: 0x001D9C58 File Offset: 0x001D8058
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromGlowCurve == null)
			{
				yield return "factorFromLightCurve is null.";
			}
			yield break;
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x001D9C82 File Offset: 0x001D8082
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val *= this.FactorFromGlow(req.Thing);
			}
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x001D9CB8 File Offset: 0x001D80B8
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

		// Token: 0x0600377C RID: 14204 RVA: 0x001D9D34 File Offset: 0x001D8134
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

		// Token: 0x0600377D RID: 14205 RVA: 0x001D9D80 File Offset: 0x001D8180
		private float GlowLevel(Thing t)
		{
			return t.Map.glowGrid.GameGlowAt(t.Position, false);
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x001D9DAC File Offset: 0x001D81AC
		private float FactorFromGlow(Thing t)
		{
			return this.factorFromGlowCurve.Evaluate(this.GlowLevel(t));
		}
	}
}
