using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class HediffDef : Def
	{
		public Type hediffClass = typeof(Hediff);

		public List<HediffCompProperties> comps;

		public float initialSeverity = 0.5f;

		public float lethalSeverity = -1f;

		public List<HediffStage> stages;

		public bool tendable;

		public bool isBad = true;

		public ThingDef spawnThingOnRemoved;

		public float chanceToCauseNoPain;

		public bool makesSickThought;

		public bool makesAlert = true;

		public NeedDef causesNeed;

		public float minSeverity;

		public float maxSeverity = 3.40282347E+38f;

		public bool scenarioCanAdd;

		public List<HediffGiver> hediffGivers;

		public bool displayWound;

		public Color defaultLabelColor = Color.white;

		public InjuryProps injuryProps;

		public AddedBodyPartProps addedPartProps;

		public bool IsAddiction
		{
			get
			{
				return typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass);
			}
		}

		public bool HasComp(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return true;
					}
				}
			}
			return false;
		}

		public HediffCompProperties CompPropsFor(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return this.comps[i];
					}
				}
			}
			return null;
		}

		public T CompProps<T>() where T : class
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					T val = (T)(((object)this.comps[i]) as T);
					if (val != null)
					{
						return val;
					}
				}
			}
			return (T)null;
		}

		public bool PossibleToDevelopImmunityNaturally()
		{
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.CompProps<HediffCompProperties_Immunizable>();
			if (hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0.0 || hediffCompProperties_Immunizable.immunityPerDaySick > 0.0))
			{
				return true;
			}
			return false;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.hediffClass == null)
			{
				yield return "hediffClass is null";
			}
			if (!this.comps.NullOrEmpty() && !typeof(HediffWithComps).IsAssignableFrom(this.hediffClass))
			{
				yield return "has comps but hediffClass is not HediffWithComps or subclass thereof";
			}
			if (this.minSeverity > this.initialSeverity)
			{
				yield return "minSeverity is greater than initialSeverity";
			}
			if (this.maxSeverity < this.initialSeverity)
			{
				yield return "maxSeverity is lower than initialSeverity";
			}
			if (!this.tendable && this.HasComp(typeof(HediffComp_TendDuration)))
			{
				yield return "has HediffComp_TendDuration but tendable = false";
			}
			if (this.comps != null)
			{
				for (int k = 0; k < this.comps.Count; k++)
				{
					foreach (string item2 in this.comps[k].ConfigErrors(this))
					{
						yield return this.comps[k] + ": " + item2;
					}
				}
			}
			if (this.stages != null)
			{
				if (!typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass))
				{
					for (int j = 0; j < this.stages.Count; j++)
					{
						if (j >= 1 && this.stages[j].minSeverity <= this.stages[j - 1].minSeverity)
						{
							yield return "stages are not in order of minSeverity";
						}
					}
				}
				for (int i = 0; i < this.stages.Count; i++)
				{
					if (this.stages[i].makeImmuneTo != null && !this.stages[i].makeImmuneTo.Any((Predicate<HediffDef>)((HediffDef im) => im.HasComp(typeof(HediffComp_Immunizable)))))
					{
						yield return "makes immune to hediff which doesn't have comp immunizable";
					}
				}
			}
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.stages != null && this.stages.Count == 1)
			{
				foreach (StatDrawEntry item in this.stages[0].SpecialDisplayStats())
				{
					yield return item;
				}
			}
		}

		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
		}
	}
}
