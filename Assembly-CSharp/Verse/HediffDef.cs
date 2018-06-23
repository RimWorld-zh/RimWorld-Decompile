using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B3E RID: 2878
	public class HediffDef : Def
	{
		// Token: 0x0400296E RID: 10606
		public Type hediffClass = typeof(Hediff);

		// Token: 0x0400296F RID: 10607
		public List<HediffCompProperties> comps = null;

		// Token: 0x04002970 RID: 10608
		public float initialSeverity = 0.5f;

		// Token: 0x04002971 RID: 10609
		public float lethalSeverity = -1f;

		// Token: 0x04002972 RID: 10610
		public List<HediffStage> stages = null;

		// Token: 0x04002973 RID: 10611
		public bool tendable = false;

		// Token: 0x04002974 RID: 10612
		public bool isBad = true;

		// Token: 0x04002975 RID: 10613
		public ThingDef spawnThingOnRemoved = null;

		// Token: 0x04002976 RID: 10614
		public float chanceToCauseNoPain = 0f;

		// Token: 0x04002977 RID: 10615
		public bool makesSickThought = false;

		// Token: 0x04002978 RID: 10616
		public bool makesAlert = true;

		// Token: 0x04002979 RID: 10617
		public NeedDef causesNeed = null;

		// Token: 0x0400297A RID: 10618
		public float minSeverity = 0f;

		// Token: 0x0400297B RID: 10619
		public float maxSeverity = float.MaxValue;

		// Token: 0x0400297C RID: 10620
		public bool scenarioCanAdd = false;

		// Token: 0x0400297D RID: 10621
		public List<HediffGiver> hediffGivers = null;

		// Token: 0x0400297E RID: 10622
		public bool cureAllAtOnceIfCuredByItem = false;

		// Token: 0x0400297F RID: 10623
		public TaleDef taleOnVisible = null;

		// Token: 0x04002980 RID: 10624
		public bool everCurableByItem = true;

		// Token: 0x04002981 RID: 10625
		public string battleStateLabel = null;

		// Token: 0x04002982 RID: 10626
		public string labelNounPretty = null;

		// Token: 0x04002983 RID: 10627
		public bool displayWound = false;

		// Token: 0x04002984 RID: 10628
		public Color defaultLabelColor = Color.white;

		// Token: 0x04002985 RID: 10629
		public InjuryProps injuryProps = null;

		// Token: 0x04002986 RID: 10630
		public AddedBodyPartProps addedPartProps = null;

		// Token: 0x04002987 RID: 10631
		[MustTranslate]
		public string labelNoun = null;

		// Token: 0x04002988 RID: 10632
		private bool alwaysAllowMothballCached = false;

		// Token: 0x04002989 RID: 10633
		private bool alwaysAllowMothball;

		// Token: 0x0400298A RID: 10634
		private Hediff concreteExampleInt;

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06003F38 RID: 16184 RVA: 0x002148B4 File Offset: 0x00212CB4
		public bool IsAddiction
		{
			get
			{
				return typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass);
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06003F39 RID: 16185 RVA: 0x002148E0 File Offset: 0x00212CE0
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
							if (hediffStage.deathMtbDays > 0f || (hediffStage.hediffGivers != null && hediffStage.hediffGivers.Count > 0))
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

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06003F3A RID: 16186 RVA: 0x002149A4 File Offset: 0x00212DA4
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

		// Token: 0x06003F3B RID: 16187 RVA: 0x002149D8 File Offset: 0x00212DD8
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

		// Token: 0x06003F3C RID: 16188 RVA: 0x00214A38 File Offset: 0x00212E38
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

		// Token: 0x06003F3D RID: 16189 RVA: 0x00214AA4 File Offset: 0x00212EA4
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

		// Token: 0x06003F3E RID: 16190 RVA: 0x00214B18 File Offset: 0x00212F18
		public bool PossibleToDevelopImmunityNaturally()
		{
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.CompProps<HediffCompProperties_Immunizable>();
			return hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0f || hediffCompProperties_Immunizable.immunityPerDaySick > 0f);
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x00214B64 File Offset: 0x00212F64
		public string PrettyTextForPart(BodyPartRecord bodyPart)
		{
			string result;
			if (this.labelNounPretty.NullOrEmpty())
			{
				result = null;
			}
			else
			{
				result = string.Format(this.labelNounPretty, this.label, bodyPart.Label);
			}
			return result;
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x00214BA8 File Offset: 0x00212FA8
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.hediffClass == null)
			{
				yield return "hediffClass is null";
			}
			if (!this.comps.NullOrEmpty<HediffCompProperties>() && !typeof(HediffWithComps).IsAssignableFrom(this.hediffClass))
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
				for (int i = 0; i < this.comps.Count; i++)
				{
					foreach (string compErr in this.comps[i].ConfigErrors(this))
					{
						yield return this.comps[i] + ": " + compErr;
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
				for (int k = 0; k < this.stages.Count; k++)
				{
					if (this.stages[k].makeImmuneTo != null)
					{
						if (!this.stages[k].makeImmuneTo.Any((HediffDef im) => im.HasComp(typeof(HediffComp_Immunizable))))
						{
							yield return "makes immune to hediff which doesn't have comp immunizable";
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x00214BD4 File Offset: 0x00212FD4
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.stages != null && this.stages.Count == 1)
			{
				foreach (StatDrawEntry de in this.stages[0].SpecialDisplayStats())
				{
					yield return de;
				}
			}
			yield break;
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x00214C00 File Offset: 0x00213000
		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
		}
	}
}
