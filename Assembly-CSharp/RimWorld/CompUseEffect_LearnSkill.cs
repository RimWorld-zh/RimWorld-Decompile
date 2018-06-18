using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000766 RID: 1894
	public class CompUseEffect_LearnSkill : CompUseEffect
	{
		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060029D4 RID: 10708 RVA: 0x00162DAC File Offset: 0x001611AC
		private SkillDef Skill
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().skill;
			}
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x00162DD4 File Offset: 0x001611D4
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

		// Token: 0x060029D6 RID: 10710 RVA: 0x00162E78 File Offset: 0x00161278
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

		// Token: 0x0400169E RID: 5790
		private const float XPGainAmount = 50000f;
	}
}
