using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000762 RID: 1890
	public class CompUseEffect_LearnSkill : CompUseEffect
	{
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060029CD RID: 10701 RVA: 0x00162F84 File Offset: 0x00161384
		private SkillDef Skill
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().skill;
			}
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x00162FAC File Offset: 0x001613AC
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

		// Token: 0x060029CF RID: 10703 RVA: 0x00163050 File Offset: 0x00161450
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

		// Token: 0x0400169C RID: 5788
		private const float XPGainAmount = 50000f;
	}
}
