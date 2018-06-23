using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B4 RID: 1204
	public static class NegativeInteractionUtility
	{
		// Token: 0x04000CAF RID: 3247
		public const float AbrasiveSelectionChanceFactor = 2.3f;

		// Token: 0x04000CB0 RID: 3248
		private static readonly SimpleCurve CompatibilityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-2.5f, 4f),
				true
			},
			{
				new CurvePoint(-1.5f, 3f),
				true
			},
			{
				new CurvePoint(-0.5f, 2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.75f),
				true
			},
			{
				new CurvePoint(2f, 0.5f),
				true
			},
			{
				new CurvePoint(3f, 0.4f),
				true
			}
		};

		// Token: 0x04000CB1 RID: 3249
		private static readonly SimpleCurve OpinionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-100f, 6f),
				true
			},
			{
				new CurvePoint(-50f, 4f),
				true
			},
			{
				new CurvePoint(-25f, 2f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(50f, 0.1f),
				true
			},
			{
				new CurvePoint(100f, 0f),
				true
			}
		};

		// Token: 0x0600157B RID: 5499 RVA: 0x000BED0C File Offset: 0x000BD10C
		public static float NegativeInteractionChanceFactor(Pawn initiator, Pawn recipient)
		{
			float result;
			if (initiator.story.traits.HasTrait(TraitDefOf.Kind))
			{
				result = 0f;
			}
			else
			{
				float num = 1f;
				num *= NegativeInteractionUtility.OpinionFactorCurve.Evaluate((float)initiator.relations.OpinionOf(recipient));
				num *= NegativeInteractionUtility.CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
				if (initiator.story.traits.HasTrait(TraitDefOf.Abrasive))
				{
					num *= 2.3f;
				}
				result = num;
			}
			return result;
		}
	}
}
