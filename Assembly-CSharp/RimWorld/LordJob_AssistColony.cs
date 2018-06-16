using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200016A RID: 362
	public class LordJob_AssistColony : LordJob
	{
		// Token: 0x0600076E RID: 1902 RVA: 0x00049EE7 File Offset: 0x000482E7
		public LordJob_AssistColony()
		{
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00049EF0 File Offset: 0x000482F0
		public LordJob_AssistColony(Faction faction, IntVec3 fallbackLocation)
		{
			this.faction = faction;
			this.fallbackLocation = fallbackLocation;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00049F08 File Offset: 0x00048308
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_HuntEnemies lordToil_HuntEnemies = new LordToil_HuntEnemies(this.fallbackLocation);
			stateGraph.AddToil(lordToil_HuntEnemies);
			StateGraph stateGraph2 = new LordJob_Travel(IntVec3.Invalid).CreateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(stateGraph2).StartingToil;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Jog, true);
			stateGraph.AddToil(lordToil_ExitMap2);
			Transition transition = new Transition(lordToil_HuntEnemies, startingToil, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageVisitorsDangerousTemperature".Translate(new object[]
			{
				this.faction.def.pawnsPlural.CapitalizeFirst(),
				this.faction.Name
			}), null, 1f));
			transition.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition.AddTrigger(new Trigger_PawnExperiencingDangerousTemperatures());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(lordToil_HuntEnemies, lordToil_ExitMap2, false, true);
			transition2.AddSource(lordToil_ExitMap);
			transition2.AddSources(stateGraph2.lordToils);
			transition2.AddPreAction(new TransitionAction_Message("MessageVisitorsTrappedLeaving".Translate(new object[]
			{
				this.faction.def.pawnsPlural.CapitalizeFirst(),
				this.faction.Name
			}), null, 1f));
			transition2.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			stateGraph.AddTransition(transition2);
			Transition transition3 = new Transition(lordToil_ExitMap2, startingToil, false, true);
			transition3.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition3.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition3);
			Transition transition4 = new Transition(lordToil_HuntEnemies, startingToil, false, true);
			transition4.AddPreAction(new TransitionAction_Message("MessageFriendlyFightersLeaving".Translate(new object[]
			{
				this.faction.def.pawnsPlural.CapitalizeFirst(),
				this.faction.Name
			}), null, 1f));
			transition4.AddTrigger(new Trigger_TicksPassed(25000));
			transition4.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition4);
			Transition transition5 = new Transition(startingToil, lordToil_ExitMap, false, true);
			transition5.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition5);
			return stateGraph;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0004A148 File Offset: 0x00048548
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.fallbackLocation, "fallbackLocation", default(IntVec3), false);
		}

		// Token: 0x04000339 RID: 825
		private Faction faction;

		// Token: 0x0400033A RID: 826
		private IntVec3 fallbackLocation;
	}
}
