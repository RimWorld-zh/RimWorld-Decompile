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
	public class JobDriver_EnterCryptosleepCasket : JobDriver
	{
		public JobDriver_EnterCryptosleepCasket()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil prepare = Toils_General.Wait(500, TargetIndex.None);
			prepare.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return prepare;
			Toil enter = new Toil();
			enter.initAction = delegate()
			{
				Pawn actor = enter.actor;
				Building_CryptosleepCasket pod = (Building_CryptosleepCasket)actor.CurJob.targetA.Thing;
				Action action = delegate()
				{
					actor.DeSpawn(DestroyMode.Vanish);
					pod.TryAcceptThing(actor, true);
				};
				if (!pod.def.building.isPlayerEjectable)
				{
					int freeColonistsSpawnedOrInPlayerEjectablePodsCount = this.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount;
					if (freeColonistsSpawnedOrInPlayerEjectablePodsCount <= 1)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CasketWarning".Translate().AdjustedFor(actor, "PAWN"), action, false, null));
					}
					else
					{
						action();
					}
				}
				else
				{
					action();
				}
			};
			enter.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return enter;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <prepare>__0;

			internal JobDriver_EnterCryptosleepCasket $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_EnterCryptosleepCasket.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.FailOnDespawnedOrNull(TargetIndex.A);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					prepare = Toils_General.Wait(500, TargetIndex.None);
					prepare.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
					prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
					this.$current = prepare;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
				{
					Toil enter = new Toil();
					enter.initAction = delegate()
					{
						Pawn actor = enter.actor;
						Building_CryptosleepCasket pod = (Building_CryptosleepCasket)actor.CurJob.targetA.Thing;
						Action action = delegate()
						{
							actor.DeSpawn(DestroyMode.Vanish);
							pod.TryAcceptThing(actor, true);
						};
						if (!pod.def.building.isPlayerEjectable)
						{
							int freeColonistsSpawnedOrInPlayerEjectablePodsCount = this.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount;
							if (freeColonistsSpawnedOrInPlayerEjectablePodsCount <= 1)
							{
								Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CasketWarning".Translate().AdjustedFor(actor, "PAWN"), action, false, null));
							}
							else
							{
								action();
							}
						}
						else
						{
							action();
						}
					};
					enter.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = enter;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
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
				JobDriver_EnterCryptosleepCasket.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_EnterCryptosleepCasket.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil enter;

				internal JobDriver_EnterCryptosleepCasket.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					JobDriver_EnterCryptosleepCasket.<MakeNewToils>c__Iterator0 <>f__ref$0 = this.<>f__ref$0;
					JobDriver_EnterCryptosleepCasket.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 <>f__ref$1 = this;
					Pawn actor = this.enter.actor;
					Building_CryptosleepCasket pod = (Building_CryptosleepCasket)actor.CurJob.targetA.Thing;
					Action action = delegate()
					{
						actor.DeSpawn(DestroyMode.Vanish);
						pod.TryAcceptThing(actor, true);
					};
					if (!pod.def.building.isPlayerEjectable)
					{
						int freeColonistsSpawnedOrInPlayerEjectablePodsCount = this.<>f__ref$0.$this.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount;
						if (freeColonistsSpawnedOrInPlayerEjectablePodsCount <= 1)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CasketWarning".Translate().AdjustedFor(actor, "PAWN"), action, false, null));
						}
						else
						{
							action();
						}
					}
					else
					{
						action();
					}
				}

				private sealed class <MakeNewToils>c__AnonStorey2
				{
					internal Pawn actor;

					internal Building_CryptosleepCasket pod;

					internal JobDriver_EnterCryptosleepCasket.<MakeNewToils>c__Iterator0 <>f__ref$0;

					internal JobDriver_EnterCryptosleepCasket.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 <>f__ref$1;

					public <MakeNewToils>c__AnonStorey2()
					{
					}

					internal void <>m__0()
					{
						this.actor.DeSpawn(DestroyMode.Vanish);
						this.pod.TryAcceptThing(this.actor, true);
					}
				}
			}
		}
	}
}
