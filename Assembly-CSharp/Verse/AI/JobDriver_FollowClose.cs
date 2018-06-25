using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A34 RID: 2612
	public class JobDriver_FollowClose : JobDriver
	{
		// Token: 0x040024FC RID: 9468
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x040024FD RID: 9469
		private const int CheckPathIntervalTicks = 30;

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x060039FA RID: 14842 RVA: 0x001EA4B4 File Offset: 0x001E88B4
		private Pawn Followee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x060039FB RID: 14843 RVA: 0x001EA4E4 File Offset: 0x001E88E4
		private bool CurrentlyWalkingToFollowee
		{
			get
			{
				return this.pawn.pather.Moving && this.pawn.pather.Destination == this.Followee;
			}
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x001EA534 File Offset: 0x001E8934
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x001EA54C File Offset: 0x001E894C
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.job.followRadius <= 0f)
			{
				Log.Error("Follow radius is <= 0. pawn=" + this.pawn.ToStringSafe<Pawn>(), false);
				this.job.followRadius = 10f;
			}
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x001EA5A4 File Offset: 0x001E89A4
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

		// Token: 0x060039FF RID: 14847 RVA: 0x001EA5D0 File Offset: 0x001E89D0
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x001EA600 File Offset: 0x001E8A00
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

		// Token: 0x06003A01 RID: 14849 RVA: 0x001EA6E4 File Offset: 0x001E8AE4
		private static bool NearFollowee(Pawn follower, Pawn followee, float radius)
		{
			return follower.Position.AdjacentTo8WayOrInside(followee.Position) || (follower.Position.InHorDistOf(followee.Position, radius) && GenSight.LineOfSight(follower.Position, followee.Position, follower.Map, false, null, 0, 0));
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x001EA750 File Offset: 0x001E8B50
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
	}
}
