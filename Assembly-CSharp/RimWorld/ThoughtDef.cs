using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E4 RID: 740
	public class ThoughtDef : Def
	{
		// Token: 0x040007AF RID: 1967
		public Type thoughtClass = null;

		// Token: 0x040007B0 RID: 1968
		public Type workerClass = null;

		// Token: 0x040007B1 RID: 1969
		public List<ThoughtStage> stages = new List<ThoughtStage>();

		// Token: 0x040007B2 RID: 1970
		public int stackLimit = 1;

		// Token: 0x040007B3 RID: 1971
		public float stackedEffectMultiplier = 0.75f;

		// Token: 0x040007B4 RID: 1972
		public float durationDays = 0f;

		// Token: 0x040007B5 RID: 1973
		public bool invert = false;

		// Token: 0x040007B6 RID: 1974
		public bool validWhileDespawned = false;

		// Token: 0x040007B7 RID: 1975
		public ThoughtDef nextThought = null;

		// Token: 0x040007B8 RID: 1976
		public List<TraitDef> nullifyingTraits = null;

		// Token: 0x040007B9 RID: 1977
		public List<TaleDef> nullifyingOwnTales = null;

		// Token: 0x040007BA RID: 1978
		public List<TraitDef> requiredTraits = null;

		// Token: 0x040007BB RID: 1979
		public int requiredTraitsDegree = int.MinValue;

		// Token: 0x040007BC RID: 1980
		public StatDef effectMultiplyingStat = null;

		// Token: 0x040007BD RID: 1981
		public HediffDef hediff;

		// Token: 0x040007BE RID: 1982
		public GameConditionDef gameCondition;

		// Token: 0x040007BF RID: 1983
		public bool nullifiedIfNotColonist;

		// Token: 0x040007C0 RID: 1984
		public ThoughtDef thoughtToMake = null;

		// Token: 0x040007C1 RID: 1985
		[NoTranslate]
		private string icon = null;

		// Token: 0x040007C2 RID: 1986
		public bool showBubble = false;

		// Token: 0x040007C3 RID: 1987
		public int stackLimitForSameOtherPawn = -1;

		// Token: 0x040007C4 RID: 1988
		public float lerpOpinionToZeroAfterDurationPct = 0.7f;

		// Token: 0x040007C5 RID: 1989
		public float maxCumulatedOpinionOffset = float.MaxValue;

		// Token: 0x040007C6 RID: 1990
		public TaleDef taleDef;

		// Token: 0x040007C7 RID: 1991
		[Unsaved]
		private ThoughtWorker workerInt = null;

		// Token: 0x040007C8 RID: 1992
		private Texture2D iconInt;

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x0006C2A8 File Offset: 0x0006A6A8
		public string Label
		{
			get
			{
				string result;
				if (!this.label.NullOrEmpty())
				{
					result = this.label;
				}
				else
				{
					if (!this.stages.NullOrEmpty<ThoughtStage>())
					{
						if (!this.stages[0].label.NullOrEmpty())
						{
							return this.stages[0].label;
						}
						if (!this.stages[0].labelSocial.NullOrEmpty())
						{
							return this.stages[0].labelSocial;
						}
					}
					Log.Error("Cannot get good label for ThoughtDef " + this.defName, false);
					result = this.defName;
				}
				return result;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x0006C36C File Offset: 0x0006A76C
		public int DurationTicks
		{
			get
			{
				return (int)(this.durationDays * 60000f);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x0006C390 File Offset: 0x0006A790
		public bool IsMemory
		{
			get
			{
				return this.durationDays > 0f || typeof(Thought_Memory).IsAssignableFrom(this.thoughtClass);
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0006C3D0 File Offset: 0x0006A7D0
		public bool IsSituational
		{
			get
			{
				return this.Worker != null;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x0006C3F4 File Offset: 0x0006A7F4
		public bool IsSocial
		{
			get
			{
				return typeof(ISocialThought).IsAssignableFrom(this.ThoughtClass);
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0006C420 File Offset: 0x0006A820
		public bool RequiresSpecificTraitsDegree
		{
			get
			{
				return this.requiredTraitsDegree != int.MinValue;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x0006C448 File Offset: 0x0006A848
		public ThoughtWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (ThoughtWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0006C4A0 File Offset: 0x0006A8A0
		public Type ThoughtClass
		{
			get
			{
				Type typeFromHandle;
				if (this.thoughtClass != null)
				{
					typeFromHandle = this.thoughtClass;
				}
				else if (this.IsMemory)
				{
					typeFromHandle = typeof(Thought_Memory);
				}
				else
				{
					typeFromHandle = typeof(Thought_Situational);
				}
				return typeFromHandle;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x0006C4F4 File Offset: 0x0006A8F4
		public Texture2D Icon
		{
			get
			{
				if (this.iconInt == null)
				{
					if (this.icon == null)
					{
						return null;
					}
					this.iconInt = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconInt;
			}
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0006C548 File Offset: 0x0006A948
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return error;
			}
			if (this.stages.NullOrEmpty<ThoughtStage>())
			{
				yield return "no stages";
			}
			if (this.workerClass != null && this.nextThought != null)
			{
				yield return "has a nextThought but also has a workerClass. nextThought only works for memories";
			}
			if (this.IsMemory && this.workerClass != null)
			{
				yield return "has a workerClass but is a memory. workerClass only works for situational thoughts, not memories";
			}
			if (!this.IsMemory && this.workerClass == null && this.IsSituational)
			{
				yield return "is a situational thought but has no workerClass. Situational thoughts require workerClasses to analyze the situation";
			}
			for (int i = 0; i < this.stages.Count; i++)
			{
				if (this.stages[i] != null)
				{
					foreach (string e in this.stages[i].ConfigErrors())
					{
						yield return e;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0006C574 File Offset: 0x0006A974
		public static ThoughtDef Named(string defName)
		{
			return DefDatabase<ThoughtDef>.GetNamed(defName, true);
		}
	}
}
