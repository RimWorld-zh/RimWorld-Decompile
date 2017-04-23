using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class IngestibleProperties
	{
		public int maxNumToIngestAtOnce = 20;

		public List<IngestionOutcomeDoer> outcomeDoers;

		public int baseIngestTicks = 500;

		public float chairSearchRadius = 25f;

		public bool useEatingSpeedStat = true;

		public ThoughtDef tasteThought;

		public ThoughtDef specialThoughtDirect;

		public ThoughtDef specialThoughtAsIngredient;

		public EffecterDef ingestEffect;

		public EffecterDef ingestEffectEat;

		public SoundDef ingestSound;

		public string ingestCommandString;

		public string ingestReportString;

		public HoldOffsetSet ingestHoldOffsetStanding;

		public bool ingestHoldUsesTable = true;

		public FoodTypeFlags foodType;

		public float nutrition;

		public float joy;

		public JoyKindDef joyKind;

		public ThingDef sourceDef;

		public FoodPreferability preferability;

		public bool nurseable;

		public float optimalityOffset;

		public DrugCategory drugCategory;

		public JoyKindDef JoyKind
		{
			get
			{
				return (this.joyKind == null) ? JoyKindDefOf.Gluttonous : this.joyKind;
			}
		}

		public bool HumanEdible
		{
			get
			{
				return (FoodTypeFlags.OmnivoreHuman & this.foodType) != FoodTypeFlags.None;
			}
		}

		public bool IsMeal
		{
			get
			{
				return this.preferability >= FoodPreferability.MealAwful && this.preferability <= FoodPreferability.MealLavish;
			}
		}

		[DebuggerHidden]
		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			IngestibleProperties.<ConfigErrors>c__Iterator7D <ConfigErrors>c__Iterator7D = new IngestibleProperties.<ConfigErrors>c__Iterator7D();
			<ConfigErrors>c__Iterator7D.parentDef = parentDef;
			<ConfigErrors>c__Iterator7D.<$>parentDef = parentDef;
			<ConfigErrors>c__Iterator7D.<>f__this = this;
			IngestibleProperties.<ConfigErrors>c__Iterator7D expr_1C = <ConfigErrors>c__Iterator7D;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		[DebuggerHidden]
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			IngestibleProperties.<SpecialDisplayStats>c__Iterator7E <SpecialDisplayStats>c__Iterator7E = new IngestibleProperties.<SpecialDisplayStats>c__Iterator7E();
			<SpecialDisplayStats>c__Iterator7E.parentDef = parentDef;
			<SpecialDisplayStats>c__Iterator7E.<$>parentDef = parentDef;
			<SpecialDisplayStats>c__Iterator7E.<>f__this = this;
			IngestibleProperties.<SpecialDisplayStats>c__Iterator7E expr_1C = <SpecialDisplayStats>c__Iterator7E;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
