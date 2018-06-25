using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200041E RID: 1054
	[StaticConstructorOnStartup]
	public class CompPowerPlantWater : CompPowerPlant
	{
		// Token: 0x04000B13 RID: 2835
		private float spinPosition = 0f;

		// Token: 0x04000B14 RID: 2836
		private float spinRate = 1f;

		// Token: 0x04000B15 RID: 2837
		[TweakValue("Graphics", 0f, 1f)]
		private static float SpinRateFactor = 0.005f;

		// Token: 0x04000B16 RID: 2838
		[TweakValue("Graphics", 1f, 3f)]
		private static float BladeOffset = 2.36f;

		// Token: 0x04000B17 RID: 2839
		[TweakValue("Graphics", 0f, 20f)]
		private static int BladeCount = 9;

		// Token: 0x04000B18 RID: 2840
		public static readonly Material BladesMat = MaterialPool.MatFrom("Things/Building/Power/WatermillGenerator/WatermillGeneratorBlades");

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06001243 RID: 4675 RVA: 0x0009E724 File Offset: 0x0009CB24
		protected override float DesiredPowerOutput
		{
			get
			{
				foreach (IntVec3 c in this.WaterCells())
				{
					if (!this.parent.Map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.MovingFluid))
					{
						return 0f;
					}
				}
				return base.DesiredPowerOutput;
			}
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0009E7BC File Offset: 0x0009CBBC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.spinPosition = Rand.Range(0f, 15f);
			Vector3 vector = Vector3.zero;
			foreach (IntVec3 intVec in this.WaterCells())
			{
				vector += this.parent.Map.waterInfo.GetWaterMovement(intVec.ToVector3Shifted());
			}
			this.spinRate = Mathf.Sign(Vector3.Dot(vector, this.parent.Rotation.Rotated(RotationDirection.Clockwise).FacingCell.ToVector3()));
			this.spinRate *= Rand.RangeSeeded(0.9f, 1.1f, this.parent.thingIDNumber * 60509 + 33151);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0009E8C4 File Offset: 0x0009CCC4
		public override void CompTick()
		{
			base.CompTick();
			if (base.PowerOutput > 0.01f)
			{
				this.spinPosition = (this.spinPosition + CompPowerPlantWater.SpinRateFactor * this.spinRate + 6.28318548f) % 6.28318548f;
			}
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0009E904 File Offset: 0x0009CD04
		public IEnumerable<IntVec3> WaterCells()
		{
			return CompPowerPlantWater.WaterCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0009E934 File Offset: 0x0009CD34
		public static IEnumerable<IntVec3> WaterCells(IntVec3 loc, Rot4 rot)
		{
			IntVec3 perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
			yield return loc + rot.FacingCell * 3;
			yield return loc + rot.FacingCell * 3 - perpOffset;
			yield return loc + rot.FacingCell * 3 - perpOffset * 2;
			yield return loc + rot.FacingCell * 3 + perpOffset;
			yield return loc + rot.FacingCell * 3 + perpOffset * 2;
			yield break;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0009E968 File Offset: 0x0009CD68
		public IEnumerable<IntVec3> GroundCells()
		{
			return CompPowerPlantWater.GroundCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0009E998 File Offset: 0x0009CD98
		public static IEnumerable<IntVec3> GroundCells(IntVec3 loc, Rot4 rot)
		{
			IntVec3 perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
			yield return loc - rot.FacingCell;
			yield return loc - rot.FacingCell - perpOffset;
			yield return loc - rot.FacingCell + perpOffset;
			yield return loc;
			yield return loc - perpOffset;
			yield return loc + perpOffset;
			yield return loc + rot.FacingCell;
			yield return loc + rot.FacingCell - perpOffset;
			yield return loc + rot.FacingCell + perpOffset;
			yield break;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0009E9CC File Offset: 0x0009CDCC
		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 a = this.parent.TrueCenter();
			a += this.parent.Rotation.FacingCell.ToVector3() * CompPowerPlantWater.BladeOffset;
			for (int i = 0; i < CompPowerPlantWater.BladeCount; i++)
			{
				float num = this.spinPosition + 6.28318548f * (float)i / (float)CompPowerPlantWater.BladeCount;
				float x = Mathf.Abs(4f * Mathf.Sin(num));
				bool flag = num % 6.28318548f < 3.14159274f;
				Vector2 vector = new Vector2(x, 1f);
				Vector3 s = new Vector3(vector.x, 1f, vector.y);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(a + Vector3.up * 0.046875f * Mathf.Cos(num), this.parent.Rotation.AsQuat, s);
				Graphics.DrawMesh((!flag) ? MeshPool.plane10Flip : MeshPool.plane10, matrix, CompPowerPlantWater.BladesMat, 0);
			}
		}
	}
}
