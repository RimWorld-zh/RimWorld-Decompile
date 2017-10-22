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
			if (pawn.GetLord() == null)
			{
				Log.Error(pawn + " doing ThinkNode_DutyConstant with no Lord.");
				return ThinkResult.NoJob;
			}
			if (pawn.mindState.duty == null)
			{
				Log.Error(pawn + " doing ThinkNode_DutyConstant with no duty.");
				return ThinkResult.NoJob;
			}
			if (this.dutyDefToSubNode == null)
			{
				Log.Error(pawn + " has null dutyDefToSubNode. Recovering by calling ResolveSubnodes() (though that should have been called already).");
				this.ResolveSubnodes();
			}
			int num = this.dutyDefToSubNode[pawn.mindState.duty.def];
			if (num < 0)
			{
				return ThinkResult.NoJob;
			}
			return base.subNodes[num].TryIssueJobPackage(pawn, jobParams);
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
