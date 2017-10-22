using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompPowerPlantSolar : CompPowerPlant
	{
		private const float FullSunPower = 1700f;

		private const float NightPower = 0f;

		private static readonly Vector2 BarSize = new Vector2(2.3f, 0.14f);

		private static readonly Material PowerPlantSolarBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		private static readonly Material PowerPlantSolarBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);

		protected override float DesiredPowerOutput
		{
			get
			{
				return Mathf.Lerp(0f, 1700f, base.parent.Map.skyManager.CurSkyGlow) * this.RoofedPowerOutputFactor;
			}
		}

		private float RoofedPowerOutputFactor
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 item in base.parent.OccupiedRect())
				{
					num++;
					if (base.parent.Map.roofGrid.Roofed(item))
					{
						num2++;
					}
				}
				return (float)(num - num2) / (float)num;
			}
		}

		public override void PostDraw()
		{
			base.PostDraw();
			GenDraw.FillableBarRequest r = new GenDraw.FillableBarRequest
			{
				center = base.parent.DrawPos + Vector3.up * 0.1f,
				size = CompPowerPlantSolar.BarSize,
				fillPercent = (float)(base.PowerOutput / 1700.0),
				filledMat = CompPowerPlantSolar.PowerPlantSolarBarFilledMat,
				unfilledMat = CompPowerPlantSolar.PowerPlantSolarBarUnfilledMat,
				margin = 0.15f
			};
			Rot4 rotation = base.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
		}
	}
}
