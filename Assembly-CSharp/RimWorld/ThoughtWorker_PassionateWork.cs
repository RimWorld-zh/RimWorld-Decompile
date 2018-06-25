using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThoughtWorker_PassionateWork : ThoughtWorker
	{
		public ThoughtWorker_PassionateWork()
		{
		}

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
