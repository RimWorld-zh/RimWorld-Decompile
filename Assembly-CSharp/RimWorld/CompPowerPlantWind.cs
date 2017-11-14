using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompPowerPlantWind : CompPowerPlant
	{
		public int updateWeatherEveryXTicks = 250;

		private int ticksSinceWeatherUpdate;

		private float cachedPowerOutput;

		private List<IntVec3> windPathCells = new List<IntVec3>();

		private List<Thing> windPathBlockedByThings = new List<Thing>();

		private List<IntVec3> windPathBlockedCells = new List<IntVec3>();

		private float spinPosition;

		private const float MaxUsableWindIntensity = 1.5f;

		private const float SpinRateFactor = 0.05f;

		private const float PowerReductionPercentPerObstacle = 0.2f;

		private const string TranslateWindPathIsBlockedBy = "WindTurbine_WindPathIsBlockedBy";

		private const string TranslateWindPathIsBlockedByRoof = "WindTurbine_WindPathIsBlockedByRoof";

		private static Vector2 BarSize;

		private static readonly Material WindTurbineBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		private static readonly Material WindTurbineBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);

		private static readonly Material WindTurbineBladesMat = MaterialPool.MatFrom("Things/Building/Power/WindTurbine/WindTurbineBlades");

		protected override float DesiredPowerOutput
		{
			get
			{
				return this.cachedPowerOutput;
			}
		}

		private float PowerPercent
		{
			get
			{
				return (float)(base.PowerOutput / ((0.0 - base.Props.basePowerConsumption) * 1.5));
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			CompPowerPlantWind.BarSize = new Vector2((float)((float)base.parent.def.size.z - 0.949999988079071), 0.14f);
			this.RecalculateBlockages();
			this.spinPosition = Rand.Range(0f, 15f);
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceWeatherUpdate, "updateCounter", 0, false);
			Scribe_Values.Look<float>(ref this.cachedPowerOutput, "cachedPowerOutput", 0f, false);
		}

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
					float num = Mathf.Min(base.parent.Map.windManager.WindSpeed, 1.5f);
					this.ticksSinceWeatherUpdate = 0;
					this.cachedPowerOutput = (float)(0.0 - base.Props.basePowerConsumption * num);
					this.RecalculateBlockages();
					if (this.windPathBlockedCells.Count > 0)
					{
						float num2 = 0f;
						for (int i = 0; i < this.windPathBlockedCells.Count; i++)
						{
							num2 = (float)(num2 + this.cachedPowerOutput * 0.20000000298023224);
						}
						this.cachedPowerOutput -= num2;
						if (this.cachedPowerOutput < 0.0)
						{
							this.cachedPowerOutput = 0f;
						}
					}
				}
				if (this.cachedPowerOutput > 0.0099999997764825821)
				{
					this.spinPosition += (float)(this.PowerPercent * 0.05000000074505806);
				}
			}
		}

		public override void PostDraw()
		{
			base.PostDraw();
			GenDraw.FillableBarRequest fillableBarRequest = default(GenDraw.FillableBarRequest);
			fillableBarRequest.center = base.parent.DrawPos + Vector3.up * 0.1f;
			fillableBarRequest.size = CompPowerPlantWind.BarSize;
			fillableBarRequest.fillPercent = this.PowerPercent;
			fillableBarRequest.filledMat = CompPowerPlantWind.WindTurbineBarFilledMat;
			fillableBarRequest.unfilledMat = CompPowerPlantWind.WindTurbineBarUnfilledMat;
			fillableBarRequest.margin = 0.15f;
			GenDraw.FillableBarRequest r = fillableBarRequest;
			Rot4 rotation = base.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			Vector3 vector = base.parent.TrueCenter();
			vector += base.parent.Rotation.FacingCell.ToVector3() * 0.7f;
			vector.y += 0.046875f;
			float num = (float)(4.0 * Mathf.Sin(this.spinPosition));
			if (num < 0.0)
			{
				num = (float)(num * -1.0);
			}
			bool flag = this.spinPosition % 3.1415927410125732 * 2.0 < 3.1415927410125732;
			Vector2 vector2 = new Vector2(num, 1f);
			Vector3 s = new Vector3(vector2.x, 1f, vector2.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector, base.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh((!flag) ? MeshPool.plane10Flip : MeshPool.plane10, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
			vector.y -= 0.09375f;
			matrix.SetTRS(vector, base.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh((!flag) ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
		}

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

		private void RecalculateBlockages()
		{
			if (this.windPathCells.Count == 0)
			{
				IEnumerable<IntVec3> collection = WindTurbineUtility.CalculateWindCells(base.parent.Position, base.parent.Rotation, base.parent.def.size);
				this.windPathCells.AddRange(collection);
			}
			this.windPathBlockedCells.Clear();
			this.windPathBlockedByThings.Clear();
			for (int i = 0; i < this.windPathCells.Count; i++)
			{
				IntVec3 intVec = this.windPathCells[i];
				if (base.parent.Map.roofGrid.Roofed(intVec))
				{
					this.windPathBlockedByThings.Add(null);
					this.windPathBlockedCells.Add(intVec);
				}
				else
				{
					List<Thing> list = base.parent.Map.thingGrid.ThingsListAt(intVec);
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
