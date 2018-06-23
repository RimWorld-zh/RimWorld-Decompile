using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001B6 RID: 438
	public class ThinkNode_DutyConstant : ThinkNode
	{
		// Token: 0x040003D8 RID: 984
		private DefMap<DutyDef, int> dutyDefToSubNode;

		// Token: 0x0600091C RID: 2332 RVA: 0x00055718 File Offset: 0x00053B18
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result;
			if (pawn.GetLord() == null)
			{
				Log.Error(pawn + " doing ThinkNode_DutyConstant with no Lord.", false);
				result = ThinkResult.NoJob;
			}
			else if (pawn.mindState.duty == null)
			{
				Log.Error(pawn + " doing ThinkNode_DutyConstant with no duty.", false);
				result = ThinkResult.NoJob;
			}
			else
			{
				if (this.dutyDefToSubNode == null)
				{
					Log.Error(pawn + " has null dutyDefToSubNode. Recovering by calling ResolveSubnodes() (though that should have been called already).", false);
					this.ResolveSubnodes();
				}
				int num = this.dutyDefToSubNode[pawn.mindState.duty.def];
				if (num < 0)
				{
					result = ThinkResult.NoJob;
				}
				else
				{
					result = this.subNodes[num].TryIssueJobPackage(pawn, jobParams);
				}
			}
			return result;
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x000557E8 File Offset: 0x00053BE8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_DutyConstant thinkNode_DutyConstant = (ThinkNode_DutyConstant)base.DeepCopy(resolve);
			if (this.dutyDefToSubNode != null)
			{
				thinkNode_DutyConstant.dutyDefToSubNode = new DefMap<DutyDef, int>();
				thinkNode_DutyConstant.dutyDefToSubNode.SetAll(-1);
				foreach (DutyDef def in DefDatabase<DutyDef>.AllDefs)
				{
					thinkNode_DutyConstant.dutyDefToSubNode[def] = this.dutyDefToSubNode[def];
				}
			}
			return thinkNode_DutyConstant;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00055890 File Offset: 0x00053C90
		protected override void ResolveSubnodes()
		{
			this.dutyDefToSubNode = new DefMap<DutyDef, int>();
			this.dutyDefToSubNode.SetAll(-1);
			foreach (DutyDef dutyDef in DefDatabase<DutyDef>.AllDefs)
			{
				if (dutyDef.constantThinkNode != null)
				{
					this.dutyDefToSubNode[dutyDef] = this.subNodes.Count;
					dutyDef.constantThinkNode.ResolveSubnodesAndRecur();
					this.subNodes.Add(dutyDef.constantThinkNode.DeepCopy(true));
				}
			}
		}
	}
}
