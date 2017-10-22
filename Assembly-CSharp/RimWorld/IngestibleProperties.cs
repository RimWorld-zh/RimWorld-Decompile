using System.Collections.Generic;
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
				return (3903 & (int)this.foodType) != 0;
			}
		}

		public bool IsMeal
		{
			get
			{
				return (int)this.preferability >= 5 && (int)this.preferability <= 8;
			}
		}

		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.preferability == FoodPreferability.Undefined)
			{
				yield return "undefined preferability";
			}
			if (this.foodType == FoodTypeFlags.None)
			{
				yield return "no foodType";
			}
			if (this.nutrition == 0.0 && this.preferability != FoodPreferability.NeverForNutrition)
			{
				yield return "nutrition == 0 but preferability is " + this.preferability + " instead of " + FoodPreferability.NeverForNutrition;
			}
			if (!parentDef.IsCorpse && (int)this.preferability > 2 && !parentDef.socialPropernessMatters && parentDef.EverHaulable)
			{
				yield return "ingestible preferability > DesperateOnly but socialPropernessMatters=false. This will cause bugs wherein wardens will look in prison cells for food to give to prisoners and so will repeatedly pick up and drop food inside the cell.";
			}
			if (this.joy > 0.0 && this.joyKind == null)
			{
				yield return "joy > 0 with no joy kind";
			}
		}

		internal IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (!parentDef.IsCorpse)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Nutrition".Translate(), this.nutrition.ToString("0.##"), 0);
			}
			if (this.joy > 0.0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Joy".Translate(), this.joy.ToStringPercent("F2") + " (" + this.JoyKind.label + ")", 0);
			}
			if (this.outcomeDoers != null)
			{
				for (int i = 0; i < this.outcomeDoers.Count; i++)
				{
					foreach (StatDrawEntry item in this.outcomeDoers[i].SpecialDisplayStats(parentDef))
					{
						yield return item;
					}
				}
			}
		}
	}
}
