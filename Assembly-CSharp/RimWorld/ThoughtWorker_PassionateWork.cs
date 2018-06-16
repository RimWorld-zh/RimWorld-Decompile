using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200022B RID: 555
	public class ThoughtWorker_PassionateWork : ThoughtWorker
	{
		// Token: 0x06000A26 RID: 2598 RVA: 0x00059A58 File Offset: 0x00057E58
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			JobDriver curDriver = p.jobs.curDriver;
			ThoughtState result;
			if (curDriver == null)
			{
				result = ThoughtState.Inactive;
			}
			else if (p.skills == null)
			{
				result = ThoughtState.Inactive;
			}
			else if (curDriver.ActiveSkill == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				SkillRecord skill = p.skills.GetSkill(curDriver.ActiveSkill);
				if (skill == null)
				{
					result = ThoughtState.Inactive;
				}
				else if (skill.passion == Passion.Minor)
				{
					result = ThoughtState.ActiveAtStage(0);
				}
				else if (skill.passion == Passion.Major)
				{
					result = ThoughtState.ActiveAtStage(1);
				}
				else
				{
					result = ThoughtState.Inactive;
				}
			}
			return result;
		}
	}
}
