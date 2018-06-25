using System;
using Verse;

namespace RimWorld
{
	public class CompNeurotrainer : CompUsable
	{
		public SkillDef skill;

		public CompNeurotrainer()
		{
		}

		protected override string FloatMenuOptionLabel
		{
			get
			{
				return string.Format(base.Props.useLabel, this.skill.LabelCap);
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<SkillDef>(ref this.skill, "skill");
		}

		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.skill = DefDatabase<SkillDef>.GetRandom();
		}

		public override string TransformLabel(string label)
		{
			return this.skill.LabelCap + " " + label;
		}

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
