using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000764 RID: 1892
	public class CompUseEffect_LearnSkill : CompUseEffect
	{
		// Token: 0x040016A0 RID: 5792
		private const float XPGainAmount = 50000f;

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060029D0 RID: 10704 RVA: 0x00163334 File Offset: 0x00161734
		private SkillDef Skill
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().skill;
			}
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x0016335C File Offset: 0x0016175C
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

		// Token: 0x060029D2 RID: 10706 RVA: 0x00163400 File Offset: 0x00161800
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
