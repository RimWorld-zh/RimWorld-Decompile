using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B2 RID: 1202
	public class InteractionWorker_DeepTalk : InteractionWorker
	{
		// Token: 0x06001578 RID: 5496 RVA: 0x000BECA8 File Offset: 0x000BD0A8
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			float num = 0.075f;
			return num * this.CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
		}

		// Token: 0x04000CAC RID: 3244
		private const float BaseSelectionWeight = 0.075f;

		// Token: 0x04000CAD RID: 3245
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
	}
}
