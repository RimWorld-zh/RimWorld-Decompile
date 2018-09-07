using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class ThinkNode_DutyConstant : ThinkNode
	{
		private DefMap<DutyDef, int> dutyDefToSubNode;

		public ThinkNode_DutyConstant()
		{
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (pawn.GetLord() == null)
			{
				Log.Error(pawn + " doing ThinkNode_DutyConstant with no Lord.", false);
				return ThinkResult.NoJob;
			}
			if (pawn.mindState.duty == null)
			{
				Log.Error(pawn + " doing ThinkNode_DutyConstant with no duty.", false);
				return ThinkResult.NoJob;
			}
			if (this.dutyDefToSubNode == null)
			{
				Log.Error(pawn + " has null dutyDefToSubNode. Recovering by calling ResolveSubnodes() (though that should have been called already).", false);
				this.ResolveSubnodes();
			}
			int num = this.dutyDefToSubNode[pawn.mindState.duty.def];
			if (num < 0)
			{
				return ThinkResult.NoJob;
			}
			return this.subNodes[num].TryIssueJobPackage(pawn, jobParams);
		}

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
