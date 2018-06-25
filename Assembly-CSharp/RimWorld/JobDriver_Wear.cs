using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Wear : JobDriver
	{
		private int duration;

		private int unequipBuffer;

		private const TargetIndex ApparelInd = TargetIndex.A;

		public JobDriver_Wear()
		{
		}

		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.unequipBuffer, "unequipBuffer", 0, false);
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Apparel, this.job, 1, -1, null);
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true) * 60f);
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					this.duration += (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true) * 60f);
				}
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil prepare = new Toil();
			prepare.tickAction = delegate()
			{
				this.unequipBuffer++;
				this.TryUnequipSomething();
			};
			prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			prepare.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			prepare.defaultCompleteMode = ToilCompleteMode.Delay;
			prepare.defaultDuration = this.duration;
			yield return prepare;
			yield return Toils_General.Do(delegate
			{
				Apparel apparel = this.Apparel;
				this.pawn.apparel.Wear(apparel, true);
				if (this.pawn.outfits != null && this.job.playerForced)
				{
					this.pawn.outfits.forcedHandler.SetForced(apparel, true);
				}
			});
			yield break;
		}

		private void TryUnequipSomething()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					int num = (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true) * 60f);
					if (this.unequipBuffer >= num)
					{
						bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
						Apparel apparel2;
						if (!this.pawn.apparel.TryDrop(wornApparel[i], out apparel2, this.pawn.PositionHeld, forbid))
						{
							Log.Error(this.pawn + " could not drop " + wornApparel[i].ToStringSafe<Apparel>(), false);
							base.EndJobWith(JobCondition.Errored);
						}
					}
					break;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <prepare>__1;

			internal JobDriver_Wear $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					prepare = new Toil();
					prepare.tickAction = delegate()
					{
						this.unequipBuffer++;
						base.TryUnequipSomething();
					};
					prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
					prepare.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					prepare.defaultCompleteMode = ToilCompleteMode.Delay;
					prepare.defaultDuration = this.duration;
					this.$current = prepare;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_General.Do(delegate
					{
						Apparel apparel = base.Apparel;
						this.pawn.apparel.Wear(apparel, true);
						if (this.pawn.outfits != null && this.job.playerForced)
						{
							this.pawn.outfits.forcedHandler.SetForced(apparel, true);
						}
					});
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Toil IEnumerator<Toil>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_Wear.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Wear.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				this.unequipBuffer++;
				base.TryUnequipSomething();
			}

			internal void <>m__1()
			{
				Apparel apparel = base.Apparel;
				this.pawn.apparel.Wear(apparel, true);
				if (this.pawn.outfits != null && this.job.playerForced)
				{
					this.pawn.outfits.forcedHandler.SetForced(apparel, true);
				}
			}
		}
	}
}
