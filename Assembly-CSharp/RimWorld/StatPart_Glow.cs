using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AD RID: 2477
	public class StatPart_Glow : StatPart
	{
		// Token: 0x0600377A RID: 14202 RVA: 0x001D95AC File Offset: 0x001D79AC
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromGlowCurve == null)
			{
				yield return "factorFromLightCurve is null.";
			}
			yield break;
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x001D95D6 File Offset: 0x001D79D6
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val *= this.FactorFromGlow(req.Thing);
			}
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x001D960C File Offset: 0x001D7A0C
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

		// Token: 0x0600377D RID: 14205 RVA: 0x001D9688 File Offset: 0x001D7A88
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

		// Token: 0x0600377E RID: 14206 RVA: 0x001D96D4 File Offset: 0x001D7AD4
		private float GlowLevel(Thing t)
		{
			return t.Map.glowGrid.GameGlowAt(t.Position, false);
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x001D9700 File Offset: 0x001D7B00
		private float FactorFromGlow(Thing t)
		{
			return this.factorFromGlowCurve.Evaluate(this.GlowLevel(t));
		}

		// Token: 0x040023A9 RID: 9129
		private bool humanlikeOnly = false;

		// Token: 0x040023AA RID: 9130
		private SimpleCurve factorFromGlowCurve = null;
	}
}
