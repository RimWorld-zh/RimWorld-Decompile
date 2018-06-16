using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000173 RID: 371
	public class LordJob_PrisonBreak : LordJob
	{
		// Token: 0x0600079F RID: 1951 RVA: 0x0004B1D4 File Offset: 0x000495D4
		public LordJob_PrisonBreak()
		{
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0004B1E4 File Offset: 0x000495E4
		public LordJob_PrisonBreak(IntVec3 groupUpLoc, IntVec3 exitPoint, int sapperThingID)
		{
			this.groupUpLoc = groupUpLoc;
			this.exitPoint = exitPoint;
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0004B20C File Offset: 0x0004960C
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0004B224 File Offset: 0x00049624
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.groupUpLoc);
			lordToil_Travel.maxDanger = Danger.Deadly;
			lordToil_Travel.avoidGridMode = AvoidGridMode.Smart;
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_PrisonerEscape lordToil_PrisonerEscape = new LordToil_PrisonerEscape(this.exitPoint, this.sapperThingID);
			lordToil_PrisonerEscape.avoidGridMode = AvoidGridMode.Smart;
			stateGraph.AddToil(lordToil_PrisonerEscape);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false);
			lordToil_ExitMap.avoidGridMode = AvoidGridMode.Smart;
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Jog, true);
			stateGraph.AddToil(lordToil_ExitMap2);
			Transition transition = new Transition(lordToil_Travel, lordToil_ExitMap2, false, true);
			transition.AddSources(new LordToil[]
			{
				lordToil_PrisonerEscape,
				lordToil_ExitMap
			});
			transition.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(lordToil_ExitMap2, lordToil_ExitMap, false, true);
			transition2.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2);
			Transition transition3 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
			transition3.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition3);
			Transition transition4 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
			transition4.AddTrigger(new Trigger_PawnLost());
			stateGraph.AddTransition(transition4);
			Transition transition5 = new Transition(lordToil_PrisonerEscape, lordToil_PrisonerEscape, true, true);
			transition5.AddTrigger(new Trigger_PawnLost());
			transition5.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
			stateGraph.AddTransition(transition5);
			Transition transition6 = new Transition(lordToil_PrisonerEscape, lordToil_ExitMap, false, true);
			transition6.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition6);
			return stateGraph;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0004B3B0 File Offset: 0x000497B0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.groupUpLoc, "groupUpLoc", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitPoint, "exitPoint", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.sapperThingID, "sapperThingID", -1, false);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0004B40A File Offset: 0x0004980A
		public override void Notify_PawnAdded(Pawn p)
		{
			ReachabilityUtility.ClearCache();
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0004B412 File Offset: 0x00049812
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			ReachabilityUtility.ClearCache();
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0004B41C File Offset: 0x0004981C
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0004B434 File Offset: 0x00049834
		public override bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			Pawn pawn = target as Pawn;
			bool result;
			if (pawn == null)
			{
				result = true;
			}
			else
			{
				MentalStateDef mentalStateDef = pawn.MentalStateDef;
				result = (mentalStateDef == null || !mentalStateDef.escapingPrisonersIgnore);
			}
			return result;
		}

		// Token: 0x04000354 RID: 852
		private IntVec3 groupUpLoc;

		// Token: 0x04000355 RID: 853
		private IntVec3 exitPoint;

		// Token: 0x04000356 RID: 854
		private int sapperThingID = -1;
	}
}
