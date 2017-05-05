using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return (T)((object)null);
		}

		public bool PossibleToDevelopImmunityNaturally()
		{
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.CompProps<HediffCompProperties_Immunizable>();
			return hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0f || hediffCompProperties_Immunizable.immunityPerDaySick > 0f);
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			HediffDef.<ConfigErrors>c__Iterator1CB <ConfigErrors>c__Iterator1CB = new HediffDef.<ConfigErrors>c__Iterator1CB();
			<ConfigErrors>c__Iterator1CB.<>f__this = this;
			HediffDef.<ConfigErrors>c__Iterator1CB expr_0E = <ConfigErrors>c__Iterator1CB;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		[DebuggerHidden]
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			HediffDef.<SpecialDisplayStats>c__Iterator1CC <SpecialDisplayStats>c__Iterator1CC = new HediffDef.<SpecialDisplayStats>c__Iterator1CC();
			<SpecialDisplayStats>c__Iterator1CC.<>f__this = this;
			HediffDef.<SpecialDisplayStats>c__Iterator1CC expr_0E = <SpecialDisplayStats>c__Iterator1CC;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
		}
	}
}
