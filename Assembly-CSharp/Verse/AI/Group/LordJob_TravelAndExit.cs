using System;

namespace Verse.AI.Group
{
	// Token: 0x020009EB RID: 2539
	public class LordJob_TravelAndExit : LordJob
	{
		// Token: 0x04002468 RID: 9320
		private IntVec3 travelDest;

		// Token: 0x0600390F RID: 14607 RVA: 0x001E6624 File Offset: 0x001E4A24
		public LordJob_TravelAndExit()
		{
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x001E662D File Offset: 0x001E4A2D
		public LordJob_TravelAndExit(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x001E6640 File Offset: 0x001E4A40
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

		// Token: 0x06003912 RID: 14610 RVA: 0x001E66B8 File Offset: 0x001E4AB8
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}
	}
}
