using System;

namespace Verse.AI.Group
{
	// Token: 0x020009EC RID: 2540
	public class LordJob_TravelAndExit : LordJob
	{
		// Token: 0x04002478 RID: 9336
		private IntVec3 travelDest;

		// Token: 0x06003910 RID: 14608 RVA: 0x001E6950 File Offset: 0x001E4D50
		public LordJob_TravelAndExit()
		{
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x001E6959 File Offset: 0x001E4D59
		public LordJob_TravelAndExit(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x001E696C File Offset: 0x001E4D6C
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

		// Token: 0x06003913 RID: 14611 RVA: 0x001E69E4 File Offset: 0x001E4DE4
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}
	}
}
