using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000723 RID: 1827
	public class CompNeurotrainer : CompUsable
	{
		// Token: 0x04001600 RID: 5632
		public SkillDef skill;

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x0015868C File Offset: 0x00156A8C
		protected override string FloatMenuOptionLabel
		{
			get
			{
				return string.Format(base.Props.useLabel, this.skill.LabelCap);
			}
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x001586BC File Offset: 0x00156ABC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<SkillDef>(ref this.skill, "skill");
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x001586D5 File Offset: 0x00156AD5
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.skill = DefDatabase<SkillDef>.GetRandom();
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x001586EC File Offset: 0x00156AEC
		public override string TransformLabel(string label)
		{
			return this.skill.LabelCap + " " + label;
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x00158718 File Offset: 0x00156B18
		public override bool AllowStackWith(Thing other)
		{
			bool result;
			if (!base.AllowStackWith(other))
			{
				result = false;
			}
			else
			{
				CompNeurotrainer compNeurotrainer = other.TryGetComp<CompNeurotrainer>();
				result = (compNeurotrainer != null && compNeurotrainer.skill == this.skill);
			}
			return result;
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x00158768 File Offset: 0x00156B68
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompNeurotrainer compNeurotrainer = piece.TryGetComp<CompNeurotrainer>();
			if (compNeurotrainer != null)
			{
				compNeurotrainer.skill = this.skill;
			}
		}
	}
}
