using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class PawnRelationDef : Def
	{
		public Type workerClass = typeof(PawnRelationWorker);

		[MustTranslate]
		public string labelFemale;

		public float importance;

		public bool implied;

		public bool reflexive;

		public int opinionOffset;

		public float generationChanceFactor;

		public float attractionFactor = 1f;

		public float incestOpinionOffset;

		public bool familyByBloodRelation;

		public ThoughtDef diedThought;

		public ThoughtDef diedThoughtFemale;

		public ThoughtDef soldThought;

		public ThoughtDef killedThought;

		public ThoughtDef killedThoughtFemale;

		[Unsaved]
		private PawnRelationWorker workerInt;

		public PawnRelationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnRelationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		public string GetGenderSpecificLabel(Pawn pawn)
		{
			if (pawn.gender == Gender.Female && !this.labelFemale.NullOrEmpty())
			{
				return this.labelFemale;
			}
			return this.label;
		}

		public string GetGenderSpecificLabelCap(Pawn pawn)
		{
			return this.GetGenderSpecificLabel(pawn).CapitalizeFirst();
		}

		public ThoughtDef GetGenderSpecificDiedThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.diedThoughtFemale;
			}
			return this.diedThought;
		}

		public ThoughtDef GetGenderSpecificKilledThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.killedThoughtFemale != null)
			{
				return this.killedThoughtFemale;
			}
			return this.killedThought;
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			PawnRelationDef.<ConfigErrors>c__Iterator92 <ConfigErrors>c__Iterator = new PawnRelationDef.<ConfigErrors>c__Iterator92();
			<ConfigErrors>c__Iterator.<>f__this = this;
			PawnRelationDef.<ConfigErrors>c__Iterator92 expr_0E = <ConfigErrors>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
