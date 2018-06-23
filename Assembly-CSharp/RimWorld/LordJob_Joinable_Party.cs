using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200017C RID: 380
	public class LordJob_Joinable_Party : LordJob_VoluntarilyJoinable
	{
		// Token: 0x0400036A RID: 874
		private IntVec3 spot;

		// Token: 0x0400036B RID: 875
		private Pawn organizer;

		// Token: 0x0400036C RID: 876
		private Trigger_TicksPassed timeoutTrigger;

		// Token: 0x060007E3 RID: 2019 RVA: 0x0004D32D File Offset: 0x0004B72D
		public LordJob_Joinable_Party()
		{
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0004D336 File Offset: 0x0004B736
		public LordJob_Joinable_Party(IntVec3 spot, Pawn organizer)
		{
			this.spot = spot;
			this.organizer = organizer;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0004D350 File Offset: 0x0004B750
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0004D368 File Offset: 0x0004B768
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0004D384 File Offset: 0x0004B784
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Party lordToil_Party = new LordToil_Party(this.spot, 600);
			stateGraph.AddToil(lordToil_Party);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_Party, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_TickCondition(() => this.ShouldBeCalledOff(), 1));
			transition.AddTrigger(new Trigger_PawnKilled());
			transition.AddPreAction(new TransitionAction_Message("MessagePartyCalledOff".Translate(), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition);
			this.timeoutTrigger = new Trigger_TicksPassed(Rand.RangeInclusive(5000, 15000));
			Transition transition2 = new Transition(lordToil_Party, lordToil_End, false, true);
			transition2.AddTrigger(this.timeoutTrigger);
			transition2.AddPreAction(new TransitionAction_Message("MessagePartyFinished".Translate(), MessageTypeDefOf.SituationResolved, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition2);
			return stateGraph;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0004D49C File Offset: 0x0004B89C
		private bool ShouldBeCalledOff()
		{
			return !PartyUtility.AcceptableGameConditionsToContinueParty(base.Map) || (!this.spot.Roofed(base.Map) && !JoyUtility.EnjoyableOutsideNow(base.Map, null));
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0004D4F8 File Offset: 0x0004B8F8
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			float result;
			if (this.IsInvited(p))
			{
				if (!PartyUtility.ShouldPawnKeepPartying(p))
				{
					result = 0f;
				}
				else if (this.spot.IsForbidden(p))
				{
					result = 0f;
				}
				else
				{
					if (!this.lord.ownedPawns.Contains(p))
					{
						if (this.IsPartyAboutToEnd())
						{
							return 0f;
						}
					}
					result = VoluntarilyJoinableLordJobJoinPriorities.PartyGuest;
				}
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0004D58C File Offset: 0x0004B98C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
			Scribe_References.Look<Pawn>(ref this.organizer, "organizer", false);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0004D5C8 File Offset: 0x0004B9C8
		public override string GetReport()
		{
			return "LordReportAttendingParty".Translate();
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0004D5E8 File Offset: 0x0004B9E8
		private bool IsPartyAboutToEnd()
		{
			return this.timeoutTrigger.TicksLeft < 1200;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0004D61C File Offset: 0x0004BA1C
		private bool IsInvited(Pawn p)
		{
			return this.lord.faction != null && p.Faction == this.lord.faction;
		}
	}
}
