using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F1 RID: 1777
	public class ThingSetMaker_Sum : ThingSetMaker
	{
		// Token: 0x060026A6 RID: 9894 RVA: 0x0014A994 File Offset: 0x00148D94
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].chance > 0f && this.options[i].thingSetMaker.CanGenerate(parms))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x0014AA08 File Offset: 0x00148E08
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			this.optionsInRandomOrder.Clear();
			this.optionsInRandomOrder.AddRange(this.options);
			this.optionsInRandomOrder.Shuffle<ThingSetMaker_Sum.Option>();
			for (int i = 0; i < this.optionsInRandomOrder.Count; i++)
			{
				ThingSetMakerParams parms2 = parms;
				IntRange? countRange = parms2.countRange;
				if (countRange != null)
				{
					parms2.countRange = new IntRange?(new IntRange(parms2.countRange.Value.min, parms2.countRange.Value.max - num));
				}
				FloatRange? totalMarketValueRange = parms2.totalMarketValueRange;
				if (totalMarketValueRange != null)
				{
					parms2.totalMarketValueRange = new FloatRange?(new FloatRange(parms2.totalMarketValueRange.Value.min, parms2.totalMarketValueRange.Value.max - num2));
				}
				FloatRange? totalNutritionRange = parms2.totalNutritionRange;
				if (totalNutritionRange != null)
				{
					parms2.totalNutritionRange = new FloatRange?(new FloatRange(parms2.totalNutritionRange.Value.min, parms2.totalNutritionRange.Value.max - num3));
				}
				float? maxTotalMass = parms2.maxTotalMass;
				if (maxTotalMass != null)
				{
					float? maxTotalMass2 = parms2.maxTotalMass;
					parms2.maxTotalMass = ((maxTotalMass2 == null) ? null : new float?(maxTotalMass2.GetValueOrDefault() - num4));
				}
				if (Rand.Chance(this.optionsInRandomOrder[i].chance) && this.optionsInRandomOrder[i].thingSetMaker.CanGenerate(parms2))
				{
					List<Thing> list = this.optionsInRandomOrder[i].thingSetMaker.Generate(parms2);
					num += list.Count;
					for (int j = 0; j < list.Count; j++)
					{
						num2 += list[j].MarketValue * (float)list[j].stackCount;
						if (list[j].def.IsIngestible)
						{
							num3 += list[j].GetStatValue(StatDefOf.Nutrition, true) * (float)list[j].stackCount;
						}
						if (!(list[j] is Pawn))
						{
							num4 += list[j].GetStatValue(StatDefOf.Mass, true) * (float)list[j].stackCount;
						}
					}
					outThings.AddRange(list);
				}
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x0014ACDC File Offset: 0x001490DC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x0014AD24 File Offset: 0x00149124
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].chance > 0f)
				{
					foreach (ThingDef t in this.options[i].thingSetMaker.AllGeneratableThingsDebug(parms))
					{
						yield return t;
					}
				}
			}
			yield break;
		}

		// Token: 0x04001582 RID: 5506
		public List<ThingSetMaker_Sum.Option> options;

		// Token: 0x04001583 RID: 5507
		private List<ThingSetMaker_Sum.Option> optionsInRandomOrder = new List<ThingSetMaker_Sum.Option>();

		// Token: 0x020006F2 RID: 1778
		public class Option
		{
			// Token: 0x04001584 RID: 5508
			public ThingSetMaker thingSetMaker;

			// Token: 0x04001585 RID: 5509
			public float chance = 1f;
		}
	}
}
