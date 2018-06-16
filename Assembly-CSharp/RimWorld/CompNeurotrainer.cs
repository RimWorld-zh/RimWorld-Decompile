using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000725 RID: 1829
	public class CompNeurotrainer : CompUsable
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002843 RID: 10307 RVA: 0x001580A8 File Offset: 0x001564A8
		protected override string FloatMenuOptionLabel
		{
			get
			{
				return string.Format(base.Props.useLabel, this.skill.LabelCap);
			}
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x001580D8 File Offset: 0x001564D8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<SkillDef>(ref this.skill, "skill");
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x001580F1 File Offset: 0x001564F1
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.skill = DefDatabase<SkillDef>.GetRandom();
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x00158108 File Offset: 0x00156508
		public override string TransformLabel(string label)
		{
			return this.skill.LabelCap + " " + label;
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x00158134 File Offset: 0x00156534
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

		// Token: 0x06002848 RID: 10312 RVA: 0x00158184 File Offset: 0x00156584
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompNeurotrainer compNeurotrainer = piece.TryGetComp<CompNeurotrainer>();
			if (compNeurotrainer != null)
			{
				compNeurotrainer.skill = this.skill;
			}
		}

		// Token: 0x040015FE RID: 5630
		public SkillDef skill;
	}
}
