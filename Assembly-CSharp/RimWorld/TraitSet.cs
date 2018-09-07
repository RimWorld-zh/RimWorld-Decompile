using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class TraitSet : IExposable
	{
		protected Pawn pawn;

		public List<Trait> allTraits = new List<Trait>();

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
				Log.Warning(this.pawn + " already has trait " + trait.def, false);
				return;
			}
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

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<MentalBreakDef>, IEnumerator, IDisposable, IEnumerator<MentalBreakDef>
		{
			internal int <i>__1;

			internal Trait <trait>__2;

			internal int <j>__3;

			internal TraitSet $this;

			internal MentalBreakDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					goto IL_DB;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_AD:
				if (j < trait.CurrentData.theOnlyAllowedMentalBreaks.Count)
				{
					this.$current = trait.CurrentData.theOnlyAllowedMentalBreaks[j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_CD:
				i++;
				IL_DB:
				if (i >= this.allTraits.Count)
				{
					this.$PC = -1;
				}
				else
				{
					trait = this.allTraits[i];
					if (trait.CurrentData.theOnlyAllowedMentalBreaks != null)
					{
						j = 0;
						goto IL_AD;
					}
					goto IL_CD;
				}
				return false;
			}

			MentalBreakDef IEnumerator<MentalBreakDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.MentalBreakDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<MentalBreakDef> IEnumerable<MentalBreakDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TraitSet.<>c__Iterator0 <>c__Iterator = new TraitSet.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}
