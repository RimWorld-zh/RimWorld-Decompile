using System;
using Verse;

namespace RimWorld
{
	public class CompUseEffect_LearnSkill : CompUseEffect
	{
		private const float XPGainAmount = 50000f;

		public CompUseEffect_LearnSkill()
		{
		}

		private SkillDef Skill
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().skill;
			}
		}

		public override void DoEffect(Pawn user)
		{
			base.DoEffect(user);
			SkillDef skill = this.Skill;
			int level = user.skills.GetSkill(skill).Level;
			user.skills.Learn(skill, 50000f, true);
			int level2 = user.skills.GetSkill(skill).Level;
			if (PawnUtility.ShouldSendNotificationAbout(user))
			{
				Messages.Message("NeurotrainerUsed".Translate(new object[]
				{
					user.LabelShort,
					skill.LabelCap,
					level,
					level2
				}), user, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			bool result;
			if (p.skills == null)
			{
				failReason = null;
				result = false;
			}
			else if (p.skills.GetSkill(this.Skill).TotallyDisabled)
			{
				failReason = "SkillDisabled".Translate();
				result = false;
			}
			else
			{
				result = base.CanBeUsedBy(p, out failReason);
			}
			return result;
		}
	}
}
