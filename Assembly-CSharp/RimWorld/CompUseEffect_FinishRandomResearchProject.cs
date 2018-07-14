using System;
using Verse;

namespace RimWorld
{
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		public CompUseEffect_FinishRandomResearchProject()
		{
		}

		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
			if (currentProj != null)
			{
				this.FinishInstantly(currentProj, usedBy);
			}
		}

		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			bool result;
			if (Find.ResearchManager.currentProj == null)
			{
				failReason = "NoActiveResearchProjectToFinish".Translate();
				result = false;
			}
			else
			{
				failReason = null;
				result = true;
			}
			return result;
		}

		private void FinishInstantly(ResearchProjectDef proj, Pawn usedBy)
		{
			Find.ResearchManager.InstantFinish(proj, false);
			Messages.Message("MessageResearchProjectFinishedByItem".Translate(new object[]
			{
				proj.LabelCap
			}), usedBy, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
