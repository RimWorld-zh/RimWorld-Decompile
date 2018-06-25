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
		// Token: 0x04000B10 RID: 2832
		private float spinPosition = 0f;

		// Token: 0x04000B11 RID: 2833
		private float spinDirection = 1f;

		// Token: 0x04000B12 RID: 2834
		[TweakValue("Graphics", 0f, 1f)]
		private static float SpinRateFactor = 0.005f;

		// Token: 0x04000B13 RID: 2835
		[TweakValue("Graphics", 0f, 3f)]
		private static float BladeOffset = 1.9f;

		// Token: 0x04000B14 RID: 2836
		[TweakValue("Graphics", 0f, 20f)]
		private static int BladeCount = 9;

		// Token: 0x04000B15 RID: 2837
		public static readonly Material BladesMat = MaterialPool.MatFrom("Things/Building/Power/WatermillGenerator/WatermillGeneratorBlades");

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x0009E714 File Offset: 0x0009CB14
		protected override float DesiredPowerOutput
		{
			get
			{
				foreach (IntVec3 c in this.WaterPoints())
				{
					if (!this.parent.Map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.MovingFluid))
					{
						return 0f;
					}
				}
				return base.DesiredPowerOutput;
			}
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0009E7AC File Offset: 0x0009CBAC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.spinPosition = Rand.Range(0f, 15f);
			Vector3 vector = Vector3.zero;
			foreach (IntVec3 intVec in this.WaterPoints())
			{
				vector += this.parent.Map.waterInfo.GetWaterMovement(intVec.ToVector3Shifted());
			}
			this.spinDirection = Mathf.Sign(Vector3.Dot(vector, this.parent.Rotation.Rotated(RotationDirection.Clockwise).FacingCell.ToVector3()));
			this.spinDirection *= Rand.RangeSeeded(0.9f, 1.1f, this.parent.thingIDNumber * 60509 + 33151);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0009E8B4 File Offset: 0x0009CCB4
		public override void CompTick()
		{
			base.CompTick();
			if (base.PowerOutput > 0.01f)
			{
				this.spinPosition = (this.spinPosition + CompPowerPlantWater.SpinRateFactor * this.spinDirection + 6.28318548f) % 6.28318548f;
			}
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0009E8F4 File Offset: 0x0009CCF4
		public IEnumerable<IntVec3> WaterPoints()
		{
			return CompPowerPlantWater.WaterPoints(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0009E924 File Offset: 0x0009CD24
		public static IEnumerable<IntVec3> WaterPoints(IntVec3 loc, Rot4 rot)
		{
			Rot4 alongAxis = rot;
			alongAxis.Rotate(RotationDirection.Counterclockwise);
			yield return loc - rot.FacingCell * 2;
			yield return loc - rot.FacingCell * 2 - alongAxis.FacingCell;
			yield break;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0009E958 File Offset: 0x0009CD58
		public IEnumerable<IntVec3> GroundPoints()
		{
			return CompPowerPlantWater.GroundPoints(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0009E988 File Offset: 0x0009CD88
		public static IEnumerable<IntVec3> GroundPoints(IntVec3 loc, Rot4 rot)
		{
			Rot4 alongAxis = rot;
			alongAxis.Rotate(RotationDirection.Counterclockwise);
			yield return loc;
			yield return loc + rot.FacingCell;
			yield return loc - alongAxis.FacingCell;
			yield return loc + rot.FacingCell - alongAxis.FacingCell;
			yield break;
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0009E9BC File Offset: 0x0009CDBC
		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 a = this.parent.TrueCenter();
			a += this.parent.Rotation.FacingCell.ToVector3() * -CompPowerPlantWater.BladeOffset;
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
