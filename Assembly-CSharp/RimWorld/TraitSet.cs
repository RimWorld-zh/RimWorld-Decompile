using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200053C RID: 1340
	public class TraitSet : IExposable
	{
		// Token: 0x04000EAB RID: 3755
		protected Pawn pawn;

		// Token: 0x04000EAC RID: 3756
		public List<Trait> allTraits = new List<Trait>();

		// Token: 0x060018F4 RID: 6388 RVA: 0x000D94A8 File Offset: 0x000D78A8
		public TraitSet(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x000D94C3 File Offset: 0x000D78C3
		public void ExposeData()
		{
			Scribe_Collections.Look<Trait>(ref this.allTraits, "allTraits", LookMode.Deep, new object[0]);
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x000D94E0 File Offset: 0x000D78E0
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

		// Token: 0x060018F7 RID: 6391 RVA: 0x000D95C4 File Offset: 0x000D79C4
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
		// (get) Token: 0x060018F8 RID: 6392 RVA: 0x000D9618 File Offset: 0x000D7A18
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

		// Token: 0x060018F9 RID: 6393 RVA: 0x000D9644 File Offset: 0x000D7A44
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

		// Token: 0x060018FA RID: 6394 RVA: 0x000D96A4 File Offset: 0x000D7AA4
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
	}
}
