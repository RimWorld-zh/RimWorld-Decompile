using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Pawn_DrugPolicyTracker : IExposable
	{
		private const float DangerousDrugOverdoseSeverity = 0.5f;

		public Pawn pawn;

		private DrugPolicy curPolicy;

		private List<DrugTakeRecord> drugTakeRecords = new List<DrugTakeRecord>();

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

		private float DayPercentNotSleeping
		{
			get
			{
				if (this.pawn.IsCaravanMember())
				{
					return Mathf.InverseLerp(6f, 22f, GenLocalDate.HourFloat(this.pawn));
				}
				if (this.pawn.timetable == null)
				{
					return GenLocalDate.DayPercent(this.pawn);
				}
				float hoursPerDayNotSleeping = this.HoursPerDayNotSleeping;
				if (hoursPerDayNotSleeping == 0.0)
				{
					return 1f;
				}
				float num = 0f;
				int num2 = GenLocalDate.HourOfDay(this.pawn);
				for (int num3 = 0; num3 < num2; num3++)
				{
					if (this.pawn.timetable.times[num3] != TimeAssignmentDefOf.Sleep)
					{
						num = (float)(num + 1.0);
					}
				}
				TimeAssignmentDef currentAssignment = this.pawn.timetable.CurrentAssignment;
				if (currentAssignment != TimeAssignmentDefOf.Sleep)
				{
					float num4 = (float)((float)(Find.TickManager.TicksAbs % 2500) / 2500.0);
					num += num4;
				}
				return num / hoursPerDayNotSleeping;
			}
		}

		private float HoursPerDayNotSleeping
		{
			get
			{
				if (this.pawn.IsCaravanMember())
				{
					return 16f;
				}
				int num = 0;
				for (int i = 0; i < 24; i++)
				{
					if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
					{
						num++;
					}
				}
				return (float)num;
			}
		}

		public Pawn_DrugPolicyTracker()
		{
		}

		public Pawn_DrugPolicyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void ExposeData()
		{
			Scribe_References.Look<DrugPolicy>(ref this.curPolicy, "curAssignedDrugs", false);
			Scribe_Collections.Look<DrugTakeRecord>(ref this.drugTakeRecords, "drugTakeRecords", LookMode.Deep, new object[0]);
		}

		public bool HasEverTaken(ThingDef drug)
		{
			if (!drug.IsDrug)
			{
				Log.Warning(drug + " is not a drug.");
				return false;
			}
			return this.drugTakeRecords.Any((Predicate<DrugTakeRecord>)((DrugTakeRecord x) => x.drug == drug));
		}

		public bool AllowedToTakeScheduledEver(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef);
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			if (!drugPolicyEntry.allowScheduled)
			{
				return false;
			}
			if (thingDef.IsPleasureDrug && this.pawn.IsTeetotaler())
			{
				return false;
			}
			return true;
		}

		public bool AllowedToTakeScheduledNow(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef);
				return false;
			}
			if (!this.AllowedToTakeScheduledEver(thingDef))
			{
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			if (drugPolicyEntry.onlyIfMoodBelow < 1.0 && this.pawn.needs.mood != null && this.pawn.needs.mood.CurLevelPercentage >= drugPolicyEntry.onlyIfMoodBelow)
			{
				return false;
			}
			if (drugPolicyEntry.onlyIfJoyBelow < 1.0 && this.pawn.needs.joy != null && this.pawn.needs.joy.CurLevelPercentage >= drugPolicyEntry.onlyIfJoyBelow)
			{
				return false;
			}
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((Predicate<DrugTakeRecord>)((DrugTakeRecord x) => x.drug == thingDef));
			if (drugTakeRecord != null)
			{
				if (drugPolicyEntry.daysFrequency < 1.0)
				{
					int num = Mathf.RoundToInt((float)(1.0 / drugPolicyEntry.daysFrequency));
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
			return true;
		}

		public bool ShouldTryToTakeScheduledNow(ThingDef ingestible)
		{
			if (!ingestible.IsDrug)
			{
				return false;
			}
			if (!this.AllowedToTakeScheduledNow(ingestible))
			{
				return false;
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
			if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.5 && this.CanCauseOverdose(ingestible))
			{
				int num = this.LastTicksWhenTakenDrugWhichCanCauseOverdose();
				if (Find.TickManager.TicksGame - num < 1250)
				{
					return false;
				}
			}
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((Predicate<DrugTakeRecord>)((DrugTakeRecord x) => x.drug == ingestible));
			if (drugTakeRecord == null)
			{
				return true;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[ingestible];
			if (drugPolicyEntry.daysFrequency < 1.0)
			{
				int num2 = Mathf.RoundToInt((float)(1.0 / drugPolicyEntry.daysFrequency));
				float num3 = (float)(1.0 / (float)(num2 + 1));
				int num4 = 0;
				float dayPercentNotSleeping = this.DayPercentNotSleeping;
				for (int num5 = 0; num5 < num2; num5++)
				{
					if (dayPercentNotSleeping > (float)(num5 + 1) * num3 - num3 * 0.5)
					{
						num4++;
					}
				}
				if (drugTakeRecord.TimesTakenThisDay >= num4)
				{
					return false;
				}
				if (drugTakeRecord.TimesTakenThisDay != 0 && (float)(Find.TickManager.TicksGame - drugTakeRecord.lastTakenTicks) / (this.HoursPerDayNotSleeping * 2500.0) < 0.60000002384185791 * num3)
				{
					return false;
				}
				return true;
			}
			float dayPercentNotSleeping2 = this.DayPercentNotSleeping;
			Rand.PushState();
			Rand.Seed = Gen.HashCombineInt(GenDate.DaysPassed, this.pawn.thingIDNumber);
			bool result = dayPercentNotSleeping2 >= Rand.Range(0.1f, 0.35f);
			Rand.PopState();
			return result;
		}

		public void Notify_DrugIngested(Thing drug)
		{
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((Predicate<DrugTakeRecord>)((DrugTakeRecord x) => x.drug == drug.def));
			if (drugTakeRecord == null)
			{
				drugTakeRecord = new DrugTakeRecord();
				drugTakeRecord.drug = drug.def;
				this.drugTakeRecords.Add(drugTakeRecord);
			}
			drugTakeRecord.lastTakenTicks = Find.TickManager.TicksGame;
			drugTakeRecord.TimesTakenThisDay++;
		}

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

		private bool CanCauseOverdose(ThingDef drug)
		{
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			if (compProperties == null)
			{
				return false;
			}
			return compProperties.CanCauseOverdose;
		}
	}
}
