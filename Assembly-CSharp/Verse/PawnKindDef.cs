using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class PawnKindDef : Def
	{
		public ThingDef race;

		public FactionDef defaultFactionType;

		public string backstoryCategory;

		public string labelPlural;

		public float backstoryCryptosleepCommonality;

		public bool forceNormalGearQuality;

		public int minGenerationAge;

		public int maxGenerationAge = 999999;

		public FloatRange gearHealthRange = FloatRange.One;

		public bool factionLeader;

		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		public bool isFighter = true;

		public float combatPower = -1f;

		public bool canArriveManhunter = true;

		public float baseRecruitDifficulty = 0.5f;

		public bool aiAvoidCover;

		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		public FloatRange apparelMoney = FloatRange.Zero;

		public List<ThingDef> apparelRequired;

		public List<string> apparelTags;

		public float apparelAllowHeadwearChance = 1f;

		public bool apparelIgnoreSeasons;

		public FloatRange weaponMoney = FloatRange.Zero;

		public List<string> weaponTags;

		public FloatRange techHediffsMoney = FloatRange.Zero;

		public List<string> techHediffsTags;

		public float techHediffsChance = 0.1f;

		public QualityCategory itemQuality = QualityCategory.Normal;

		public List<ThingCountClass> fixedInventory = new List<ThingCountClass>();

		public PawnInventoryOption inventoryOptions;

		public float invNutrition;

		public ThingDef invFoodDef;

		public float chemicalAddictionChance;

		public float combatEnhancingDrugsChance = 0.04f;

		public IntRange combatEnhancingDrugsCount = IntRange.zero;

		public bool trader;

		public string labelMale;

		public string labelMalePlural;

		public string labelFemale;

		public string labelFemalePlural;

		public bool wildSpawn_spawnWild;

		public float wildSpawn_EcoSystemWeight = 1f;

		public IntRange wildSpawn_GroupSizeRange = IntRange.one;

		public RaceProperties RaceProps
		{
			get
			{
				return this.race.race;
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.lifeStages.Count; i++)
			{
				this.lifeStages[i].ResolveReferences();
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.race == null)
			{
				yield return "no race";
			}
			else if (this.RaceProps.Humanlike && this.backstoryCategory.NullOrEmpty())
			{
				yield return "Humanlike needs backstoryCategory.";
			}
			if (this.baseRecruitDifficulty > 1.0001000165939331)
			{
				yield return base.defName + " recruitDifficulty is greater than 1. 1 means impossible to recruit.";
			}
			if (this.combatPower < 0.0)
			{
				yield return base.defName + " has no pointsCost.";
			}
			if (this.weaponMoney.min > 0.0)
			{
				float minCost = 999999f;
				for (int i = 0; i < this.weaponTags.Count; i++)
				{
					minCost = Mathf.Min(minCost, (from d in DefDatabase<ThingDef>.AllDefs
					where d.weaponTags != null && d.weaponTags.Contains(((_003CConfigErrors_003Ec__Iterator1D5)/*Error near IL_01e0: stateMachine*/)._003C_003Ef__this.weaponTags[((_003CConfigErrors_003Ec__Iterator1D5)/*Error near IL_01e0: stateMachine*/)._003Ci_003E__3])
					select d).Min((Func<ThingDef, float>)((ThingDef d) => PawnWeaponGenerator.CheapestNonDerpPriceFor(d))));
				}
				if (minCost > this.weaponMoney.min)
				{
					yield return "Cheapest weapon with one of my weaponTags costs " + minCost + " but weaponMoney min is " + this.weaponMoney.min + ", so could end up weaponless.";
				}
			}
			if (!this.RaceProps.Humanlike && this.lifeStages.Count != this.RaceProps.lifeStageAges.Count)
			{
				yield return "PawnKindDef defines " + this.lifeStages.Count + " lifeStages while race def defines " + this.RaceProps.lifeStageAges.Count;
			}
		}

		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}
	}
}
