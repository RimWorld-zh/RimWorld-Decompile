using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001B5 RID: 437
	public class ThinkNode_Duty : ThinkNode
	{
		// Token: 0x06000919 RID: 2329 RVA: 0x00055604 File Offset: 0x00053A04
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result;
			if (pawn.GetLord() == null)
			{
				Log.Error(pawn + " doing ThinkNode_Duty with no Lord.", false);
				result = ThinkResult.NoJob;
			}
			else if (pawn.mindState.duty == null)
			{
				Log.Error(pawn + " doing ThinkNode_Duty with no duty.", false);
				result = ThinkResult.NoJob;
			}
			else
			{
				result = this.subNodes[(int)pawn.mindState.duty.def.index].TryIssueJobPackage(pawn, jobParams);
			}
			return result;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00055698 File Offset: 0x00053A98
		protected override void ResolveSubnodes()
		{
			foreach (DutyDef dutyDef in DefDatabase<DutyDef>.AllDefs)
			{
				dutyDef.thinkNode.ResolveSubnodesAndRecur();
				this.subNodes.Add(dutyDef.thinkNode.DeepCopy(true));
			}
		}
	}
}
