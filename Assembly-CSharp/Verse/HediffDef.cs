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

		public bool cureAllAtOnceIfCuredByItem;

		public TaleDef taleOnVisible;

		public bool everCurableByItem = true;

		public bool displayWound;

		public Color defaultLabelColor = Color.white;

		public InjuryProps injuryProps;

		public AddedBodyPartProps addedPartProps;

		public string labelNoun;

		private bool alwaysAllowMothballCached;

		private bool alwaysAllowMothball;

		private Hediff concreteExampleInt;

		public bool IsAddiction
		{
			get
			{
				return typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass);
			}
		}

		public bool AlwaysAllowMothball
		{
			get
			{
				if (!this.alwaysAllowMothballCached)
				{
					this.alwaysAllowMothball = true;
					if (this.comps != null && this.comps.Count > 0)
					{
						this.alwaysAllowMothball = false;
					}
					if (this.stages != null)
					{
						for (int i = 0; i < this.stages.Count; i++)
						{
							HediffStage hediffStage = this.stages[i];
							if (hediffStage.deathMtbDays > 0.0 || (hediffStage.hediffGivers != null && hediffStage.hediffGivers.Count > 0))
							{
								this.alwaysAllowMothball = false;
							}
						}
					}
					this.alwaysAllowMothballCached = true;
				}
				return this.alwaysAllowMothball;
			}
		}

		public Hediff ConcreteExample
		{
			get
			{
				if (this.concreteExampleInt == null)
				{
					this.concreteExampleInt = HediffMaker.MakeConcreteExampleHediff(this);
				}
				return this.concreteExampleInt;
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
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string err = enumerator.Current;
					yield return err;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.hediffClass == null)
			{
				yield return "hediffClass is null";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!this.comps.NullOrEmpty() && !typeof(HediffWithComps).IsAssignableFrom(this.hediffClass))
			{
				yield return "has comps but hediffClass is not HediffWithComps or subclass thereof";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.minSeverity > this.initialSeverity)
			{
				yield return "minSeverity is greater than initialSeverity";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.maxSeverity < this.initialSeverity)
			{
				yield return "maxSeverity is lower than initialSeverity";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!this.tendable && this.HasComp(typeof(HediffComp_TendDuration)))
			{
				yield return "has HediffComp_TendDuration but tendable = false";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.comps != null)
			{
				for (int k = 0; k < this.comps.Count; k++)
				{
					using (IEnumerator<string> enumerator2 = this.comps[k].ConfigErrors(this).GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							string compErr = enumerator2.Current;
							yield return this.comps[k] + ": " + compErr;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			if (this.stages == null)
				yield break;
			if (!typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass))
			{
				for (int j = 0; j < this.stages.Count; j++)
				{
					if (j >= 1 && this.stages[j].minSeverity <= this.stages[j - 1].minSeverity)
					{
						yield return "stages are not in order of minSeverity";
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			int i = 0;
			while (true)
			{
				if (i < this.stages.Count)
				{
					if (this.stages[i].makeImmuneTo != null && !this.stages[i].makeImmuneTo.Any((HediffDef im) => im.HasComp(typeof(HediffComp_Immunizable))))
						break;
					i++;
					continue;
				}
				yield break;
			}
			yield return "makes immune to hediff which doesn't have comp immunizable";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_04a3:
			/*Error near IL_04a4: Unexpected return in MoveNext()*/;
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.stages != null && this.stages.Count == 1)
			{
				using (IEnumerator<StatDrawEntry> enumerator = this.stages[0].SpecialDisplayStats().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						StatDrawEntry de = enumerator.Current;
						yield return de;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_00ea:
			/*Error near IL_00eb: Unexpected return in MoveNext()*/;
		}

		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
		}
	}
}
