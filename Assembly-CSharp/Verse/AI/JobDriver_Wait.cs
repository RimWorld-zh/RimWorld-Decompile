using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse.AI
{
	public class JobDriver_Wait : JobDriver
	{
		private const int TargetSearchInterval = 4;

		public JobDriver_Wait()
		{
		}

		public override string GetReport()
		{
			string result;
			if (this.job.def == JobDefOf.Wait_Combat)
			{
				if (this.pawn.RaceProps.Humanlike && this.pawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					result = "ReportStanding".Translate();
				}
				else
				{
					result = base.GetReport();
				}
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil wait = new Toil();
			wait.initAction = delegate()
			{
				base.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.pawn.Position);
				this.pawn.pather.StopDead();
				this.CheckForAutoAttack();
			};
			wait.tickAction = delegate()
			{
				if (this.job.expiryInterval == -1 && this.job.def == JobDefOf.Wait_Combat && !this.pawn.Drafted)
				{
					Log.Error(this.pawn + " in eternal WaitCombat without being drafted.", false);
					base.ReadyForNextToil();
				}
				else if ((Find.TickManager.TicksGame + this.pawn.thingIDNumber) % 4 == 0)
				{
					this.CheckForAutoAttack();
				}
			};
			this.DecorateWaitToil(wait);
			wait.defaultCompleteMode = ToilCompleteMode.Never;
			yield return wait;
			yield break;
		}

		public virtual void DecorateWaitToil(Toil wait)
		{
		}

		public override void Notify_StanceChanged()
		{
			if (this.pawn.stances.curStance is Stance_Mobile)
			{
				this.CheckForAutoAttack();
			}
		}

		private void CheckForAutoAttack()
		{
			if (!this.pawn.Downed)
			{
				if (!this.pawn.stances.FullBodyBusy)
				{
					this.collideWithPawns = false;
					bool flag = this.pawn.story == null || !this.pawn.story.WorkTagIsDisabled(WorkTags.Violent);
					bool flag2 = this.pawn.RaceProps.ToolUser && this.pawn.Faction == Faction.OfPlayer && !this.pawn.story.WorkTagIsDisabled(WorkTags.Firefighting);
					if (flag || flag2)
					{
						Fire fire = null;
						for (int i = 0; i < 9; i++)
						{
							IntVec3 c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[i];
							if (c.InBounds(this.pawn.Map))
							{
								List<Thing> thingList = c.GetThingList(base.Map);
								for (int j = 0; j < thingList.Count; j++)
								{
									if (flag)
									{
										Pawn pawn = thingList[j] as Pawn;
										if (pawn != null && !pawn.Downed && this.pawn.HostileTo(pawn))
										{
											this.pawn.meleeVerbs.TryMeleeAttack(pawn, null, false);
											this.collideWithPawns = true;
											return;
										}
									}
									if (flag2)
									{
										Fire fire2 = thingList[j] as Fire;
										if (fire2 != null && (fire == null || fire2.fireSize < fire.fireSize || i == 8) && (fire2.parent == null || fire2.parent != this.pawn))
										{
											fire = fire2;
										}
									}
								}
							}
						}
						if (fire != null && (!this.pawn.InMentalState || this.pawn.MentalState.def.allowBeatfire))
						{
							this.pawn.natives.TryBeatFire(fire);
						}
						else if (flag && this.pawn.Faction != null && this.job.def == JobDefOf.Wait_Combat && (this.pawn.drafter == null || this.pawn.drafter.FireAtWill))
						{
							bool allowManualCastWeapons = !this.pawn.IsColonist;
							Verb verb = this.pawn.TryGetAttackVerb(null, allowManualCastWeapons);
							if (verb != null && !verb.verbProps.IsMeleeAttack)
							{
								TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedThreat;
								if (verb.IsIncendiary())
								{
									targetScanFlags |= TargetScanFlags.NeedNonBurning;
								}
								Thing thing = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this.pawn, null, verb.verbProps.range, verb.verbProps.minRange, targetScanFlags);
								if (thing != null)
								{
									this.pawn.TryStartAttack(thing);
									this.collideWithPawns = true;
								}
							}
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <wait>__0;

			internal JobDriver_Wait $this;

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
					wait = new Toil();
					wait.initAction = delegate()
					{
						base.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.pawn.Position);
						this.pawn.pather.StopDead();
						base.CheckForAutoAttack();
					};
					wait.tickAction = delegate()
					{
						if (this.job.expiryInterval == -1 && this.job.def == JobDefOf.Wait_Combat && !this.pawn.Drafted)
						{
							Log.Error(this.pawn + " in eternal WaitCombat without being drafted.", false);
							base.ReadyForNextToil();
						}
						else if ((Find.TickManager.TicksGame + this.pawn.thingIDNumber) % 4 == 0)
						{
							base.CheckForAutoAttack();
						}
					};
					this.DecorateWaitToil(wait);
					wait.defaultCompleteMode = ToilCompleteMode.Never;
					this.$current = wait;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
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
				JobDriver_Wait.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Wait.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				base.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.pawn.Position);
				this.pawn.pather.StopDead();
				base.CheckForAutoAttack();
			}

			internal void <>m__1()
			{
				if (this.job.expiryInterval == -1 && this.job.def == JobDefOf.Wait_Combat && !this.pawn.Drafted)
				{
					Log.Error(this.pawn + " in eternal WaitCombat without being drafted.", false);
					base.ReadyForNextToil();
				}
				else if ((Find.TickManager.TicksGame + this.pawn.thingIDNumber) % 4 == 0)
				{
					base.CheckForAutoAttack();
				}
			}
		}
	}
}
