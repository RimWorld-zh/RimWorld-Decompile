using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class ThinkNode_Duty : ThinkNode
	{
		public ThinkNode_Duty()
		{
		}

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
