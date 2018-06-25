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
	public class JobDriver_Skygaze : JobDriver
	{
		public JobDriver_Skygaze()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil gaze = new Toil();
			gaze.initAction = delegate()
			{
				this.pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp;
			};
			gaze.tickAction = delegate()
			{
				float num = this.pawn.Map.gameConditionManager.AggregateSkyGazeJoyGainFactor(this.pawn.Map);
				Pawn pawn = this.pawn;
				float extraJoyGainFactor = num;
				JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, null);
			};
			gaze.defaultCompleteMode = ToilCompleteMode.Delay;
			gaze.defaultDuration = this.job.def.joyDuration;
			gaze.FailOn(() => this.pawn.Position.Roofed(this.pawn.Map));
			gaze.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			yield return gaze;
			yield break;
		}

		public override string GetReport()
		{
			string result;
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
			{
				result = "WatchingEclipse".Translate();
			}
			else if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Aurora))
			{
				result = "WatchingAurora".Translate();
			}
			else
			{
				float num = GenCelestial.CurCelestialSunGlow(base.Map);
				if (num < 0.1f)
				{
					result = "Stargazing".Translate();
				}
				else if (num < 0.65f)
				{
					if (GenLocalDate.DayPercent(this.pawn) < 0.5f)
					{
						result = "WatchingSunrise".Translate();
					}
					else
					{
						result = "WatchingSunset".Translate();
					}
				}
				else
				{
					result = "CloudWatching".Translate();
				}
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <gaze>__0;

			internal JobDriver_Skygaze $this;

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
					this.$current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					gaze = new Toil();
					gaze.initAction = delegate()
					{
						this.pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp;
					};
					gaze.tickAction = delegate()
					{
						float num2 = this.pawn.Map.gameConditionManager.AggregateSkyGazeJoyGainFactor(this.pawn.Map);
						Pawn pawn = this.pawn;
						float extraJoyGainFactor = num2;
						JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, null);
					};
					gaze.defaultCompleteMode = ToilCompleteMode.Delay;
					gaze.defaultDuration = this.job.def.joyDuration;
					gaze.FailOn(() => this.pawn.Position.Roofed(this.pawn.Map));
					gaze.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
					this.$current = gaze;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
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
				JobDriver_Skygaze.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Skygaze.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				this.pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp;
			}

			internal void <>m__1()
			{
				float num = this.pawn.Map.gameConditionManager.AggregateSkyGazeJoyGainFactor(this.pawn.Map);
				Pawn pawn = this.pawn;
				float extraJoyGainFactor = num;
				JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, null);
			}

			internal bool <>m__2()
			{
				return this.pawn.Position.Roofed(this.pawn.Map);
			}

			internal bool <>m__3()
			{
				return !JoyUtility.EnjoyableOutsideNow(this.pawn, null);
			}
		}
	}
}
