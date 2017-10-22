using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class IncidentDef : Def
	{
		public Type workerClass;

		public IncidentCategory category = IncidentCategory.Undefined;

		public List<IncidentTargetTypeDef> targetTypes = null;

		public float baseChance = 0f;

		public IncidentPopulationEffect populationEffect = IncidentPopulationEffect.None;

		public int earliestDay = 0;

		public int minPopulation = 0;

		public float minRefireDays = 0f;

		public int minDifficulty = 0;

		public bool pointsScaleable = false;

		public float minThreatPoints = -1f;

		public List<BiomeDef> allowedBiomes;

		public List<string> tags;

		public List<string> refireCheckTags;

		public SimpleCurve chanceFactorByPopulationCurve = null;

		[MustTranslate]
		public string letterText;

		[MustTranslate]
		public string letterLabel;

		public LetterDef letterDef;

		public GameConditionDef gameCondition;

		public FloatRange durationDays;

		public HediffDef diseaseIncident = null;

		public FloatRange diseaseVictimFractionRange = new FloatRange(0f, 0.49f);

		public int diseaseMaxVictims = 99999;

		public List<BiomeDiseaseRecord> diseaseBiomeRecords = null;

		public List<BodyPartDef> diseasePartsToAffect = null;

		public ThingDef shipPart = null;

		public List<MTBByBiome> mtbDaysByBiome;

		public TaleDef tale = null;

		[Unsaved]
		private IncidentWorker workerInt = null;

		[Unsaved]
		private List<IncidentDef> cachedRefireCheckIncidents = null;

		public bool NeedsParms
		{
			get
			{
				return this.category == IncidentCategory.ThreatSmall || this.category == IncidentCategory.ThreatBig || this.category == IncidentCategory.RaidBeacon;
			}
		}

		public IncidentWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (IncidentWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		public List<IncidentDef> RefireCheckIncidents
		{
			get
			{
				List<IncidentDef> result;
				if (this.refireCheckTags == null)
				{
					result = null;
				}
				else
				{
					if (this.cachedRefireCheckIncidents == null)
					{
						this.cachedRefireCheckIncidents = new List<IncidentDef>();
						List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
						for (int i = 0; i < allDefsListForReading.Count; i++)
						{
							if (this.ShouldDoRefireCheckWith(allDefsListForReading[i]))
							{
								this.cachedRefireCheckIncidents.Add(allDefsListForReading[i]);
							}
						}
					}
					result = this.cachedRefireCheckIncidents;
				}
				return result;
			}
		}

		public static IncidentDef Named(string defName)
		{
			return DefDatabase<IncidentDef>.GetNamed(defName, true);
		}

		private bool ShouldDoRefireCheckWith(IncidentDef other)
		{
			bool result;
			if (other.tags == null)
			{
				result = false;
			}
			else if (other == this)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < other.tags.Count; i++)
				{
					for (int j = 0; j < this.refireCheckTags.Count; j++)
					{
						if (other.tags[i] == this.refireCheckTags[j])
							goto IL_0053;
					}
				}
				result = false;
			}
			goto IL_008d;
			IL_0053:
			result = true;
			goto IL_008d;
			IL_008d:
			return result;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string c = enumerator.Current;
					yield return c;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.category == IncidentCategory.Undefined)
			{
				yield return "category is undefined.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (((this.targetTypes != null) ? this.targetTypes.Count : 0) != 0)
			{
				if (this.TargetTypeAllowed(IncidentTargetTypeDefOf.World) && this.targetTypes.Any((Predicate<IncidentTargetTypeDef>)((IncidentTargetTypeDef tt) => tt != IncidentTargetTypeDefOf.World)))
				{
					yield return "allows world target type along with other targets. World targeting incidents should only target the world.";
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (!this.TargetTypeAllowed(IncidentTargetTypeDefOf.World))
					yield break;
				if (this.allowedBiomes == null)
					yield break;
				yield return "world-targeting incident has a biome restriction list";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield return "no target type";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_01ea:
			/*Error near IL_01eb: Unexpected return in MoveNext()*/;
		}

		public bool TargetTypeAllowed(IncidentTargetTypeDef target)
		{
			return this.targetTypes.Contains(target);
		}

		public bool TargetAllowed(IIncidentTarget target)
		{
			return this.targetTypes.Intersect(target.AcceptedTypes()).Any();
		}
	}
}
