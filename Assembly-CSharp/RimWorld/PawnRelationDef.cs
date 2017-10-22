using System;
using System.Collections.Generic;
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
		private PawnRelationWorker workerInt = null;

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
			return (pawn.gender != Gender.Female || this.labelFemale.NullOrEmpty()) ? base.label : this.labelFemale;
		}

		public string GetGenderSpecificLabelCap(Pawn pawn)
		{
			return this.GetGenderSpecificLabel(pawn).CapitalizeFirst();
		}

		public ThoughtDef GetGenderSpecificDiedThought(Pawn killed)
		{
			return (killed.gender != Gender.Female || this.diedThoughtFemale == null) ? this.diedThought : this.diedThoughtFemale;
		}

		public ThoughtDef GetGenderSpecificKilledThought(Pawn killed)
		{
			return (killed.gender != Gender.Female || this.killedThoughtFemale == null) ? this.killedThought : this.killedThoughtFemale;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string c = enumerator.Current;
					yield return c;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!this.implied)
				yield break;
			if (!this.reflexive)
				yield break;
			yield return base.defName + ": implied relations can't use the \"reflexive\" option.";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_011e:
			/*Error near IL_011f: Unexpected return in MoveNext()*/;
		}
	}
}
