using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse.AI
{
	public class JobDriver_FollowClose : JobDriver
	{
		private const TargetIndex FolloweeInd = TargetIndex.A;

		private const int CheckPathIntervalTicks = 30;

		public JobDriver_FollowClose()
		{
		}

		private Pawn Followee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private bool CurrentlyWalkingToFollowee
		{
			get
			{
				return this.pawn.pather.Moving && this.pawn.pather.Destination == this.Followee;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.job.followRadius <= 0f)
			{
				Log.Error("Follow radius is <= 0. pawn=" + this.pawn.ToStringSafe<Pawn>(), false);
				this.job.followRadius = 10f;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				tickAction = delegate()
				{
					Pawn followee = this.Followee;
					float followRadius = this.job.followRadius;
					if (!this.pawn.pather.Moving || this.pawn.IsHashIntervalTick(30))
					{
						bool flag = false;
						if (this.CurrentlyWalkingToFollowee)
						{
							if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, followRadius))
							{
								flag = true;
							}
						}
						else
						{
							float radius = followRadius * 1.2f;
							if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, radius))
							{
								flag = true;
							}
							else
							{
								if (!this.pawn.CanReach(followee, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
								{
									base.EndJobWith(JobCondition.Incompletable);
									return;
								}
								this.pawn.pather.StartPath(followee, PathEndMode.Touch);
								this.locomotionUrgencySameAs = null;
							}
						}
						if (flag)
						{
							if (JobDriver_FollowClose.NearDestinationOrNotMoving(this.pawn, followee, followRadius))
							{
								base.EndJobWith(JobCondition.Succeeded);
							}
							else
							{
								IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
								if (!this.pawn.pather.Moving || this.pawn.pather.Destination.HasThing || !this.pawn.pather.Destination.Cell.InHorDistOf(lastPassableCellInPath, followRadius))
								{
									IntVec3 intVec = CellFinder.RandomClosewalkCellNear(lastPassableCellInPath, base.Map, Mathf.FloorToInt(followRadius), null);
									if (intVec == this.pawn.Position)
									{
										base.EndJobWith(JobCondition.Succeeded);
									}
									else if (intVec.IsValid && this.pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
									{
										this.pawn.pather.StartPath(intVec, PathEndMode.OnCell);
										this.locomotionUrgencySameAs = followee;
									}
									else
									{
										base.EndJobWith(JobCondition.Incompletable);
									}
								}
							}
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		public static bool FarEnoughAndPossibleToStartJob(Pawn follower, Pawn followee, float radius)
		{
			bool result;
			if (radius <= 0f)
			{
				string text = "Checking follow job with radius <= 0. pawn=" + follower.ToStringSafe<Pawn>();
				if (follower.mindState != null && follower.mindState.duty != null)
				{
					text = text + " duty=" + follower.mindState.duty.def;
				}
				Log.ErrorOnce(text, follower.thingIDNumber ^ 843254009, false);
				result = false;
			}
			else if (!follower.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				result = false;
			}
			else
			{
				float radius2 = radius * 1.2f;
				result = (!JobDriver_FollowClose.NearFollowee(follower, followee, radius2) || (!JobDriver_FollowClose.NearDestinationOrNotMoving(follower, followee, radius2) && follower.CanReach(followee.pather.LastPassableCellInPath, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)));
			}
			return result;
		}

		private static bool NearFollowee(Pawn follower, Pawn followee, float radius)
		{
			return follower.Position.AdjacentTo8WayOrInside(followee.Position) || (follower.Position.InHorDistOf(followee.Position, radius) && GenSight.LineOfSight(follower.Position, followee.Position, follower.Map, false, null, 0, 0));
		}

		private static bool NearDestinationOrNotMoving(Pawn follower, Pawn followee, float radius)
		{
			bool result;
			if (!followee.pather.Moving)
			{
				result = true;
			}
			else
			{
				IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
				result = (!lastPassableCellInPath.IsValid || follower.Position.AdjacentTo8WayOrInside(lastPassableCellInPath) || follower.Position.InHorDistOf(lastPassableCellInPath, radius));
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <follow>__0;

			internal JobDriver_FollowClose $this;

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
				{
					this.FailOnDespawnedOrNull(TargetIndex.A);
					Toil follow = new Toil();
					follow.tickAction = delegate()
					{
						Pawn followee = base.Followee;
						float followRadius = this.job.followRadius;
						if (!this.pawn.pather.Moving || this.pawn.IsHashIntervalTick(30))
						{
							bool flag = false;
							if (base.CurrentlyWalkingToFollowee)
							{
								if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, followRadius))
								{
									flag = true;
								}
							}
							else
							{
								float radius = followRadius * 1.2f;
								if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, radius))
								{
									flag = true;
								}
								else
								{
									if (!this.pawn.CanReach(followee, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
									{
										base.EndJobWith(JobCondition.Incompletable);
										return;
									}
									this.pawn.pather.StartPath(followee, PathEndMode.Touch);
									this.locomotionUrgencySameAs = null;
								}
							}
							if (flag)
							{
								if (JobDriver_FollowClose.NearDestinationOrNotMoving(this.pawn, followee, followRadius))
								{
									base.EndJobWith(JobCondition.Succeeded);
								}
								else
								{
									IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
									if (!this.pawn.pather.Moving || this.pawn.pather.Destination.HasThing || !this.pawn.pather.Destination.Cell.InHorDistOf(lastPassableCellInPath, followRadius))
									{
										IntVec3 intVec = CellFinder.RandomClosewalkCellNear(lastPassableCellInPath, base.Map, Mathf.FloorToInt(followRadius), null);
										if (intVec == this.pawn.Position)
										{
											base.EndJobWith(JobCondition.Succeeded);
										}
										else if (intVec.IsValid && this.pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
										{
											this.pawn.pather.StartPath(intVec, PathEndMode.OnCell);
											this.locomotionUrgencySameAs = followee;
										}
										else
										{
											base.EndJobWith(JobCondition.Incompletable);
										}
									}
								}
							}
						}
					};
					follow.defaultCompleteMode = ToilCompleteMode.Never;
					this.$current = follow;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
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
				JobDriver_FollowClose.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_FollowClose.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				Pawn followee = base.Followee;
				float followRadius = this.job.followRadius;
				if (!this.pawn.pather.Moving || this.pawn.IsHashIntervalTick(30))
				{
					bool flag = false;
					if (base.CurrentlyWalkingToFollowee)
					{
						if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, followRadius))
						{
							flag = true;
						}
					}
					else
					{
						float radius = followRadius * 1.2f;
						if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, radius))
						{
							flag = true;
						}
						else
						{
							if (!this.pawn.CanReach(followee, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
							{
								base.EndJobWith(JobCondition.Incompletable);
								return;
							}
							this.pawn.pather.StartPath(followee, PathEndMode.Touch);
							this.locomotionUrgencySameAs = null;
						}
					}
					if (flag)
					{
						if (JobDriver_FollowClose.NearDestinationOrNotMoving(this.pawn, followee, followRadius))
						{
							base.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
							if (!this.pawn.pather.Moving || this.pawn.pather.Destination.HasThing || !this.pawn.pather.Destination.Cell.InHorDistOf(lastPassableCellInPath, followRadius))
							{
								IntVec3 intVec = CellFinder.RandomClosewalkCellNear(lastPassableCellInPath, base.Map, Mathf.FloorToInt(followRadius), null);
								if (intVec == this.pawn.Position)
								{
									base.EndJobWith(JobCondition.Succeeded);
								}
								else if (intVec.IsValid && this.pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
								{
									this.pawn.pather.StartPath(intVec, PathEndMode.OnCell);
									this.locomotionUrgencySameAs = followee;
								}
								else
								{
									base.EndJobWith(JobCondition.Incompletable);
								}
							}
						}
					}
				}
			}
		}
	}
}
