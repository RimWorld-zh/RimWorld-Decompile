using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000723 RID: 1827
	public class CompNeurotrainer : CompUsable
	{
		// Token: 0x040015FC RID: 5628
		public SkillDef skill;

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002841 RID: 10305 RVA: 0x0015842C File Offset: 0x0015682C
		protected override string FloatMenuOptionLabel
		{
			get
			{
				return string.Format(base.Props.useLabel, this.skill.LabelCap);
			}
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x0015845C File Offset: 0x0015685C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<SkillDef>(ref this.skill, "skill");
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x00158475 File Offset: 0x00156875
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.skill = DefDatabase<SkillDef>.GetRandom();
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x0015848C File Offset: 0x0015688C
		public override string TransformLabel(string label)
		{
			return this.skill.LabelCap + " " + label;
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x001584B8 File Offset: 0x001568B8
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

		// Token: 0x06002846 RID: 10310 RVA: 0x00158508 File Offset: 0x00156908
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
