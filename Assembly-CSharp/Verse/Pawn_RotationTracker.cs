using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D59 RID: 3417
	public class Pawn_RotationTracker : IExposable
	{
		// Token: 0x06004C9E RID: 19614 RVA: 0x0027F639 File Offset: 0x0027DA39
		public Pawn_RotationTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x0027F649 File Offset: 0x0027DA49
		public void Notify_Spawned()
		{
			this.UpdateRotation();
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x0027F654 File Offset: 0x0027DA54
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
								this.FaceTarget(target);
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

		// Token: 0x06004CA1 RID: 19617 RVA: 0x0027F7CC File Offset: 0x0027DBCC
		public void RotationTrackerTick()
		{
			this.UpdateRotation();
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x0027F7D8 File Offset: 0x0027DBD8
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

		// Token: 0x06004CA3 RID: 19619 RVA: 0x0027F88C File Offset: 0x0027DC8C
		public void FaceCell(IntVec3 c)
		{
			if (!(c == this.pawn.Position))
			{
				float angle = (c - this.pawn.Position).ToVector3().AngleFlat();
				this.pawn.Rotation = Pawn_RotationTracker.RotFromAngleBiased(angle);
			}
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x0027F8E8 File Offset: 0x0027DCE8
		public void Face(Vector3 p)
		{
			if (!(p == this.pawn.DrawPos))
			{
				float angle = (p - this.pawn.DrawPos).AngleFlat();
				this.pawn.Rotation = Pawn_RotationTracker.RotFromAngleBiased(angle);
			}
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x0027F93C File Offset: 0x0027DD3C
		public void FaceTarget(LocalTargetInfo target)
		{
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
				}
				else
				{
					this.Face(target.Thing.DrawPos);
				}
			}
			else if (this.pawn.Position.AdjacentTo8Way(target.Cell))
			{
				if (DebugViewSettings.drawPawnRotatorTarget)
				{
					this.pawn.Map.debugDrawer.FlashCell(this.pawn.Position, 0.2f, "jbloc", 50);
					GenDraw.DrawLineBetween(this.pawn.Position.ToVector3Shifted(), target.Cell.ToVector3Shifted());
				}
				this.FaceAdjacentCell(target.Cell);
			}
			else if (target.Cell.IsValid && target.Cell != this.pawn.Position)
			{
				this.Face(target.Cell.ToVector3());
			}
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x0027FBE0 File Offset: 0x0027DFE0
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

		// Token: 0x06004CA7 RID: 19623 RVA: 0x0027FC52 File Offset: 0x0027E052
		public void ExposeData()
		{
		}

		// Token: 0x04003321 RID: 13089
		private Pawn pawn;
	}
}
