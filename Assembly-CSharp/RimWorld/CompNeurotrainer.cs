using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000721 RID: 1825
	public class CompNeurotrainer : CompUsable
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x001582DC File Offset: 0x001566DC
		protected override string FloatMenuOptionLabel
		{
			get
			{
				return string.Format(base.Props.useLabel, this.skill.LabelCap);
			}
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x0015830C File Offset: 0x0015670C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<SkillDef>(ref this.skill, "skill");
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x00158325 File Offset: 0x00156725
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.skill = DefDatabase<SkillDef>.GetRandom();
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x0015833C File Offset: 0x0015673C
		public override string TransformLabel(string label)
		{
			return this.skill.LabelCap + " " + label;
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x00158368 File Offset: 0x00156768
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

		// Token: 0x06002842 RID: 10306 RVA: 0x001583B8 File Offset: 0x001567B8
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompNeurotrainer compNeurotrainer = piece.TryGetComp<CompNeurotrainer>();
			if (compNeurotrainer != null)
			{
				compNeurotrainer.skill = this.skill;
			}
		}

		// Token: 0x040015FC RID: 5628
		public SkillDef skill;
	}
}
