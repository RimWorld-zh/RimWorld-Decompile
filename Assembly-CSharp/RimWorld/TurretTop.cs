using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000688 RID: 1672
	public class TurretTop
	{
		// Token: 0x040013D7 RID: 5079
		private Building_Turret parentTurret;

		// Token: 0x040013D8 RID: 5080
		private float curRotationInt = 0f;

		// Token: 0x040013D9 RID: 5081
		private int ticksUntilIdleTurn;

		// Token: 0x040013DA RID: 5082
		private int idleTurnTicksLeft;

		// Token: 0x040013DB RID: 5083
		private bool idleTurnClockwise;

		// Token: 0x040013DC RID: 5084
		private const float IdleTurnDegreesPerTick = 0.26f;

		// Token: 0x040013DD RID: 5085
		private const int IdleTurnDuration = 140;

		// Token: 0x040013DE RID: 5086
		private const int IdleTurnIntervalMin = 150;

		// Token: 0x040013DF RID: 5087
		private const int IdleTurnIntervalMax = 350;

		// Token: 0x0600236A RID: 9066 RVA: 0x0013079C File Offset: 0x0012EB9C
		public TurretTop(Building_Turret ParentTurret)
		{
			this.parentTurret = ParentTurret;
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x001307B8 File Offset: 0x0012EBB8
		// (set) Token: 0x0600236C RID: 9068 RVA: 0x001307D4 File Offset: 0x0012EBD4
		private float CurRotation
		{
			get
			{
				return this.curRotationInt;
			}
			set
			{
				this.curRotationInt = value;
				if (this.curRotationInt > 360f)
				{
					this.curRotationInt -= 360f;
				}
				if (this.curRotationInt < 0f)
				{
					this.curRotationInt += 360f;
				}
			}
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x00130830 File Offset: 0x0012EC30
		public void TurretTopTick()
		{
			LocalTargetInfo currentTarget = this.parentTurret.CurrentTarget;
			if (currentTarget.IsValid)
			{
				float curRotation = (currentTarget.Cell.ToVector3Shifted() - this.parentTurret.DrawPos).AngleFlat();
				this.CurRotation = curRotation;
				this.ticksUntilIdleTurn = Rand.RangeInclusive(150, 350);
			}
			else if (this.ticksUntilIdleTurn > 0)
			{
				this.ticksUntilIdleTurn--;
				if (this.ticksUntilIdleTurn == 0)
				{
					if (Rand.Value < 0.5f)
					{
						this.idleTurnClockwise = true;
					}
					else
					{
						this.idleTurnClockwise = false;
					}
					this.idleTurnTicksLeft = 140;
				}
			}
			else
			{
				if (this.idleTurnClockwise)
				{
					this.CurRotation += 0.26f;
				}
				else
				{
					this.CurRotation -= 0.26f;
				}
				this.idleTurnTicksLeft--;
				if (this.idleTurnTicksLeft <= 0)
				{
					this.ticksUntilIdleTurn = Rand.RangeInclusive(150, 350);
				}
			}
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x00130964 File Offset: 0x0012ED64
		public void DrawTurret()
		{
			Vector3 b = new Vector3(this.parentTurret.def.building.turretTopOffset.x, 0f, this.parentTurret.def.building.turretTopOffset.y);
			float turretTopDrawSize = this.parentTurret.def.building.turretTopDrawSize;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(this.parentTurret.DrawPos + Altitudes.AltIncVect + b, this.CurRotation.ToQuat(), new Vector3(turretTopDrawSize, 1f, turretTopDrawSize));
			Graphics.DrawMesh(MeshPool.plane10, matrix, this.parentTurret.def.building.turretTopMat, 0);
		}
	}
}
