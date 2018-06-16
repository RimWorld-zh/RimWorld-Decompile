using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001B5 RID: 437
	public class ThinkNode_Duty : ThinkNode
	{
		// Token: 0x0600091B RID: 2331 RVA: 0x000555F0 File Offset: 0x000539F0
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

		// Token: 0x0600091C RID: 2332 RVA: 0x00055684 File Offset: 0x00053A84
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
