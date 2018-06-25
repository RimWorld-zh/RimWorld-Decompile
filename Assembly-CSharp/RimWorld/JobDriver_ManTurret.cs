using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public class JobDriver_ManTurret : JobDriver
	{
		private const float ShellSearchRadius = 40f;

		private const int MaxPawnAmmoReservations = 10;

		public JobDriver_ManTurret()
		{
		}

		private static bool GunNeedsLoading(Building b)
		{
			Building_TurretGun building_TurretGun = b as Building_TurretGun;
			bool result;
			if (building_TurretGun == null)
			{
				result = false;
			}
			else
			{
				CompChangeableProjectile compChangeableProjectile = building_TurretGun.gun.TryGetComp<CompChangeableProjectile>();
				result = (compChangeableProjectile != null && !compChangeableProjectile.Loaded);
			}
			return result;
		}

		public static Thing FindAmmoForTurret(Pawn pawn, Building_TurretGun gun)
		{
			StorageSettings allowedShellsSettings = (!pawn.IsColonist) ? null : gun.gun.TryGetComp<CompChangeableProjectile>().allowedShellsSettings;
			Predicate<Thing> validator = (Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t, 10, 1, null, false) && (allowedShellsSettings == null || allowedShellsSettings.AllowedToAccept(t));
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.Shell), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 40f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil gotoTurret = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil loadIfNeeded = new Toil();
			loadIfNeeded.initAction = delegate()
			{
				Pawn actor = loadIfNeeded.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				Building_TurretGun building_TurretGun = building as Building_TurretGun;
				if (!JobDriver_ManTurret.GunNeedsLoading(building))
				{
					this.JumpToToil(gotoTurret);
				}
				else
				{
					Thing thing = JobDriver_ManTurret.FindAmmoForTurret(this.pawn, building_TurretGun);
					if (thing == null)
					{
						if (actor.Faction == Faction.OfPlayer)
						{
							Messages.Message("MessageOutOfNearbyShellsFor".Translate(new object[]
							{
								actor.LabelShort,
								building_TurretGun.Label
							}).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent, true);
						}
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					actor.CurJob.targetB = thing;
					actor.CurJob.count = 1;
				}
			};
			yield return loadIfNeeded;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 10, 1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					Pawn actor = loadIfNeeded.actor;
					Building building = (Building)actor.CurJob.targetA.Thing;
					Building_TurretGun building_TurretGun = building as Building_TurretGun;
					SoundDefOf.Artillery_ShellLoaded.PlayOneShot(new TargetInfo(building_TurretGun.Position, building_TurretGun.Map, false));
					building_TurretGun.gun.TryGetComp<CompChangeableProjectile>().LoadShell(actor.CurJob.targetB.Thing.def, 1);
					actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
			};
			yield return gotoTurret;
			Toil man = new Toil();
			man.tickAction = delegate()
			{
				Pawn actor = man.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				if (JobDriver_ManTurret.GunNeedsLoading(building))
				{
					this.JumpToToil(loadIfNeeded);
				}
				else
				{
					building.GetComp<CompMannable>().ManForATick(actor);
				}
			};
			man.defaultCompleteMode = ToilCompleteMode.Never;
			man.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return man;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <FindAmmoForTurret>c__AnonStorey1
		{
			internal Pawn pawn;

			internal StorageSettings allowedShellsSettings;

			public <FindAmmoForTurret>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Thing t)
			{
				return !t.IsForbidden(this.pawn) && this.pawn.CanReserve(t, 10, 1, null, false) && (this.allowedShellsSettings == null || this.allowedShellsSettings.AllowedToAccept(t));
			}
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <loadShell>__0;

			internal JobDriver_ManTurret $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_ManTurret.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey2 $locvar0;

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
				{
					this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					Toil gotoTurret = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
					Toil loadIfNeeded = new Toil();
					loadIfNeeded.initAction = delegate()
					{
						Pawn actor = loadIfNeeded.actor;
						Building building = (Building)actor.CurJob.targetA.Thing;
						Building_TurretGun building_TurretGun = building as Building_TurretGun;
						if (!JobDriver_ManTurret.GunNeedsLoading(building))
						{
							this.JumpToToil(gotoTurret);
						}
						else
						{
							Thing thing = JobDriver_ManTurret.FindAmmoForTurret(this.pawn, building_TurretGun);
							if (thing == null)
							{
								if (actor.Faction == Faction.OfPlayer)
								{
									Messages.Message("MessageOutOfNearbyShellsFor".Translate(new object[]
									{
										actor.LabelShort,
										building_TurretGun.Label
									}).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent, true);
								}
								actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
							}
							actor.CurJob.targetB = thing;
							actor.CurJob.count = 1;
						}
					};
					this.$current = loadIfNeeded;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
					this.$current = Toils_Reserve.Reserve(TargetIndex.B, 10, 1, null);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
				{
					Toil loadShell = new Toil();
					loadShell.initAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.loadIfNeeded.actor;
						Building building = (Building)actor.CurJob.targetA.Thing;
						Building_TurretGun building_TurretGun = building as Building_TurretGun;
						SoundDefOf.Artillery_ShellLoaded.PlayOneShot(new TargetInfo(building_TurretGun.Position, building_TurretGun.Map, false));
						building_TurretGun.gun.TryGetComp<CompChangeableProjectile>().LoadShell(actor.CurJob.targetB.Thing.def, 1);
						actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
					};
					this.$current = loadShell;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				case 6u:
					this.$current = <MakeNewToils>c__AnonStorey.gotoTurret;
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					<MakeNewToils>c__AnonStorey.man = new Toil();
					<MakeNewToils>c__AnonStorey.man.tickAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.man.actor;
						Building building = (Building)actor.CurJob.targetA.Thing;
						if (JobDriver_ManTurret.GunNeedsLoading(building))
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.JumpToToil(<MakeNewToils>c__AnonStorey.loadIfNeeded);
						}
						else
						{
							building.GetComp<CompMannable>().ManForATick(actor);
						}
					};
					<MakeNewToils>c__AnonStorey.man.defaultCompleteMode = ToilCompleteMode.Never;
					<MakeNewToils>c__AnonStorey.man.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
					this.$current = <MakeNewToils>c__AnonStorey.man;
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
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
				JobDriver_ManTurret.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_ManTurret.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey2
			{
				internal Toil loadIfNeeded;

				internal Toil gotoTurret;

				internal Toil man;

				internal JobDriver_ManTurret.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey2()
				{
				}

				internal void <>m__0()
				{
					Pawn actor = this.loadIfNeeded.actor;
					Building building = (Building)actor.CurJob.targetA.Thing;
					Building_TurretGun building_TurretGun = building as Building_TurretGun;
					if (!JobDriver_ManTurret.GunNeedsLoading(building))
					{
						this.<>f__ref$0.$this.JumpToToil(this.gotoTurret);
					}
					else
					{
						Thing thing = JobDriver_ManTurret.FindAmmoForTurret(this.<>f__ref$0.$this.pawn, building_TurretGun);
						if (thing == null)
						{
							if (actor.Faction == Faction.OfPlayer)
							{
								Messages.Message("MessageOutOfNearbyShellsFor".Translate(new object[]
								{
									actor.LabelShort,
									building_TurretGun.Label
								}).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent, true);
							}
							actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
						}
						actor.CurJob.targetB = thing;
						actor.CurJob.count = 1;
					}
				}

				internal void <>m__1()
				{
					Pawn actor = this.loadIfNeeded.actor;
					Building building = (Building)actor.CurJob.targetA.Thing;
					Building_TurretGun building_TurretGun = building as Building_TurretGun;
					SoundDefOf.Artillery_ShellLoaded.PlayOneShot(new TargetInfo(building_TurretGun.Position, building_TurretGun.Map, false));
					building_TurretGun.gun.TryGetComp<CompChangeableProjectile>().LoadShell(actor.CurJob.targetB.Thing.def, 1);
					actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}

				internal void <>m__2()
				{
					Pawn actor = this.man.actor;
					Building building = (Building)actor.CurJob.targetA.Thing;
					if (JobDriver_ManTurret.GunNeedsLoading(building))
					{
						this.<>f__ref$0.$this.JumpToToil(this.loadIfNeeded);
					}
					else
					{
						building.GetComp<CompMannable>().ManForATick(actor);
					}
				}
			}
		}
	}
}
