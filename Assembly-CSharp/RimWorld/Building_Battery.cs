using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Building_Battery : Building
	{
		private int ticksToExplode = 0;

		private Sustainer wickSustainer = null;

		private static readonly Vector2 BarSize = new Vector2(1.3f, 0.4f);

		private const float MinEnergyToExplode = 500f;

		private const float EnergyToLoseWhenExplode = 400f;

		private const float ExplodeChancePerDamage = 0.05f;

		private static readonly Material BatteryBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f), false);

		private static readonly Material BatteryBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToExplode, "ticksToExplode", 0, false);
		}

		public override void Draw()
		{
			base.Draw();
			CompPowerBattery comp = base.GetComp<CompPowerBattery>();
			GenDraw.FillableBarRequest r = new GenDraw.FillableBarRequest
			{
				center = this.DrawPos + Vector3.up * 0.1f,
				size = Building_Battery.BarSize,
				fillPercent = comp.StoredEnergy / comp.Props.storedEnergyMax,
				filledMat = Building_Battery.BatteryBarFilledMat,
				unfilledMat = Building_Battery.BatteryBarUnfilledMat,
				margin = 0.15f
			};
			Rot4 rotation = base.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			if (this.ticksToExplode > 0 && base.Spawned)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
			}
		}

		public override void Tick()
		{
			base.Tick();
			if (this.ticksToExplode > 0)
			{
				if (this.wickSustainer == null)
				{
					this.StartWickSustainer();
				}
				else
				{
					this.wickSustainer.Maintain();
				}
				this.ticksToExplode--;
				if (this.ticksToExplode == 0)
				{
					IntVec3 randomCell = this.OccupiedRect().RandomCell;
					float radius = (float)(Rand.Range(0.5f, 1f) * 3.0);
					GenExplosion.DoExplosion(randomCell, base.Map, radius, DamageDefOf.Flame, null, -1, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
					base.GetComp<CompPowerBattery>().DrawPower(400f);
				}
			}
		}

		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && this.ticksToExplode == 0 && dinfo.Def == DamageDefOf.Flame && Rand.Value < 0.05000000074505806 && base.GetComp<CompPowerBattery>().StoredEnergy > 500.0)
			{
				this.ticksToExplode = Rand.Range(70, 150);
				this.StartWickSustainer();
			}
		}

		private void StartWickSustainer()
		{
			SoundInfo info = SoundInfo.InMap((Thing)this, MaintenanceType.PerTick);
			this.wickSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}
	}
}
