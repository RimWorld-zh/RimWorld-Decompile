using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A9 RID: 2473
	public class StatPart_Glow : StatPart
	{
		// Token: 0x06003775 RID: 14197 RVA: 0x001D9844 File Offset: 0x001D7C44
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromGlowCurve == null)
			{
				yield return "factorFromLightCurve is null.";
			}
			yield break;
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x001D986E File Offset: 0x001D7C6E
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val *= this.FactorFromGlow(req.Thing);
			}
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x001D98A4 File Offset: 0x001D7CA4
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

		// Token: 0x06003778 RID: 14200 RVA: 0x001D9920 File Offset: 0x001D7D20
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

		// Token: 0x06003779 RID: 14201 RVA: 0x001D996C File Offset: 0x001D7D6C
		private float GlowLevel(Thing t)
		{
			return t.Map.glowGrid.GameGlowAt(t.Position, false);
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x001D9998 File Offset: 0x001D7D98
		private float FactorFromGlow(Thing t)
		{
			return this.factorFromGlowCurve.Evaluate(this.GlowLevel(t));
		}

		// Token: 0x040023A3 RID: 9123
		private bool humanlikeOnly = false;

		// Token: 0x040023A4 RID: 9124
		private SimpleCurve factorFromGlowCurve = null;
	}
}
