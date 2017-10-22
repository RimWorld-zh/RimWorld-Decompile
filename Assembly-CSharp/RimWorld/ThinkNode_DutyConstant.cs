using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class ThinkNode_DutyConstant : ThinkNode
	{
		private DefMap<DutyDef, int> dutyDefToSubNode;

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result;
			if (pawn.GetLord() == null)
			{
				Log.Error(pawn + " doing ThinkNode_DutyConstant with no Lord.");
				result = ThinkResult.NoJob;
			}
			else if (pawn.mindState.duty == null)
			{
				Log.Error(pawn + " doing ThinkNode_DutyConstant with no duty.");
				result = ThinkResult.NoJob;
			}
			else
			{
				if (this.dutyDefToSubNode == null)
				{
					Log.Error(pawn + " has null dutyDefToSubNode. Recovering by calling ResolveSubnodes() (though that should have been called already).");
					this.ResolveSubnodes();
				}
				int num = this.dutyDefToSubNode[pawn.mindState.duty.def];
				result = ((num >= 0) ? base.subNodes[num].TryIssueJobPackage(pawn, jobParams) : ThinkResult.NoJob);
			}
			return result;
		}

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_DutyConstant thinkNode_DutyConstant = (ThinkNode_DutyConstant)base.DeepCopy(resolve);
			if (this.dutyDefToSubNode != null)
			{
				thinkNode_DutyConstant.dutyDefToSubNode = new DefMap<DutyDef, int>();
				thinkNode_DutyConstant.dutyDefToSubNode.SetAll(-1);
				foreach (DutyDef allDef in DefDatabase<DutyDef>.AllDefs)
				{
					thinkNode_DutyConstant.dutyDefToSubNode[allDef] = this.dutyDefToSubNode[allDef];
				}
			}
			return thinkNode_DutyConstant;
		}

		protected override void ResolveSubnodes()
		{
			this.dutyDefToSubNode = new DefMap<DutyDef, int>();
			this.dutyDefToSubNode.SetAll(-1);
			foreach (DutyDef allDef in DefDatabase<DutyDef>.AllDefs)
			{
				if (allDef.constantThinkNode != null)
				{
					this.dutyDefToSubNode[allDef] = base.subNodes.Count;
					allDef.constantThinkNode.ResolveSubnodesAndRecur();
					base.subNodes.Add(allDef.constantThinkNode.DeepCopy(true));
				}
			}
		}
	}
}
