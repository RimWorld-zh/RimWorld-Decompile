using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			PawnKindDef.<ConfigErrors>c__Iterator1D5 <ConfigErrors>c__Iterator1D = new PawnKindDef.<ConfigErrors>c__Iterator1D5();
			<ConfigErrors>c__Iterator1D.<>f__this = this;
			PawnKindDef.<ConfigErrors>c__Iterator1D5 expr_0E = <ConfigErrors>c__Iterator1D;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}
	}
}
