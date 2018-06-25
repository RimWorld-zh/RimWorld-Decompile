using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200041F RID: 1055
	[StaticConstructorOnStartup]
	public class CompPowerPlantWind : CompPowerPlant
	{
		// Token: 0x04000B19 RID: 2841
		public int updateWeatherEveryXTicks = 250;

		// Token: 0x04000B1A RID: 2842
		private int ticksSinceWeatherUpdate;

		// Token: 0x04000B1B RID: 2843
		private float cachedPowerOutput = 0f;

		// Token: 0x04000B1C RID: 2844
		private List<IntVec3> windPathCells = new List<IntVec3>();

		// Token: 0x04000B1D RID: 2845
		private List<Thing> windPathBlockedByThings = new List<Thing>();

		// Token: 0x04000B1E RID: 2846
		private List<IntVec3> windPathBlockedCells = new List<IntVec3>();

		// Token: 0x04000B1F RID: 2847
		private float spinPosition = 0f;

		// Token: 0x04000B20 RID: 2848
		private const float MaxUsableWindIntensity = 1.5f;

		// Token: 0x04000B21 RID: 2849
		[TweakValue("Graphics", 0f, 0.1f)]
		private static float SpinRateFactor = 0.035f;

		// Token: 0x04000B22 RID: 2850
		[TweakValue("Graphics", -1f, 1f)]
		private static float HorizontalBladeOffset = -0.02f;

		// Token: 0x04000B23 RID: 2851
		[TweakValue("Graphics", 0f, 1f)]
		private static float VerticalBladeOffset = 0.7f;

		// Token: 0x04000B24 RID: 2852
		[TweakValue("Graphics", 4f, 8f)]
		private static float BladeWidth = 6.6f;

		// Token: 0x04000B25 RID: 2853
		private const float PowerReductionPercentPerObstacle = 0.2f;

		// Token: 0x04000B26 RID: 2854
		private const string TranslateWindPathIsBlockedBy = "WindTurbine_WindPathIsBlockedBy";

		// Token: 0x04000B27 RID: 2855
		private const string TranslateWindPathIsBlockedByRoof = "WindTurbine_WindPathIsBlockedByRoof";

		// Token: 0x04000B28 RID: 2856
		private static Vector2 BarSize;

		// Token: 0x04000B29 RID: 2857
		private static readonly Material WindTurbineBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		// Token: 0x04000B2A RID: 2858
		private static readonly Material WindTurbineBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);

		// Token: 0x04000B2B RID: 2859
		private static readonly Material WindTurbineBladesMat = MaterialPool.MatFrom("Things/Building/Power/WindTurbine/WindTurbineBlades");

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600124D RID: 4685 RVA: 0x0009F0D8 File Offset: 0x0009D4D8
		protected override float DesiredPowerOutput
		{
			get
			{
				return this.cachedPowerOutput;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600124E RID: 4686 RVA: 0x0009F0F4 File Offset: 0x0009D4F4
		private float PowerPercent
		{
			get
			{
				return base.PowerOutput / (-base.Props.basePowerConsumption * 1.5f);
			}
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0009F124 File Offset: 0x0009D524
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			CompPowerPlantWind.BarSize = new Vector2((float)this.parent.def.size.z - 0.95f, 0.14f);
			this.RecalculateBlockages();
			this.spinPosition = Rand.Range(0f, 15f);
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0009F17F File Offset: 0x0009D57F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceWeatherUpdate, "updateCounter", 0, false);
			Scribe_Values.Look<float>(ref this.cachedPowerOutput, "cachedPowerOutput", 0f, false);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0009F1B0 File Offset: 0x0009D5B0
		public override void CompTick()
		{
			base.CompTick();
			if (!base.PowerOn)
			{
				this.cachedPowerOutput = 0f;
			}
			else
			{
				this.ticksSinceWeatherUpdate++;
				if (this.ticksSinceWeatherUpdate >= this.updateWeatherEveryXTicks)
				{
					float num = Mathf.Min(this.parent.Map.windManager.WindSpeed, 1.5f);
					this.ticksSinceWeatherUpdate = 0;
					this.cachedPowerOutput = -(base.Props.basePowerConsumption * num);
					this.RecalculateBlockages();
					if (this.windPathBlockedCells.Count > 0)
					{
						float num2 = 0f;
						for (int i = 0; i < this.windPathBlockedCells.Count; i++)
						{
							num2 += this.cachedPowerOutput * 0.2f;
						}
						this.cachedPowerOutput -= num2;
						if (this.cachedPowerOutput < 0f)
						{
							this.cachedPowerOutput = 0f;
						}
					}
				}
				if (this.cachedPowerOutput > 0.01f)
				{
					this.spinPosition += this.PowerPercent * CompPowerPlantWind.SpinRateFactor;
				}
			}
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0009F2DC File Offset: 0x0009D6DC
		public override void PostDraw()
		{
			base.PostDraw();
			GenDraw.FillableBarRequest r = new GenDraw.FillableBarRequest
			{
				center = this.parent.DrawPos + Vector3.up * 0.1f,
				size = CompPowerPlantWind.BarSize,
				fillPercent = this.PowerPercent,
				filledMat = CompPowerPlantWind.WindTurbineBarFilledMat,
				unfilledMat = CompPowerPlantWind.WindTurbineBarUnfilledMat,
				margin = 0.15f
			};
			Rot4 rotation = this.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			Vector3 vector = this.parent.TrueCenter();
			vector += this.parent.Rotation.FacingCell.ToVector3() * CompPowerPlantWind.VerticalBladeOffset;
			vector += this.parent.Rotation.RighthandCell.ToVector3() * CompPowerPlantWind.HorizontalBladeOffset;
			vector.y += 0.046875f;
			float num = CompPowerPlantWind.BladeWidth * Mathf.Sin(this.spinPosition);
			if (num < 0f)
			{
				num *= -1f;
			}
			bool flag = this.spinPosition % 3.14159274f * 2f < 3.14159274f;
			Vector2 vector2 = new Vector2(num, 1f);
			Vector3 s = new Vector3(vector2.x, 1f, vector2.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh((!flag) ? MeshPool.plane10Flip : MeshPool.plane10, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
			vector.y -= 0.09375f;
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh((!flag) ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0009F504 File Offset: 0x0009D904
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompInspectStringExtra());
			if (this.windPathBlockedCells.Count > 0)
			{
				stringBuilder.AppendLine();
				Thing thing = null;
				if (this.windPathBlockedByThings != null)
				{
					thing = this.windPathBlockedByThings[0];
				}
				if (thing != null)
				{
					stringBuilder.Append("WindTurbine_WindPathIsBlockedBy".Translate() + " " + thing.Label);
				}
				else
				{
					stringBuilder.Append("WindTurbine_WindPathIsBlockedByRoof".Translate());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0009F5A4 File Offset: 0x0009D9A4
		private void RecalculateBlockages()
		{
			if (this.windPathCells.Count == 0)
			{
				IEnumerable<IntVec3> collection = WindTurbineUtility.CalculateWindCells(this.parent.Position, this.parent.Rotation, this.parent.def.size);
				this.windPathCells.AddRange(collection);
			}
			this.windPathBlockedCells.Clear();
			this.windPathBlockedByThings.Clear();
			for (int i = 0; i < this.windPathCells.Count; i++)
			{
				IntVec3 intVec = this.windPathCells[i];
				if (this.parent.Map.roofGrid.Roofed(intVec))
				{
					this.windPathBlockedByThings.Add(null);
					this.windPathBlockedCells.Add(intVec);
				}
				else
				{
					List<Thing> list = this.parent.Map.thingGrid.ThingsListAt(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (thing.def.blockWind)
						{
							this.windPathBlockedByThings.Add(thing);
							this.windPathBlockedCells.Add(intVec);
							break;
						}
					}
				}
			}
		}
	}
}
