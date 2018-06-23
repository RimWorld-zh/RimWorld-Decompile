using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200016E RID: 366
	public class LordJob_FormAndSendCaravan : LordJob
	{
		// Token: 0x0400033F RID: 831
		public List<TransferableOneWay> transferables;

		// Token: 0x04000340 RID: 832
		private IntVec3 meetingPoint;

		// Token: 0x04000341 RID: 833
		private IntVec3 exitSpot;

		// Token: 0x04000342 RID: 834
		private int startingTile;

		// Token: 0x04000343 RID: 835
		private int destinationTile;

		// Token: 0x04000344 RID: 836
		private bool caravanSent;

		// Token: 0x04000345 RID: 837
		private LordToil gatherAnimals;

		// Token: 0x04000346 RID: 838
		private LordToil gatherAnimals_pause;

		// Token: 0x04000347 RID: 839
		private LordToil gatherItems;

		// Token: 0x04000348 RID: 840
		private LordToil gatherItems_pause;

		// Token: 0x04000349 RID: 841
		private LordToil gatherSlaves;

		// Token: 0x0400034A RID: 842
		private LordToil gatherSlaves_pause;

		// Token: 0x0400034B RID: 843
		private LordToil leave;

		// Token: 0x0400034C RID: 844
		private LordToil leave_pause;

		// Token: 0x0400034D RID: 845
		public const float CustomWakeThreshold = 0.5f;

		// Token: 0x0600077E RID: 1918 RVA: 0x0004A65F File Offset: 0x00048A5F
		public LordJob_FormAndSendCaravan()
		{
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0004A668 File Offset: 0x00048A68
		public LordJob_FormAndSendCaravan(List<TransferableOneWay> transferables, IntVec3 meetingPoint, IntVec3 exitSpot, int startingTile, int destinationTile)
		{
			this.transferables = transferables;
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
			this.startingTile = startingTile;
			this.destinationTile = destinationTile;
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x0004A698 File Offset: 0x00048A98
		public bool GatheringItemsNow
		{
			get
			{
				return this.lord.CurLordToil == this.gatherItems;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0004A6C0 File Offset: 0x00048AC0
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x0004A6D8 File Offset: 0x00048AD8
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0004A6F0 File Offset: 0x00048AF0
		public string Status
		{
			get
			{
				LordToil curLordToil = this.lord.CurLordToil;
				string result;
				if (curLordToil == this.gatherAnimals)
				{
					result = "FormingCaravanStatus_GatheringAnimals".Translate();
				}
				else if (curLordToil == this.gatherAnimals_pause)
				{
					result = "FormingCaravanStatus_GatheringAnimals_Pause".Translate();
				}
				else if (curLordToil == this.gatherItems)
				{
					result = "FormingCaravanStatus_GatheringItems".Translate();
				}
				else if (curLordToil == this.gatherItems_pause)
				{
					result = "FormingCaravanStatus_GatheringItems_Pause".Translate();
				}
				else if (curLordToil == this.gatherSlaves)
				{
					result = "FormingCaravanStatus_GatheringSlaves".Translate();
				}
				else if (curLordToil == this.gatherSlaves_pause)
				{
					result = "FormingCaravanStatus_GatheringSlaves_Pause".Translate();
				}
				else if (curLordToil == this.leave)
				{
					result = "FormingCaravanStatus_Leaving".Translate();
				}
				else if (curLordToil == this.leave_pause)
				{
					result = "FormingCaravanStatus_Leaving_Pause".Translate();
				}
				else
				{
					result = "FormingCaravanStatus_Waiting".Translate();
				}
				return result;
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0004A7FC File Offset: 0x00048BFC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			this.gatherAnimals = new LordToil_PrepareCaravan_GatherAnimals(this.meetingPoint);
			stateGraph.AddToil(this.gatherAnimals);
			this.gatherAnimals_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherAnimals_pause);
			this.gatherItems = new LordToil_PrepareCaravan_GatherItems(this.meetingPoint);
			stateGraph.AddToil(this.gatherItems);
			this.gatherItems_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherItems_pause);
			this.gatherSlaves = new LordToil_PrepareCaravan_GatherSlaves(this.meetingPoint);
			stateGraph.AddToil(this.gatherSlaves);
			this.gatherSlaves_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherSlaves_pause);
			LordToil_PrepareCaravan_Wait lordToil_PrepareCaravan_Wait = new LordToil_PrepareCaravan_Wait(this.meetingPoint);
			stateGraph.AddToil(lordToil_PrepareCaravan_Wait);
			LordToil_PrepareCaravan_Pause lordToil_PrepareCaravan_Pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(lordToil_PrepareCaravan_Pause);
			this.leave = new LordToil_PrepareCaravan_Leave(this.exitSpot);
			stateGraph.AddToil(this.leave);
			this.leave_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.leave_pause);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(this.gatherAnimals, this.gatherItems, false, true);
			transition.AddTrigger(new Trigger_Memo("AllAnimalsGathered"));
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(this.gatherItems, this.gatherSlaves, false, true);
			transition2.AddTrigger(new Trigger_Memo("AllItemsGathered"));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2);
			Transition transition3 = new Transition(this.gatherSlaves, lordToil_PrepareCaravan_Wait, false, true);
			transition3.AddTrigger(new Trigger_Memo("AllSlavesGathered"));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3);
			Transition transition4 = new Transition(lordToil_PrepareCaravan_Wait, this.leave, false, true);
			transition4.AddTrigger(new Trigger_NoPawnsVeryTiredAndSleeping(0f));
			transition4.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition4);
			Transition transition5 = new Transition(this.leave, lordToil_End, false, true);
			transition5.AddTrigger(new Trigger_Memo("ReadyToExitMap"));
			transition5.AddPreAction(new TransitionAction_Custom(new Action(this.SendCaravan)));
			stateGraph.AddTransition(transition5);
			Transition transition6 = this.PauseTransition(this.gatherAnimals, this.gatherAnimals_pause);
			stateGraph.AddTransition(transition6);
			Transition transition7 = this.UnpauseTransition(this.gatherAnimals_pause, this.gatherAnimals);
			stateGraph.AddTransition(transition7);
			Transition transition8 = this.PauseTransition(this.gatherItems, this.gatherItems_pause);
			stateGraph.AddTransition(transition8);
			Transition transition9 = this.UnpauseTransition(this.gatherItems_pause, this.gatherItems);
			stateGraph.AddTransition(transition9);
			Transition transition10 = this.PauseTransition(this.gatherSlaves, this.gatherSlaves_pause);
			stateGraph.AddTransition(transition10);
			Transition transition11 = this.UnpauseTransition(this.gatherSlaves_pause, this.gatherSlaves);
			stateGraph.AddTransition(transition11);
			Transition transition12 = this.PauseTransition(this.leave, this.leave_pause);
			stateGraph.AddTransition(transition12);
			Transition transition13 = this.UnpauseTransition(this.leave_pause, this.leave);
			stateGraph.AddTransition(transition13);
			Transition transition14 = this.PauseTransition(lordToil_PrepareCaravan_Wait, lordToil_PrepareCaravan_Pause);
			stateGraph.AddTransition(transition14);
			Transition transition15 = this.UnpauseTransition(lordToil_PrepareCaravan_Pause, lordToil_PrepareCaravan_Wait);
			stateGraph.AddTransition(transition15);
			return stateGraph;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0004AB50 File Offset: 0x00048F50
		public override string GetReport()
		{
			return "LordReportFormingCaravan".Translate();
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0004AB70 File Offset: 0x00048F70
		private Transition PauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationPaused".Translate(), MessageTypeDefOf.NegativeEvent, () => this.lord.ownedPawns.FirstOrDefault((Pawn x) => x.InMentalState), null, 1f));
			transition.AddTrigger(new Trigger_MentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0004ABD4 File Offset: 0x00048FD4
		private Transition UnpauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationUnpaused".Translate(), MessageTypeDefOf.SilentInput, null, 1f));
			transition.AddTrigger(new Trigger_NoMentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0004AC2C File Offset: 0x0004902C
		public override void ExposeData()
		{
			Scribe_Collections.Look<TransferableOneWay>(ref this.transferables, "transferables", LookMode.Deep, new object[0]);
			Scribe_Values.Look<IntVec3>(ref this.meetingPoint, "meetingPoint", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitSpot, "exitSpot", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.startingTile, "startingTile", 0, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0004ACA9 File Offset: 0x000490A9
		private void SendCaravan()
		{
			this.caravanSent = true;
			CaravanFormingUtility.FormAndCreateCaravan(this.lord.ownedPawns, this.lord.faction, base.Map.Tile, this.startingTile, this.destinationTile);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0004ACE5 File Offset: 0x000490E5
		public override void Notify_PawnAdded(Pawn p)
		{
			ReachabilityUtility.ClearCache();
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0004ACED File Offset: 0x000490ED
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			ReachabilityUtility.ClearCache();
			if (!this.caravanSent)
			{
				CaravanFormingUtility.RemovePawnFromCaravan(p, this.lord);
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0004AD0C File Offset: 0x0004910C
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}
	}
}
