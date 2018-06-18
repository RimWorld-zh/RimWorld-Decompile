using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200053E RID: 1342
	public class TraitSet : IExposable
	{
		// Token: 0x060018F9 RID: 6393 RVA: 0x000D9348 File Offset: 0x000D7748
		public TraitSet(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x000D9363 File Offset: 0x000D7763
		public void ExposeData()
		{
			Scribe_Collections.Look<Trait>(ref this.allTraits, "allTraits", LookMode.Deep, new object[0]);
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x000D9380 File Offset: 0x000D7780
		public void GainTrait(Trait trait)
		{
			if (this.HasTrait(trait.def))
			{
				Log.Warning(this.pawn + " already has trait " + trait.def, false);
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

		// Token: 0x060018FC RID: 6396 RVA: 0x000D9464 File Offset: 0x000D7864
		public bool HasTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x000D94B8 File Offset: 0x000D78B8
		public IEnumerable<MentalBreakDef> TheOnlyAllowedMentalBreaks
		{
			get
			{
				for (int i = 0; i < this.allTraits.Count; i++)
				{
					Trait trait = this.allTraits[i];
					if (trait.CurrentData.theOnlyAllowedMentalBreaks != null)
					{
						for (int j = 0; j < trait.CurrentData.theOnlyAllowedMentalBreaks.Count; j++)
						{
							yield return trait.CurrentData.theOnlyAllowedMentalBreaks[j];
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x000D94E4 File Offset: 0x000D78E4
		public Trait GetTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef)
				{
					return this.allTraits[i];
				}
			}
			return null;
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x000D9544 File Offset: 0x000D7944
		public int DegreeOfTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef)
				{
					return this.allTraits[i].Degree;
				}
			}
			return 0;
		}

		// Token: 0x04000EAE RID: 3758
		protected Pawn pawn;

		// Token: 0x04000EAF RID: 3759
		public List<Trait> allTraits = new List<Trait>();
	}
}
