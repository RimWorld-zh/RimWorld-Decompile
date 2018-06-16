using System;

namespace Verse.AI.Group
{
	// Token: 0x020009ED RID: 2541
	public class LordJob_TravelAndExit : LordJob
	{
		// Token: 0x0600390F RID: 14607 RVA: 0x001E61E4 File Offset: 0x001E45E4
		public LordJob_TravelAndExit()
		{
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x001E61ED File Offset: 0x001E45ED
		public LordJob_TravelAndExit(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x001E6200 File Offset: 0x001E4600
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.travelDest).CreateGraph()).StartingToil;
			stateGraph.StartingToil = startingToil;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false);
			stateGraph.AddToil(lordToil_ExitMap);
			stateGraph.AddTransition(new Transition(startingToil, lordToil_ExitMap, false, true)
			{
				triggers = 
				{
					new Trigger_Memo("TravelArrived")
				}
			});
			return stateGraph;
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x001E6278 File Offset: 0x001E4678
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x0400246C RID: 9324
		private IntVec3 travelDest;
	}
}
