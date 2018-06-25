using System;
using Verse;

namespace RimWorld
{
	public class GameCondition_PsychicEmanation : GameCondition
	{
		public Gender gender = Gender.Male;

		public GameCondition_PsychicEmanation()
		{
		}

		public override string Label
		{
			get
			{
				return this.def.label + " (" + this.gender.ToString().Translate().ToLower() + ")";
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}
	}
}
