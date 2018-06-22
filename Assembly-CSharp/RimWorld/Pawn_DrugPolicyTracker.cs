using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E8 RID: 1256
	public class Pawn_DrugPolicyTracker : IExposable
	{
		// Token: 0x06001673 RID: 5747 RVA: 0x000C71DC File Offset: 0x000C55DC
		public Pawn_DrugPolicyTracker()
		{
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x000C71F0 File Offset: 0x000C55F0
		public Pawn_DrugPolicyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x000C720C File Offset: 0x000C560C
		// (set) Token: 0x06001676 RID: 5750 RVA: 0x000C7247 File Offset: 0x000C5647
		public DrugPolicy CurrentPolicy
		{
			get
			{
				if (this.curPolicy == null)
				{
					this.curPolicy = Current.Game.drugPolicyDatabase.DefaultDrugPolicy();
				}
				return this.curPolicy;
			}
			set
			{
				if (this.curPolicy != value)
				{
					this.curPolicy = value;
				}
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001677 RID: 5751 RVA: 0x000C7264 File Offset: 0x000C5664
		private float DayPercentNotSleeping
		{
			get
			{
				float result;
				if (this.pawn.IsCaravanMember())
				{
					result = Mathf.InverseLerp(6f, 22f, GenLocalDate.HourFloat(this.pawn));
				}
				else if (this.pawn.timetable == null)
				{
					result = GenLocalDate.DayPercent(this.pawn);
				}
				else
				{
					float hoursPerDayNotSleeping = this.HoursPerDayNotSleeping;
					if (hoursPerDayNotSleeping == 0f)
					{
						result = 1f;
					}
					else
					{
						float num = 0f;
						int num2 = GenLocalDate.HourOfDay(this.pawn);
						for (int i = 0; i < num2; i++)
						{
							if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
							{
								num += 1f;
							}
						}
						TimeAssignmentDef currentAssignment = this.pawn.timetable.CurrentAssignment;
						if (currentAssignment != TimeAssignmentDefOf.Sleep)
						{
							float num3 = (float)(Find.TickManager.TicksAbs % 2500) / 2500f;
							num += num3;
						}
						result = num / hoursPerDayNotSleeping;
					}
				}
				return result;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x000C737C File Offset: 0x000C577C
		private float HoursPerDayNotSleeping
		{
			get
			{
				float result;
				if (this.pawn.IsCaravanMember())
				{
					result = 16f;
				}
				else
				{
					int num = 0;
					for (int i = 0; i < 24; i++)
					{
						if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
						{
							num++;
						}
					}
					result = (float)num;
				}
				return result;
			}
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x000C73EA File Offset: 0x000C57EA
		public void ExposeData()
		{
			Scribe_References.Look<DrugPolicy>(ref this.curPolicy, "curAssignedDrugs", false);
			Scribe_Collections.Look<DrugTakeRecord>(ref this.drugTakeRecords, "drugTakeRecords", LookMode.Deep, new object[0]);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x000C7418 File Offset: 0x000C5818
		public bool HasEverTaken(ThingDef drug)
		{
			bool result;
			if (!drug.IsDrug)
			{
				Log.Warning(drug + " is not a drug.", false);
				result = false;
			}
			else
			{
				result = this.drugTakeRecords.Any((DrugTakeRecord x) => x.drug == drug);
			}
			return result;
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x000C7480 File Offset: 0x000C5880
		public bool AllowedToTakeScheduledEver(ThingDef thingDef)
		{
			bool result;
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.", false);
				result = false;
			}
			else if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef, false);
				result = false;
			}
			else
			{
				DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
				result = (drugPolicyEntry.allowScheduled && (!thingDef.IsNonMedicalDrug || !this.pawn.IsTeetotaler()));
			}
			return result;
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x000C7520 File Offset: 0x000C5920
		public bool AllowedToTakeScheduledNow(ThingDef thingDef)
		{
			bool result;
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.", false);
				result = false;
			}
			else if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef, false);
				result = false;
			}
			else if (!this.AllowedToTakeScheduledEver(thingDef))
			{
				result = false;
			}
			else
			{
				DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
				if (drugPolicyEntry.onlyIfMoodBelow < 1f && this.pawn.needs.mood != null && this.pawn.needs.mood.CurLevelPercentage >= drugPolicyEntry.onlyIfMoodBelow)
				{
					result = false;
				}
				else if (drugPolicyEntry.onlyIfJoyBelow < 1f && this.pawn.needs.joy != null && this.pawn.needs.joy.CurLevelPercentage >= drugPolicyEntry.onlyIfJoyBelow)
				{
					result = false;
				}
				else
				{
					DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == thingDef);
					if (drugTakeRecord != null)
					{
						if (drugPolicyEntry.daysFrequency < 1f)
						{
							int num = Mathf.RoundToInt(1f / drugPolicyEntry.daysFrequency);
							if (drugTakeRecord.TimesTakenThisDay >= num)
							{
								return false;
							}
						}
						else
						{
							int num2 = Mathf.Abs(GenDate.DaysPassed - drugTakeRecord.LastTakenDays);
							int num3 = Mathf.RoundToInt(drugPolicyEntry.daysFrequency);
							if (num2 < num3)
							{
								return false;
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x000C76F4 File Offset: 0x000C5AF4
		public bool ShouldTryToTakeScheduledNow(ThingDef ingestible)
		{
			bool result;
			if (!ingestible.IsDrug)
			{
				result = false;
			}
			else if (!this.AllowedToTakeScheduledNow(ingestible))
			{
				result = false;
			}
			else
			{
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
				if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.5f && this.CanCauseOverdose(ingestible))
				{
					int num = this.LastTicksWhenTakenDrugWhichCanCauseOverdose();
					if (Find.TickManager.TicksGame - num < 1250)
					{
						return false;
					}
				}
				DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == ingestible);
				if (drugTakeRecord == null)
				{
					result = true;
				}
				else
				{
					DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[ingestible];
					if (drugPolicyEntry.daysFrequency < 1f)
					{
						int num2 = Mathf.RoundToInt(1f / drugPolicyEntry.daysFrequency);
						float num3 = 1f / (float)(num2 + 1);
						int num4 = 0;
						float dayPercentNotSleeping = this.DayPercentNotSleeping;
						for (int i = 0; i < num2; i++)
						{
							if (dayPercentNotSleeping > (float)(i + 1) * num3 - num3 * 0.5f)
							{
								num4++;
							}
						}
						result = (drugTakeRecord.TimesTakenThisDay < num4 && (drugTakeRecord.TimesTakenThisDay == 0 || (float)(Find.TickManager.TicksGame - drugTakeRecord.lastTakenTicks) / (this.HoursPerDayNotSleeping * 2500f) >= 0.6f * num3));
					}
					else
					{
						float dayPercentNotSleeping2 = this.DayPercentNotSleeping;
						Rand.PushState();
						Rand.Seed = Gen.HashCombineInt(GenDate.DaysPassed, this.pawn.thingIDNumber);
						bool flag = dayPercentNotSleeping2 >= Rand.Range(0.1f, 0.35f);
						Rand.PopState();
						result = flag;
					}
				}
			}
			return result;
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x000C7904 File Offset: 0x000C5D04
		public void Notify_DrugIngested(Thing drug)
		{
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == drug.def);
			if (drugTakeRecord == null)
			{
				drugTakeRecord = new DrugTakeRecord();
				drugTakeRecord.drug = drug.def;
				this.drugTakeRecords.Add(drugTakeRecord);
			}
			drugTakeRecord.lastTakenTicks = Find.TickManager.TicksGame;
			drugTakeRecord.TimesTakenThisDay++;
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x000C7980 File Offset: 0x000C5D80
		private int LastTicksWhenTakenDrugWhichCanCauseOverdose()
		{
			int num = -999999;
			for (int i = 0; i < this.drugTakeRecords.Count; i++)
			{
				if (this.CanCauseOverdose(this.drugTakeRecords[i].drug))
				{
					num = Mathf.Max(num, this.drugTakeRecords[i].lastTakenTicks);
				}
			}
			return num;
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x000C79F4 File Offset: 0x000C5DF4
		private bool CanCauseOverdose(ThingDef drug)
		{
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			return compProperties != null && compProperties.CanCauseOverdose;
		}

		// Token: 0x04000D1E RID: 3358
		public Pawn pawn;

		// Token: 0x04000D1F RID: 3359
		private DrugPolicy curPolicy;

		// Token: 0x04000D20 RID: 3360
		private List<DrugTakeRecord> drugTakeRecords = new List<DrugTakeRecord>();

		// Token: 0x04000D21 RID: 3361
		private const float DangerousDrugOverdoseSeverity = 0.5f;
	}
}
