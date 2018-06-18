using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000725 RID: 1829
	public class CompNeurotrainer : CompUsable
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002845 RID: 10309 RVA: 0x00158120 File Offset: 0x00156520
		protected override string FloatMenuOptionLabel
		{
			get
			{
				return string.Format(base.Props.useLabel, this.skill.LabelCap);
			}
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x00158150 File Offset: 0x00156550
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<SkillDef>(ref this.skill, "skill");
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x00158169 File Offset: 0x00156569
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.skill = DefDatabase<SkillDef>.GetRandom();
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x00158180 File Offset: 0x00156580
		public override string TransformLabel(string label)
		{
			return this.skill.LabelCap + " " + label;
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x001581AC File Offset: 0x001565AC
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

		// Token: 0x0600284A RID: 10314 RVA: 0x001581FC File Offset: 0x001565FC
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
