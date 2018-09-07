using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse.AI
{
	public class JobDriver_AttackStatic : JobDriver
	{
		private bool startedIncapacitated;

		private int numAttacksMade;

		public JobDriver_AttackStatic()
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
			Scribe_Values.Look<int>(ref this.numAttacksMade, "numAttacksMade", 0, false);
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					Pawn pawn = base.TargetThingA as Pawn;
					if (pawn != null)
					{
						this.startedIncapacitated = pawn.Downed;
					}
					this.pawn.pather.StopDead();
				},
				tickAction = delegate()
				{
					if (!base.TargetA.IsValid)
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
					if (base.TargetA.HasThing)
					{
						Pawn pawn = base.TargetA.Thing as Pawn;
						if (base.TargetA.Thing.Destroyed || (pawn != null && !this.startedIncapacitated && pawn.Downed))
						{
							base.EndJobWith(JobCondition.Succeeded);
							return;
						}
					}
					if (this.numAttacksMade >= this.job.maxNumStaticAttacks && !this.pawn.stances.FullBodyBusy)
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
					if (this.pawn.TryStartAttack(base.TargetA))
					{
						this.numAttacksMade++;
					}
					else if (this.job.endIfCantShootTargetFromCurPos && !this.pawn.stances.FullBodyBusy)
					{
						Verb verb = this.pawn.TryGetAttackVerb(base.TargetA.Thing, !this.pawn.IsColonist);
						if (verb == null || !verb.CanHitTargetFrom(this.pawn.Position, base.TargetA))
						{
							base.EndJobWith(JobCondition.Incompletable);
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <init>__0;

			internal JobDriver_AttackStatic $this;

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
					this.$current = Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
				{
					Toil init = new Toil();
					init.initAction = delegate()
					{
						Pawn pawn = base.TargetThingA as Pawn;
						if (pawn != null)
						{
							this.startedIncapacitated = pawn.Downed;
						}
						this.pawn.pather.StopDead();
					};
					init.tickAction = delegate()
					{
						if (!base.TargetA.IsValid)
						{
							base.EndJobWith(JobCondition.Succeeded);
							return;
						}
						if (base.TargetA.HasThing)
						{
							Pawn pawn = base.TargetA.Thing as Pawn;
							if (base.TargetA.Thing.Destroyed || (pawn != null && !this.startedIncapacitated && pawn.Downed))
							{
								base.EndJobWith(JobCondition.Succeeded);
								return;
							}
						}
						if (this.numAttacksMade >= this.job.maxNumStaticAttacks && !this.pawn.stances.FullBodyBusy)
						{
							base.EndJobWith(JobCondition.Succeeded);
							return;
						}
						if (this.pawn.TryStartAttack(base.TargetA))
						{
							this.numAttacksMade++;
						}
						else if (this.job.endIfCantShootTargetFromCurPos && !this.pawn.stances.FullBodyBusy)
						{
							Verb verb = this.pawn.TryGetAttackVerb(base.TargetA.Thing, !this.pawn.IsColonist);
							if (verb == null || !verb.CanHitTargetFrom(this.pawn.Position, base.TargetA))
							{
								base.EndJobWith(JobCondition.Incompletable);
							}
						}
					};
					init.defaultCompleteMode = ToilCompleteMode.Never;
					this.$current = init;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				case 2u:
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
				JobDriver_AttackStatic.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_AttackStatic.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				Pawn pawn = base.TargetThingA as Pawn;
				if (pawn != null)
				{
					this.startedIncapacitated = pawn.Downed;
				}
				this.pawn.pather.StopDead();
			}

			internal void <>m__1()
			{
				if (!base.TargetA.IsValid)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				if (base.TargetA.HasThing)
				{
					Pawn pawn = base.TargetA.Thing as Pawn;
					if (base.TargetA.Thing.Destroyed || (pawn != null && !this.startedIncapacitated && pawn.Downed))
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
				}
				if (this.numAttacksMade >= this.job.maxNumStaticAttacks && !this.pawn.stances.FullBodyBusy)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				if (this.pawn.TryStartAttack(base.TargetA))
				{
					this.numAttacksMade++;
				}
				else if (this.job.endIfCantShootTargetFromCurPos && !this.pawn.stances.FullBodyBusy)
				{
					Verb verb = this.pawn.TryGetAttackVerb(base.TargetA.Thing, !this.pawn.IsColonist);
					if (verb == null || !verb.CanHitTargetFrom(this.pawn.Position, base.TargetA))
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
				}
			}
		}
	}
}
