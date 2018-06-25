using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B4 RID: 1204
	public class InteractionWorker_DeepTalk : InteractionWorker
	{
		// Token: 0x04000CAF RID: 3247
		private const float BaseSelectionWeight = 0.075f;

		// Token: 0x04000CB0 RID: 3248
		private SimpleCurve CompatibilityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1.5f, 0f),
				true
			},
			{
				new CurvePoint(-0.5f, 0.1f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.8f),
				true
			},
			{
				new CurvePoint(2f, 3f),
				true
			}
		};

		// Token: 0x0600157B RID: 5499 RVA: 0x000BEFF8 File Offset: 0x000BD3F8
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			float num = 0.075f;
			return num * this.CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
		}
	}
}
