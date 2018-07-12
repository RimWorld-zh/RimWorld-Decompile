using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StoryWatcher_RampUp : IExposable
	{
		private float rampDays = 0f;

		private const int UpdateInterval = 30000;

		public StoryWatcher_RampUp()
		{
		}

		public float TotalThreatPointsFactor
		{
			get
			{
				return Find.Storyteller.def.pointsFactorFromRampDays.Evaluate(this.rampDays);
			}
		}

		public float RampDays
		{
			get
			{
				return this.rampDays;
			}
		}

		private int Population
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>();
			}
		}

		public void Notify_ColonyPawnDowned(Pawn p, DamageInfo? dinfo)
		{
			if (p.RaceProps.Humanlike && dinfo != null && dinfo.Value.Def.externalViolence)
			{
				float rampDaysLoss = Find.Storyteller.def.rampDaysLossFromColonistViolentlyDownedByPopulation.Evaluate((float)this.Population);
				if (DebugViewSettings.logRampUp)
				{
					Log.Message(string.Concat(new object[]
					{
						"RampUp: Colony pawn downed (",
						p,
						" by ",
						dinfo,
						"). Loss: ",
						rampDaysLoss.ToString("F1"),
						" from ",
						this.rampDays.ToString("F1")
					}), false);
				}
				this.LoseRampDays(rampDaysLoss);
			}
		}

		public void Notify_ColonyPawnDied(Pawn p)
		{
			if (p.RaceProps.Humanlike)
			{
				int num = this.Population - 1;
				float rampDaysLoss = Find.Storyteller.def.rampDaysLossFromColonistDiedByPostPopulation.Evaluate((float)num);
				if (DebugViewSettings.logRampUp)
				{
					Log.Message(string.Concat(new object[]
					{
						"RampUp: Colony pawn died (",
						p,
						"). Loss: ",
						rampDaysLoss.ToString("F1"),
						" from ",
						this.rampDays.ToString("F1")
					}), false);
				}
				this.LoseRampDays(rampDaysLoss);
			}
		}

		private void LoseRampDays(float rampDaysLoss)
		{
			this.rampDays = Mathf.Max(0f, this.rampDays - rampDaysLoss);
		}

		public void RampUpWatcherTick()
		{
			if (Find.TickManager.TicksGame % 30000 == 0)
			{
				this.rampDays += 0.5f;
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.rampDays, "rampDays", 0f, false);
		}

		public void Debug_RampUpNow(float days)
		{
			this.rampDays += days;
		}
	}
}
