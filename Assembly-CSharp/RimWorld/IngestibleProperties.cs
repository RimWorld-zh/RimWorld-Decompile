using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200026C RID: 620
	public class IngestibleProperties
	{
		// Token: 0x04000500 RID: 1280
		[Unsaved]
		public ThingDef parent;

		// Token: 0x04000501 RID: 1281
		public int maxNumToIngestAtOnce = 20;

		// Token: 0x04000502 RID: 1282
		public List<IngestionOutcomeDoer> outcomeDoers = null;

		// Token: 0x04000503 RID: 1283
		public int baseIngestTicks = 500;

		// Token: 0x04000504 RID: 1284
		public float chairSearchRadius = 32f;

		// Token: 0x04000505 RID: 1285
		public bool useEatingSpeedStat = true;

		// Token: 0x04000506 RID: 1286
		public ThoughtDef tasteThought = null;

		// Token: 0x04000507 RID: 1287
		public ThoughtDef specialThoughtDirect = null;

		// Token: 0x04000508 RID: 1288
		public ThoughtDef specialThoughtAsIngredient = null;

		// Token: 0x04000509 RID: 1289
		public EffecterDef ingestEffect = null;

		// Token: 0x0400050A RID: 1290
		public EffecterDef ingestEffectEat = null;

		// Token: 0x0400050B RID: 1291
		public SoundDef ingestSound = null;

		// Token: 0x0400050C RID: 1292
		[MustTranslate]
		public string ingestCommandString = null;

		// Token: 0x0400050D RID: 1293
		[MustTranslate]
		public string ingestReportString = null;

		// Token: 0x0400050E RID: 1294
		[MustTranslate]
		public string ingestReportStringEat = null;

		// Token: 0x0400050F RID: 1295
		public HoldOffsetSet ingestHoldOffsetStanding = null;

		// Token: 0x04000510 RID: 1296
		public bool ingestHoldUsesTable = true;

		// Token: 0x04000511 RID: 1297
		public FoodTypeFlags foodType = FoodTypeFlags.None;

		// Token: 0x04000512 RID: 1298
		public float joy = 0f;

		// Token: 0x04000513 RID: 1299
		public JoyKindDef joyKind = null;

		// Token: 0x04000514 RID: 1300
		public ThingDef sourceDef;

		// Token: 0x04000515 RID: 1301
		public FoodPreferability preferability = FoodPreferability.Undefined;

		// Token: 0x04000516 RID: 1302
		public bool nurseable = false;

		// Token: 0x04000517 RID: 1303
		public float optimalityOffsetHumanlikes = 0f;

		// Token: 0x04000518 RID: 1304
		public float optimalityOffsetFeedingAnimals = 0f;

		// Token: 0x04000519 RID: 1305
		public DrugCategory drugCategory = DrugCategory.None;

		// Token: 0x0400051A RID: 1306
		[Unsaved]
		private float cachedNutrition = -1f;

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0006070C File Offset: 0x0005EB0C
		public JoyKindDef JoyKind
		{
			get
			{
				return (this.joyKind == null) ? JoyKindDefOf.Gluttonous : this.joyKind;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x0006073C File Offset: 0x0005EB3C
		public bool HumanEdible
		{
			get
			{
				return (FoodTypeFlags.OmnivoreHuman & this.foodType) != FoodTypeFlags.None;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x00060764 File Offset: 0x0005EB64
		public bool IsMeal
		{
			get
			{
				return this.preferability >= FoodPreferability.MealAwful && this.preferability <= FoodPreferability.MealLavish;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x00060798 File Offset: 0x0005EB98
		public float CachedNutrition
		{
			get
			{
				if (this.cachedNutrition == -1f)
				{
					this.cachedNutrition = this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null);
				}
				return this.cachedNutrition;
			}
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x000607DC File Offset: 0x0005EBDC
		public IEnumerable<string> ConfigErrors()
		{
			if (this.preferability == FoodPreferability.Undefined)
			{
				yield return "undefined preferability";
			}
			if (this.foodType == FoodTypeFlags.None)
			{
				yield return "no foodType";
			}
			if (this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null) == 0f && this.preferability != FoodPreferability.NeverForNutrition)
			{
				yield return string.Concat(new object[]
				{
					"Nutrition == 0 but preferability is ",
					this.preferability,
					" instead of ",
					FoodPreferability.NeverForNutrition
				});
			}
			if (!this.parent.IsCorpse && this.preferability > FoodPreferability.DesperateOnlyForHumanlikes && !this.parent.socialPropernessMatters && this.parent.EverHaulable)
			{
				yield return "ingestible preferability > DesperateOnlyForHumanlikes but socialPropernessMatters=false. This will cause bugs wherein wardens will look in prison cells for food to give to prisoners and so will repeatedly pick up and drop food inside the cell.";
			}
			if (this.joy > 0f && this.joyKind == null)
			{
				yield return "joy > 0 with no joy kind";
			}
			yield break;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x00060808 File Offset: 0x0005EC08
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.joy > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Joy".Translate(), this.joy.ToStringPercent("F2") + " (" + this.JoyKind.label + ")", 0, "");
			}
			if (this.outcomeDoers != null)
			{
				for (int i = 0; i < this.outcomeDoers.Count; i++)
				{
					foreach (StatDrawEntry s in this.outcomeDoers[i].SpecialDisplayStats(this.parent))
					{
						yield return s;
					}
				}
			}
			yield break;
		}
	}
}
