using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class TraitSet : IExposable
	{
		protected Pawn pawn;

		public List<Trait> allTraits = new List<Trait>();

		public IEnumerable<MentalBreakDef> TheOnlyAllowedMentalBreaks
		{
			get
			{
				int j = 0;
				Trait trait;
				int i;
				while (true)
				{
					if (j < this.allTraits.Count)
					{
						trait = this.allTraits[j];
						if (trait.CurrentData.theOnlyAllowedMentalBreaks != null)
						{
							i = 0;
							if (i < trait.CurrentData.theOnlyAllowedMentalBreaks.Count)
								break;
						}
						j++;
						continue;
					}
					yield break;
				}
				yield return trait.CurrentData.theOnlyAllowedMentalBreaks[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public TraitSet(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Trait>(ref this.allTraits, "allTraits", LookMode.Deep, new object[0]);
		}

		public void GainTrait(Trait trait)
		{
			if (this.HasTrait(trait.def))
			{
				Log.Warning(this.pawn + " already has trait " + trait.def);
			}
			else
			{
				this.allTraits.Add(trait);
				if (this.pawn.workSettings != null)
				{
					this.pawn.workSettings.Notify_GainedTrait();
				}
				this.pawn.story.Notify_TraitChanged();
				if (this.pawn.skills != null)
				{
					this.pawn.skills.Notify_SkillDisablesChanged();
				}
				if (!this.pawn.Dead && this.pawn.RaceProps.Humanlike)
				{
					this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
		}

		public bool HasTrait(TraitDef tDef)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.allTraits.Count)
				{
					if (this.allTraits[num].def == tDef)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public Trait GetTrait(TraitDef tDef)
		{
			int num = 0;
			Trait result;
			while (true)
			{
				if (num < this.allTraits.Count)
				{
					if (this.allTraits[num].def == tDef)
					{
						result = this.allTraits[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public int DegreeOfTrait(TraitDef tDef)
		{
			int num = 0;
			int result;
			while (true)
			{
				if (num < this.allTraits.Count)
				{
					if (this.allTraits[num].def == tDef)
					{
						result = this.allTraits[num].Degree;
						break;
					}
					num++;
					continue;
				}
				result = 0;
				break;
			}
			return result;
		}
	}
}
