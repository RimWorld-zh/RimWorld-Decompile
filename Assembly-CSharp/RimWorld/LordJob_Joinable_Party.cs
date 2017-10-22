using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordJob_Joinable_Party : LordJob_VoluntarilyJoinable
	{
		private IntVec3 spot;

		private Trigger_TicksPassed timeoutTrigger;

		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		public LordJob_Joinable_Party()
		{
		}

		public LordJob_Joinable_Party(IntVec3 spot)
		{
			this.spot = spot;
		}

		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Party lordToil_Party = new LordToil_Party(this.spot);
			stateGraph.AddToil(lordToil_Party);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_Party, lordToil_End);
			transition.AddTrigger(new Trigger_TickCondition((Func<bool>)(() => this.ShouldBeCalledOff())));
			transition.AddTrigger(new Trigger_PawnLostViolently());
			transition.AddPreAction(new TransitionAction_Message("MessagePartyCalledOff".Translate(), MessageSound.Negative, new TargetInfo(this.spot, base.Map, false)));
			stateGraph.AddTransition(transition);
			this.timeoutTrigger = new Trigger_TicksPassed(Rand.RangeInclusive(5000, 15000));
			Transition transition2 = new Transition(lordToil_Party, lordToil_End);
			transition2.AddTrigger(this.timeoutTrigger);
			transition2.AddPreAction(new TransitionAction_Message("MessagePartyFinished".Translate(), MessageSound.Negative, new TargetInfo(this.spot, base.Map, false)));
			stateGraph.AddTransition(transition2);
			return stateGraph;
		}

		private bool ShouldBeCalledOff()
		{
			if (!PartyUtility.AcceptableGameConditionsToContinueParty(base.Map))
			{
				return true;
			}
			if (!this.spot.Roofed(base.Map) && !JoyUtility.EnjoyableOutsideNow(base.Map, null))
			{
				return true;
			}
			return false;
		}

		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (this.IsInvited(p))
			{
				if (!PartyUtility.ShouldPawnKeepPartying(p))
				{
					return 0f;
				}
				if (!base.lord.ownedPawns.Contains(p) && this.IsPartyAboutToEnd())
				{
					return 0f;
				}
				return VoluntarilyJoinableLordJobJoinPriorities.PartyGuest;
			}
			return 0f;
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
		}

		public override string GetReport()
		{
			return "LordReportAttendingParty".Translate();
		}

		private bool IsPartyAboutToEnd()
		{
			if (this.timeoutTrigger.TicksLeft < 1200)
			{
				return true;
			}
			return false;
		}

		private bool IsInvited(Pawn p)
		{
			return p.Faction == base.lord.faction;
		}
	}
}
