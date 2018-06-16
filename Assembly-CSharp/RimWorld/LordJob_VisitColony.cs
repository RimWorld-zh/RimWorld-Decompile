using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000179 RID: 377
	public class LordJob_VisitColony : LordJob
	{
		// Token: 0x060007C2 RID: 1986 RVA: 0x0004C01C File Offset: 0x0004A41C
		public LordJob_VisitColony()
		{
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0004C025 File Offset: 0x0004A425
		public LordJob_VisitColony(Faction faction, IntVec3 chillSpot)
		{
			this.faction = faction;
			this.chillSpot = chillSpot;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0004C03C File Offset: 0x0004A43C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.chillSpot).CreateGraph()).StartingToil;
			stateGraph.StartingToil = startingToil;
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.chillSpot, 28f);
			stateGraph.AddToil(lordToil_DefendPoint);
			LordToil_TakeWoundedGuest lordToil_TakeWoundedGuest = new LordToil_TakeWoundedGuest();
			stateGraph.AddToil(lordToil_TakeWoundedGuest);
			StateGraph stateGraph2 = new LordJob_TravelAndExit(IntVec3.Invalid).CreateGraph();
			LordToil startingToil2 = stateGraph.AttachSubgraph(stateGraph2).StartingToil;
			LordToil target = stateGraph2.lordToils[1];
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Walk, true);
			stateGraph.AddToil(lordToil_ExitMap);
			Transition transition = new Transition(startingToil, startingToil2, false, true);
			transition.AddSources(new LordToil[]
			{
				lordToil_DefendPoint
			});
			transition.AddTrigger(new Trigger_PawnExperiencingDangerousTemperatures());
			transition.AddPreAction(new TransitionAction_Message("MessageVisitorsDangerousTemperature".Translate(new object[]
			{
				this.faction.def.pawnsPlural.CapitalizeFirst(),
				this.faction.Name
			}), null, 1f));
			transition.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(startingToil, lordToil_ExitMap, false, true);
			transition2.AddSources(new LordToil[]
			{
				lordToil_DefendPoint,
				lordToil_TakeWoundedGuest
			});
			transition2.AddSources(stateGraph2.lordToils);
			transition2.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			transition2.AddPreAction(new TransitionAction_Message("MessageVisitorsTrappedLeaving".Translate(new object[]
			{
				this.faction.def.pawnsPlural.CapitalizeFirst(),
				this.faction.Name
			}), null, 1f));
			stateGraph.AddTransition(transition2);
			Transition transition3 = new Transition(lordToil_ExitMap, startingToil2, false, true);
			transition3.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition3.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3);
			Transition transition4 = new Transition(startingToil, lordToil_DefendPoint, false, true);
			transition4.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition4);
			Transition transition5 = new Transition(lordToil_DefendPoint, lordToil_TakeWoundedGuest, false, true);
			transition5.AddTrigger(new Trigger_WoundedGuestPresent());
			transition5.AddPreAction(new TransitionAction_Message("MessageVisitorsTakingWounded".Translate(new object[]
			{
				this.faction.def.pawnsPlural.CapitalizeFirst(),
				this.faction.Name
			}), null, 1f));
			stateGraph.AddTransition(transition5);
			Transition transition6 = new Transition(lordToil_DefendPoint, target, false, true);
			transition6.AddSources(new LordToil[]
			{
				lordToil_TakeWoundedGuest,
				startingToil
			});
			transition6.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition6.AddPreAction(new TransitionAction_SetDefendLocalGroup());
			transition6.AddPostAction(new TransitionAction_WakeAll());
			transition6.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition6);
			Transition transition7 = new Transition(lordToil_DefendPoint, startingToil2, false, true);
			transition7.AddTrigger(new Trigger_TicksPassed((!DebugSettings.instantVisitorsGift) ? Rand.Range(8000, 22000) : 0));
			transition7.AddPreAction(new TransitionAction_Message("VisitorsLeaving".Translate(new object[]
			{
				this.faction.Name
			}), null, 1f));
			transition7.AddPreAction(new TransitionAction_CheckGiveGift());
			transition7.AddPostAction(new TransitionAction_WakeAll());
			transition7.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition7);
			return stateGraph;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0004C3BC File Offset: 0x0004A7BC
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.chillSpot, "chillSpot", default(IntVec3), false);
		}

		// Token: 0x04000363 RID: 867
		private Faction faction;

		// Token: 0x04000364 RID: 868
		private IntVec3 chillSpot;
	}
}
