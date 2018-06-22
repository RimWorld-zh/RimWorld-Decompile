using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E9 RID: 2537
	public class LordJob_TravelAndExit : LordJob
	{
		// Token: 0x0600390B RID: 14603 RVA: 0x001E64F8 File Offset: 0x001E48F8
		public LordJob_TravelAndExit()
		{
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x001E6501 File Offset: 0x001E4901
		public LordJob_TravelAndExit(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x001E6514 File Offset: 0x001E4914
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

		// Token: 0x0600390E RID: 14606 RVA: 0x001E658C File Offset: 0x001E498C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x04002467 RID: 9319
		private IntVec3 travelDest;
	}
}
