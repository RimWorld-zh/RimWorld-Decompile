using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B5C RID: 2908
	public class PawnKindDef : Def
	{
		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06003F8C RID: 16268 RVA: 0x00217C94 File Offset: 0x00216094
		public RaceProperties RaceProps
		{
			get
			{
				return this.race.race;
			}
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x00217CB4 File Offset: 0x002160B4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.lifeStages.Count; i++)
			{
				this.lifeStages[i].ResolveReferences();
			}
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x00217CF8 File Offset: 0x002160F8
		public string GetLabelPlural(int count = -1)
		{
			string result;
			if (!this.labelPlural.NullOrEmpty())
			{
				result = this.labelPlural;
			}
			else
			{
				result = Find.ActiveLanguageWorker.Pluralize(this.label, count);
			}
			return result;
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x00217D3C File Offset: 0x0021613C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.race == null)
			{
				yield return "no race";
			}
			else if (this.RaceProps.Humanlike && this.backstoryCategory.NullOrEmpty())
			{
				yield return "Humanlike needs backstoryCategory.";
			}
			if (this.baseRecruitDifficulty > 1.0001f)
			{
				yield return this.defName + " recruitDifficulty is greater than 1. 1 means impossible to recruit.";
			}
			if (this.combatPower < 0f)
			{
				yield return this.defName + " has no combatPower.";
			}
			if (this.weaponMoney != FloatRange.Zero)
			{
				float minCost = 999999f;
				int i;
				for (i = 0; i < this.weaponTags.Count; i++)
				{
					IEnumerable<ThingDef> source = from d in DefDatabase<ThingDef>.AllDefs
					where d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])
					select d;
					if (source.Any<ThingDef>())
					{
						minCost = Mathf.Min(minCost, source.Min((ThingDef d) => PawnWeaponGenerator.CheapestNonDerpPriceFor(d)));
					}
				}
				if (minCost > this.weaponMoney.min)
				{
					yield return string.Concat(new object[]
					{
						"Cheapest weapon with one of my weaponTags costs ",
						minCost,
						" but weaponMoney min is ",
						this.weaponMoney.min,
						", so could end up weaponless."
					});
				}
			}
			if (!this.RaceProps.Humanlike && this.lifeStages.Count != this.RaceProps.lifeStageAges.Count)
			{
				yield return string.Concat(new object[]
				{
					"PawnKindDef defines ",
					this.lifeStages.Count,
					" lifeStages while race def defines ",
					this.RaceProps.lifeStageAges.Count
				});
			}
			if (this.apparelRequired != null)
			{
				for (int k = 0; k < this.apparelRequired.Count; k++)
				{
					for (int j = k + 1; j < this.apparelRequired.Count; j++)
					{
						if (!ApparelUtility.CanWearTogether(this.apparelRequired[k], this.apparelRequired[j], this.race.race.body))
						{
							yield return string.Concat(new object[]
							{
								"required apparel can't be worn together (",
								this.apparelRequired[k],
								", ",
								this.apparelRequired[j],
								")"
							});
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x00217D68 File Offset: 0x00216168
		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}

		// Token: 0x04002A49 RID: 10825
		public ThingDef race = null;

		// Token: 0x04002A4A RID: 10826
		public FactionDef defaultFactionType;

		// Token: 0x04002A4B RID: 10827
		[NoTranslate]
		public string backstoryCategory = null;

		// Token: 0x04002A4C RID: 10828
		[MustTranslate]
		public string labelPlural = null;

		// Token: 0x04002A4D RID: 10829
		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		// Token: 0x04002A4E RID: 10830
		public float backstoryCryptosleepCommonality = 0f;

		// Token: 0x04002A4F RID: 10831
		public int minGenerationAge = 0;

		// Token: 0x04002A50 RID: 10832
		public int maxGenerationAge = 999999;

		// Token: 0x04002A51 RID: 10833
		public bool factionLeader = false;

		// Token: 0x04002A52 RID: 10834
		public bool destroyGearOnDrop = false;

		// Token: 0x04002A53 RID: 10835
		public bool isFighter = true;

		// Token: 0x04002A54 RID: 10836
		public float combatPower = -1f;

		// Token: 0x04002A55 RID: 10837
		public bool canArriveManhunter = true;

		// Token: 0x04002A56 RID: 10838
		public bool canBeSapper = false;

		// Token: 0x04002A57 RID: 10839
		public float baseRecruitDifficulty = 0.5f;

		// Token: 0x04002A58 RID: 10840
		public bool aiAvoidCover = false;

		// Token: 0x04002A59 RID: 10841
		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		// Token: 0x04002A5A RID: 10842
		public QualityCategory itemQuality = QualityCategory.Normal;

		// Token: 0x04002A5B RID: 10843
		public bool forceNormalGearQuality = false;

		// Token: 0x04002A5C RID: 10844
		public FloatRange gearHealthRange = FloatRange.One;

		// Token: 0x04002A5D RID: 10845
		public FloatRange weaponMoney = FloatRange.Zero;

		// Token: 0x04002A5E RID: 10846
		[NoTranslate]
		public List<string> weaponTags = null;

		// Token: 0x04002A5F RID: 10847
		public FloatRange apparelMoney = FloatRange.Zero;

		// Token: 0x04002A60 RID: 10848
		public List<ThingDef> apparelRequired = null;

		// Token: 0x04002A61 RID: 10849
		[NoTranslate]
		public List<string> apparelTags = null;

		// Token: 0x04002A62 RID: 10850
		public float apparelAllowHeadgearChance = 1f;

		// Token: 0x04002A63 RID: 10851
		public bool apparelIgnoreSeasons = false;

		// Token: 0x04002A64 RID: 10852
		public FloatRange techHediffsMoney = FloatRange.Zero;

		// Token: 0x04002A65 RID: 10853
		[NoTranslate]
		public List<string> techHediffsTags = null;

		// Token: 0x04002A66 RID: 10854
		public float techHediffsChance = 0f;

		// Token: 0x04002A67 RID: 10855
		public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

		// Token: 0x04002A68 RID: 10856
		public PawnInventoryOption inventoryOptions = null;

		// Token: 0x04002A69 RID: 10857
		public float invNutrition = 0f;

		// Token: 0x04002A6A RID: 10858
		public ThingDef invFoodDef = null;

		// Token: 0x04002A6B RID: 10859
		public float chemicalAddictionChance = 0f;

		// Token: 0x04002A6C RID: 10860
		public float combatEnhancingDrugsChance = 0f;

		// Token: 0x04002A6D RID: 10861
		public IntRange combatEnhancingDrugsCount = IntRange.zero;

		// Token: 0x04002A6E RID: 10862
		public bool trader = false;

		// Token: 0x04002A6F RID: 10863
		[MustTranslate]
		public string labelMale = null;

		// Token: 0x04002A70 RID: 10864
		[MustTranslate]
		public string labelMalePlural = null;

		// Token: 0x04002A71 RID: 10865
		[MustTranslate]
		public string labelFemale = null;

		// Token: 0x04002A72 RID: 10866
		[MustTranslate]
		public string labelFemalePlural = null;

		// Token: 0x04002A73 RID: 10867
		public IntRange wildGroupSize = IntRange.one;

		// Token: 0x04002A74 RID: 10868
		public float ecoSystemWeight = 1f;
	}
}
