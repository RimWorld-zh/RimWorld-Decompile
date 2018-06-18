using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D5C RID: 3420
	public class Pawn_RotationTracker : IExposable
	{
		// Token: 0x06004C8A RID: 19594 RVA: 0x0027E099 File Offset: 0x0027C499
		public Pawn_RotationTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x0027E0A9 File Offset: 0x0027C4A9
		public void Notify_Spawned()
		{
			this.UpdateRotation();
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x0027E0B4 File Offset: 0x0027C4B4
		public void UpdateRotation()
		{
			if (!this.pawn.Destroyed)
			{
				if (!this.pawn.jobs.HandlingFacing)
				{
					if (this.pawn.pather.Moving)
					{
						if (this.pawn.pather.curPath != null && this.pawn.pather.curPath.NodesLeftCount >= 1)
						{
							this.FaceAdjacentCell(this.pawn.pather.nextCell);
						}
					}
					else
					{
						Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
						if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
						{
							if (stance_Busy.focusTarg.HasThing)
							{
								this.Face(stance_Busy.focusTarg.Thing.DrawPos);
							}
							else
							{
								this.FaceCell(stance_Busy.focusTarg.Cell);
							}
						}
						else
						{
							if (this.pawn.jobs.curJob != null)
							{
								LocalTargetInfo target = this.pawn.CurJob.GetTarget(this.pawn.jobs.curDriver.rotateToFace);
								if (target.HasThing)
								{
									bool flag = false;
									IntVec3 c = default(IntVec3);
									CellRect cellRect = target.Thing.OccupiedRect();
									for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
									{
										for (int j = cellRect.minX; j <= cellRect.maxX; j++)
										{
											if (this.pawn.Position == new IntVec3(j, 0, i))
											{
												this.Face(target.Thing.DrawPos);
												return;
											}
										}
									}
									for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
									{
										for (int l = cellRect.minX; l <= cellRect.maxX; l++)
										{
											IntVec3 intVec = new IntVec3(l, 0, k);
											if (intVec.AdjacentToCardinal(this.pawn.Position))
											{
												this.FaceAdjacentCell(intVec);
												return;
											}
											if (intVec.AdjacentTo8Way(this.pawn.Position))
											{
												flag = true;
												c = intVec;
											}
										}
									}
									if (flag)
									{
										if (DebugViewSettings.drawPawnRotatorTarget)
										{
											this.pawn.Map.debugDrawer.FlashCell(this.pawn.Position, 0.6f, "jbthing", 50);
											GenDraw.DrawLineBetween(this.pawn.Position.ToVector3Shifted(), c.ToVector3Shifted());
										}
										this.FaceAdjacentCell(c);
										return;
									}
									this.Face(target.Thing.DrawPos);
									return;
								}
								else
								{
									if (this.pawn.Position.AdjacentTo8Way(target.Cell))
									{
										if (DebugViewSettings.drawPawnRotatorTarget)
										{
											this.pawn.Map.debugDrawer.FlashCell(this.pawn.Position, 0.2f, "jbloc", 50);
											GenDraw.DrawLineBetween(this.pawn.Position.ToVector3Shifted(), target.Cell.ToVector3Shifted());
										}
										this.FaceAdjacentCell(target.Cell);
										return;
									}
									if (target.Cell.IsValid && target.Cell != this.pawn.Position)
									{
										this.Face(target.Cell.ToVector3());
										return;
									}
								}
							}
							if (this.pawn.Drafted)
							{
								this.pawn.Rotation = Rot4.South;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x0027E4C1 File Offset: 0x0027C8C1
		public void RotationTrackerTick()
		{
			this.UpdateRotation();
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x0027E4CC File Offset: 0x0027C8CC
		private void FaceAdjacentCell(IntVec3 c)
		{
			if (!(c == this.pawn.Position))
			{
				IntVec3 intVec = c - this.pawn.Position;
				if (intVec.x > 0)
				{
					this.pawn.Rotation = Rot4.East;
				}
				else if (intVec.x < 0)
				{
					this.pawn.Rotation = Rot4.West;
				}
				else if (intVec.z > 0)
				{
					this.pawn.Rotation = Rot4.North;
				}
				else
				{
					this.pawn.Rotation = Rot4.South;
				}
			}
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x0027E580 File Offset: 0x0027C980
		public void FaceCell(IntVec3 c)
		{
			if (!(c == this.pawn.Position))
			{
				float angle = (c - this.pawn.Position).ToVector3().AngleFlat();
				this.pawn.Rotation = Pawn_RotationTracker.RotFromAngleBiased(angle);
			}
		}

		// Token: 0x06004C90 RID: 19600 RVA: 0x0027E5DC File Offset: 0x0027C9DC
		public void Face(Vector3 p)
		{
			if (!(p == this.pawn.DrawPos))
			{
				float angle = (p - this.pawn.DrawPos).AngleFlat();
				this.pawn.Rotation = Pawn_RotationTracker.RotFromAngleBiased(angle);
			}
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x0027E630 File Offset: 0x0027CA30
		public static Rot4 RotFromAngleBiased(float angle)
		{
			Rot4 result;
			if (angle < 30f)
			{
				result = Rot4.North;
			}
			else if (angle < 150f)
			{
				result = Rot4.East;
			}
			else if (angle < 210f)
			{
				result = Rot4.South;
			}
			else if (angle < 330f)
			{
				result = Rot4.West;
			}
			else
			{
				result = Rot4.North;
			}
			return result;
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x0027E6A2 File Offset: 0x0027CAA2
		public void ExposeData()
		{
		}

		// Token: 0x04003316 RID: 13078
		private Pawn pawn;
	}
}
