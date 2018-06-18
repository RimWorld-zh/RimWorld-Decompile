using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000D03 RID: 3331
	public class Hediff : IExposable
	{
		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06004949 RID: 18761 RVA: 0x000A9D90 File Offset: 0x000A8190
		public virtual string LabelBase
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x0600494A RID: 18762 RVA: 0x000A9DB0 File Offset: 0x000A81B0
		public string Label
		{
			get
			{
				string labelInBrackets = this.LabelInBrackets;
				return this.LabelBase + ((!labelInBrackets.NullOrEmpty()) ? (" (" + labelInBrackets + ")") : "");
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x0600494B RID: 18763 RVA: 0x000A9DFC File Offset: 0x000A81FC
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x0600494C RID: 18764 RVA: 0x000A9E1C File Offset: 0x000A821C
		public virtual Color LabelColor
		{
			get
			{
				return this.def.defaultLabelColor;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x0600494D RID: 18765 RVA: 0x000A9E3C File Offset: 0x000A823C
		public virtual string LabelInBrackets
		{
			get
			{
				return (this.CurStage != null && !this.CurStage.label.NullOrEmpty()) ? this.CurStage.label : null;
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600494E RID: 18766 RVA: 0x000A9E84 File Offset: 0x000A8284
		public virtual string SeverityLabel
		{
			get
			{
				return (this.def.lethalSeverity > 0f) ? (this.Severity / this.def.lethalSeverity).ToStringPercent() : null;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600494F RID: 18767 RVA: 0x000A9ECC File Offset: 0x000A82CC
		public virtual int UIGroupKey
		{
			get
			{
				return this.Label.GetHashCode();
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06004950 RID: 18768 RVA: 0x000A9EEC File Offset: 0x000A82EC
		public virtual string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (StatDrawEntry statDrawEntry in HediffStatsUtility.SpecialDisplayStats(this.CurStage, this))
				{
					if (statDrawEntry.ShouldDisplay)
					{
						stringBuilder.AppendLine(statDrawEntry.LabelCap + ": " + statDrawEntry.ValueString);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06004951 RID: 18769 RVA: 0x000A9F88 File Offset: 0x000A8388
		public virtual HediffStage CurStage
		{
			get
			{
				return (!this.def.stages.NullOrEmpty<HediffStage>()) ? this.def.stages[this.CurStageIndex] : null;
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06004952 RID: 18770 RVA: 0x000A9FD0 File Offset: 0x000A83D0
		public virtual bool ShouldRemove
		{
			get
			{
				return this.Severity <= 0f;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06004953 RID: 18771 RVA: 0x000A9FF8 File Offset: 0x000A83F8
		public virtual bool Visible
		{
			get
			{
				return this.visible || this.CurStage == null || this.CurStage.becomeVisible;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06004954 RID: 18772 RVA: 0x000AA034 File Offset: 0x000A8434
		public virtual float BleedRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06004955 RID: 18773 RVA: 0x000AA050 File Offset: 0x000A8450
		public bool Bleeding
		{
			get
			{
				return this.BleedRate > 1E-05f;
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06004956 RID: 18774 RVA: 0x000AA074 File Offset: 0x000A8474
		public virtual float PainOffset
		{
			get
			{
				return (this.CurStage != null && !this.causesNoPain) ? this.CurStage.painOffset : 0f;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06004957 RID: 18775 RVA: 0x000AA0B4 File Offset: 0x000A84B4
		public virtual float PainFactor
		{
			get
			{
				return (this.CurStage != null) ? this.CurStage.painFactor : 1f;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06004958 RID: 18776 RVA: 0x000AA0EC File Offset: 0x000A84EC
		public List<PawnCapacityModifier> CapMods
		{
			get
			{
				return (this.CurStage != null) ? this.CurStage.capMods : null;
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06004959 RID: 18777 RVA: 0x000AA120 File Offset: 0x000A8520
		public virtual float SummaryHealthPercentImpact
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x0600495A RID: 18778 RVA: 0x000AA13C File Offset: 0x000A853C
		public virtual float TendPriority
		{
			get
			{
				float num = 0f;
				HediffStage curStage = this.CurStage;
				if (curStage != null && curStage.lifeThreatening)
				{
					num = Mathf.Max(num, 1f);
				}
				num = Mathf.Max(num, this.BleedRate * 1.5f);
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && hediffComp_TendDuration.TProps.severityPerDayTended < 0f)
				{
					num = Mathf.Max(num, 0.025f);
				}
				return num;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x0600495B RID: 18779 RVA: 0x000AA1C0 File Offset: 0x000A85C0
		public virtual TextureAndColor StateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x0600495C RID: 18780 RVA: 0x000AA1DC File Offset: 0x000A85DC
		public virtual int CurStageIndex
		{
			get
			{
				int result;
				if (this.def.stages == null)
				{
					result = 0;
				}
				else
				{
					List<HediffStage> stages = this.def.stages;
					float severity = this.Severity;
					for (int i = stages.Count - 1; i >= 0; i--)
					{
						if (severity >= stages[i].minSeverity)
						{
							return i;
						}
					}
					result = 0;
				}
				return result;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x0600495D RID: 18781 RVA: 0x000AA250 File Offset: 0x000A8650
		// (set) Token: 0x0600495E RID: 18782 RVA: 0x000AA26C File Offset: 0x000A866C
		public virtual float Severity
		{
			get
			{
				return this.severityInt;
			}
			set
			{
				bool flag = false;
				if (this.def.lethalSeverity > 0f && value >= this.def.lethalSeverity)
				{
					value = this.def.lethalSeverity;
					flag = true;
				}
				bool flag2 = this is Hediff_Injury && value > this.severityInt && Mathf.RoundToInt(value) != Mathf.RoundToInt(this.severityInt);
				int curStageIndex = this.CurStageIndex;
				this.severityInt = Mathf.Clamp(value, this.def.minSeverity, this.def.maxSeverity);
				if ((this.CurStageIndex != curStageIndex || flag || flag2) && this.pawn.health.hediffSet.hediffs.Contains(this))
				{
					this.pawn.health.Notify_HediffChanged(this);
					if (!this.pawn.Dead && this.pawn.needs.mood != null)
					{
						this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
					}
				}
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x0600495F RID: 18783 RVA: 0x000AA39C File Offset: 0x000A879C
		// (set) Token: 0x06004960 RID: 18784 RVA: 0x000AA3B8 File Offset: 0x000A87B8
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.pawn == null && this.part != null)
				{
					Log.Error("Hediff: Cannot set Part without setting pawn first.", false);
				}
				else if (UnityData.isDebugBuild && this.part != null && !this.pawn.RaceProps.body.AllParts.Contains(this.part))
				{
					Log.Error("Hediff: Cannot set BodyPartRecord which doesn't belong to the pawn " + this.pawn.ToStringSafe<Pawn>(), false);
				}
				else
				{
					this.part = value;
				}
			}
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x000AA450 File Offset: 0x000A8850
		public virtual bool TendableNow(bool ignoreTimer = false)
		{
			bool result;
			if (!this.def.tendable || this.Severity <= 0f || this.FullyImmune() || !this.Visible || this.IsPermanent())
			{
				result = false;
			}
			else
			{
				if (!ignoreTimer)
				{
					HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
					if (hediffComp_TendDuration != null && !hediffComp_TendDuration.AllowTend)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x000AA4D8 File Offset: 0x000A88D8
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.combatLogEntry != null)
			{
				LogEntry target = this.combatLogEntry.Target;
				if (!Current.Game.battleLog.IsEntryActive(target))
				{
					this.combatLogEntry = null;
				}
			}
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<HediffDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.source, "source");
			Scribe_Defs.Look<BodyPartGroupDef>(ref this.sourceBodyPartGroup, "sourceBodyPartGroup");
			Scribe_Defs.Look<HediffDef>(ref this.sourceHediffDef, "sourceHediffDef");
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_Values.Look<float>(ref this.severityInt, "severity", 0f, false);
			Scribe_Values.Look<bool>(ref this.recordedTale, "recordedTale", false, false);
			Scribe_Values.Look<bool>(ref this.causesNoPain, "causesNoPain", false, false);
			Scribe_Values.Look<bool>(ref this.visible, "visible", false, false);
			Scribe_References.Look<LogEntry>(ref this.combatLogEntry, "combatLogEntry", false);
			Scribe_Values.Look<string>(ref this.combatLogText, "combatLogText", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				BackCompatibility.HediffLoadingVars(this);
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				BackCompatibility.HediffResolvingCrossRefs(this);
			}
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x000AA62C File Offset: 0x000A8A2C
		public virtual void Tick()
		{
			this.ageTicks++;
			if (this.def.hediffGivers != null && this.pawn.IsHashIntervalTick(60))
			{
				for (int i = 0; i < this.def.hediffGivers.Count; i++)
				{
					this.def.hediffGivers[i].OnIntervalPassed(this.pawn, this);
				}
			}
			if (this.Visible && !this.visible)
			{
				this.visible = true;
				if (this.def.taleOnVisible != null)
				{
					TaleRecorder.RecordTale(this.def.taleOnVisible, new object[]
					{
						this.pawn,
						this.def
					});
				}
			}
			HediffStage curStage = this.CurStage;
			if (curStage != null)
			{
				if (curStage.hediffGivers != null && this.pawn.IsHashIntervalTick(60))
				{
					for (int j = 0; j < curStage.hediffGivers.Count; j++)
					{
						curStage.hediffGivers[j].OnIntervalPassed(this.pawn, this);
					}
				}
				if (curStage.mentalStateGivers != null && this.pawn.IsHashIntervalTick(60) && !this.pawn.InMentalState)
				{
					for (int k = 0; k < curStage.mentalStateGivers.Count; k++)
					{
						MentalStateGiver mentalStateGiver = curStage.mentalStateGivers[k];
						if (Rand.MTBEventOccurs(mentalStateGiver.mtbDays, 60000f, 60f))
						{
							this.pawn.mindState.mentalStateHandler.TryStartMentalState(mentalStateGiver.mentalState, null, false, false, null, false);
						}
					}
				}
				if (curStage.mentalBreakMtbDays > 0f && this.pawn.IsHashIntervalTick(60) && !this.pawn.InMentalState && Rand.MTBEventOccurs(curStage.mentalBreakMtbDays, 60000f, 60f))
				{
					MentalBreakDef mentalBreakDef;
					if ((from x in DefDatabase<MentalBreakDef>.AllDefsListForReading
					where x.Worker.BreakCanOccur(this.pawn)
					select x).TryRandomElementByWeight((MentalBreakDef x) => x.Worker.CommonalityFor(this.pawn), out mentalBreakDef))
					{
						mentalBreakDef.Worker.TryStart(this.pawn, null, false);
					}
				}
				if (curStage.vomitMtbDays > 0f)
				{
					if (this.pawn.IsHashIntervalTick(600) && Rand.MTBEventOccurs(curStage.vomitMtbDays, 60000f, 600f) && this.pawn.Spawned && this.pawn.Awake())
					{
						this.pawn.jobs.StartJob(new Job(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false);
					}
				}
				if (curStage.forgetMemoryThoughtMtbDays > 0f && this.pawn.needs.mood != null)
				{
					if (this.pawn.IsHashIntervalTick(400) && Rand.MTBEventOccurs(curStage.forgetMemoryThoughtMtbDays, 60000f, 400f))
					{
						Thought_Memory th;
						if (this.pawn.needs.mood.thoughts.memories.Memories.TryRandomElement(out th))
						{
							this.pawn.needs.mood.thoughts.memories.RemoveMemory(th);
						}
					}
				}
				if (!this.recordedTale && curStage.tale != null)
				{
					TaleRecorder.RecordTale(curStage.tale, new object[]
					{
						this.pawn
					});
					this.recordedTale = true;
				}
				if (curStage.destroyPart && this.Part != null && this.Part != this.pawn.RaceProps.body.corePart)
				{
					this.pawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.Part, null, null);
				}
				if (curStage.deathMtbDays > 0f)
				{
					if (this.pawn.IsHashIntervalTick(200) && Rand.MTBEventOccurs(curStage.deathMtbDays, 60000f, 200f))
					{
						bool flag = PawnUtility.ShouldSendNotificationAbout(this.pawn);
						Caravan caravan = this.pawn.GetCaravan();
						this.pawn.Kill(null, null);
						if (flag)
						{
							this.pawn.health.NotifyPlayerOfKilled(null, this, caravan);
						}
					}
				}
			}
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x000AAAF4 File Offset: 0x000A8EF4
		public virtual void PostMake()
		{
			this.Severity = Mathf.Max(this.Severity, this.def.initialSeverity);
			this.causesNoPain = (Rand.Value < this.def.chanceToCauseNoPain);
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x000AAB2B File Offset: 0x000A8F2B
		public virtual void PostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x000AAB2E File Offset: 0x000A8F2E
		public virtual void PostRemoved()
		{
			if (this.def.causesNeed != null && !this.pawn.Dead)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x000AAB61 File Offset: 0x000A8F61
		public virtual void PostTick()
		{
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x000AAB64 File Offset: 0x000A8F64
		public virtual void Tended(float quality, int batchPosition = 0)
		{
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x000AAB67 File Offset: 0x000A8F67
		public virtual void Heal(float amount)
		{
			if (amount > 0f)
			{
				this.Severity -= amount;
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x000AAB99 File Offset: 0x000A8F99
		public virtual void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x000AAB9C File Offset: 0x000A8F9C
		public virtual bool TryMergeWith(Hediff other)
		{
			bool result;
			if (other == null || other.def != this.def || other.Part != this.Part)
			{
				result = false;
			}
			else
			{
				this.Severity += other.Severity;
				this.ageTicks = 0;
				result = true;
			}
			return result;
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x000AABFC File Offset: 0x000A8FFC
		public virtual bool CauseDeathNow()
		{
			return this.def.lethalSeverity >= 0f && this.Severity >= this.def.lethalSeverity;
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x000AAC43 File Offset: 0x000A9043
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x000AAC48 File Offset: 0x000A9048
		public virtual string DebugString()
		{
			string text = "";
			if (!this.Visible)
			{
				text += "hidden\n";
			}
			text = text + "severity: " + this.Severity.ToString("F3") + ((this.Severity < this.def.maxSeverity) ? "" : " (reached max)");
			if (this.TendableNow(false))
			{
				text = text + "\ntend priority: " + this.TendPriority;
			}
			return text.Indented("    ");
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x000AACEC File Offset: 0x000A90EC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.def.defName,
				(this.part == null) ? "" : (" " + this.part.Label),
				" ticksSinceCreation=",
				this.ageTicks,
				")"
			});
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x000AAD70 File Offset: 0x000A9170
		public string GetUniqueLoadID()
		{
			return "Hediff_" + this.loadID;
		}

		// Token: 0x040031CD RID: 12749
		public HediffDef def = null;

		// Token: 0x040031CE RID: 12750
		public int ageTicks = 0;

		// Token: 0x040031CF RID: 12751
		private BodyPartRecord part;

		// Token: 0x040031D0 RID: 12752
		public ThingDef source = null;

		// Token: 0x040031D1 RID: 12753
		public BodyPartGroupDef sourceBodyPartGroup = null;

		// Token: 0x040031D2 RID: 12754
		public HediffDef sourceHediffDef = null;

		// Token: 0x040031D3 RID: 12755
		public int loadID = -1;

		// Token: 0x040031D4 RID: 12756
		protected float severityInt = 0f;

		// Token: 0x040031D5 RID: 12757
		private bool recordedTale = false;

		// Token: 0x040031D6 RID: 12758
		protected bool causesNoPain = false;

		// Token: 0x040031D7 RID: 12759
		private bool visible = false;

		// Token: 0x040031D8 RID: 12760
		public WeakReference<LogEntry> combatLogEntry = null;

		// Token: 0x040031D9 RID: 12761
		public string combatLogText = null;

		// Token: 0x040031DA RID: 12762
		public int temp_partIndexToSetLater = -1;

		// Token: 0x040031DB RID: 12763
		[Unsaved]
		public Pawn pawn = null;
	}
}
