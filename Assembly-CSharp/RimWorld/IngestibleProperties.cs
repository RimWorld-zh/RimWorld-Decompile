using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IngestibleProperties
	{
		public int maxNumToIngestAtOnce = 20;

		public List<IngestionOutcomeDoer> outcomeDoers = null;

		public int baseIngestTicks = 500;

		public float chairSearchRadius = 25f;

		public bool useEatingSpeedStat = true;

		public ThoughtDef tasteThought = null;

		public ThoughtDef specialThoughtDirect = null;

		public ThoughtDef specialThoughtAsIngredient = null;

		public EffecterDef ingestEffect = null;

		public EffecterDef ingestEffectEat = null;

		public SoundDef ingestSound = null;

		public string ingestCommandString = (string)null;

		public string ingestReportString = (string)null;

		public string ingestReportStringEat = (string)null;

		public HoldOffsetSet ingestHoldOffsetStanding = null;

		public bool ingestHoldUsesTable = true;

		public FoodTypeFlags foodType = FoodTypeFlags.None;

		public float nutrition = 0f;

		public float joy = 0f;

		public JoyKindDef joyKind = null;

		public ThingDef sourceDef;

		public FoodPreferability preferability = FoodPreferability.Undefined;

		public bool nurseable = false;

		public float optimalityOffsetHumanlikes = 0f;

		public float optimalityOffsetFeedingAnimals = 0f;

		public DrugCategory drugCategory = DrugCategory.None;

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
				return (int)this.preferability >= 6 && (int)this.preferability <= 9;
			}
		}

		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.preferability == FoodPreferability.Undefined)
			{
				yield return "undefined preferability";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.foodType == FoodTypeFlags.None)
			{
				yield return "no foodType";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.nutrition == 0.0 && this.preferability != FoodPreferability.NeverForNutrition)
			{
				yield return "nutrition == 0 but preferability is " + this.preferability + " instead of " + FoodPreferability.NeverForNutrition;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!parentDef.IsCorpse && (int)this.preferability > 3 && !parentDef.socialPropernessMatters && parentDef.EverHaulable)
			{
				yield return "ingestible preferability > DesperateOnlyForHumanlikes but socialPropernessMatters=false. This will cause bugs wherein wardens will look in prison cells for food to give to prisoners and so will repeatedly pick up and drop food inside the cell.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!(this.joy > 0.0))
				yield break;
			if (this.joyKind != null)
				yield break;
			yield return "joy > 0 with no joy kind";
			/*Error: Unable to find new state assignment for yield return*/;
		}

		internal IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (!parentDef.IsCorpse)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Nutrition".Translate(), this.nutrition.ToString("0.##"), 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.joy > 0.0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Joy".Translate(), this.joy.ToStringPercent("F2") + " (" + this.JoyKind.label + ")", 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.outcomeDoers != null)
			{
				for (int i = 0; i < this.outcomeDoers.Count; i++)
				{
					using (IEnumerator<StatDrawEntry> enumerator = this.outcomeDoers[i].SpecialDisplayStats(parentDef).GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							StatDrawEntry s = enumerator.Current;
							yield return s;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			yield break;
			IL_01fa:
			/*Error near IL_01fb: Unexpected return in MoveNext()*/;
		}
	}
}
