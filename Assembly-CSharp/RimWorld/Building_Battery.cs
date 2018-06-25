using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200069C RID: 1692
	[StaticConstructorOnStartup]
	public class Building_Battery : Building
	{
		// Token: 0x04001405 RID: 5125
		private int ticksToExplode = 0;

		// Token: 0x04001406 RID: 5126
		private Sustainer wickSustainer = null;

		// Token: 0x04001407 RID: 5127
		private static readonly Vector2 BarSize = new Vector2(1.3f, 0.4f);

		// Token: 0x04001408 RID: 5128
		private const float MinEnergyToExplode = 500f;

		// Token: 0x04001409 RID: 5129
		private const float EnergyToLoseWhenExplode = 400f;

		// Token: 0x0400140A RID: 5130
		private const float ExplodeChancePerDamage = 0.05f;

		// Token: 0x0400140B RID: 5131
		private static readonly Material BatteryBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f), false);

		// Token: 0x0400140C RID: 5132
		private static readonly Material BatteryBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);

		// Token: 0x060023D7 RID: 9175 RVA: 0x001344E2 File Offset: 0x001328E2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToExplode, "ticksToExplode", 0, false);
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x00134500 File Offset: 0x00132900
		public override void Draw()
		{
			base.Draw();
			CompPowerBattery comp = base.GetComp<CompPowerBattery>();
			GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
			r.center = this.DrawPos + Vector3.up * 0.1f;
			r.size = Building_Battery.BarSize;
			r.fillPercent = comp.StoredEnergy / comp.Props.storedEnergyMax;
			r.filledMat = Building_Battery.BatteryBarFilledMat;
			r.unfilledMat = Building_Battery.BatteryBarUnfilledMat;
			r.margin = 0.15f;
			Rot4 rotation = base.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			if (this.ticksToExplode > 0 && base.Spawned)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
			}
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x001345D4 File Offset: 0x001329D4
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
					float radius = Rand.Range(0.5f, 1f) * 3f;
					GenExplosion.DoExplosion(randomCell, base.Map, radius, DamageDefOf.Flame, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
					base.GetComp<CompPowerBattery>().DrawPower(400f);
				}
			}
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x00134694 File Offset: 0x00132A94
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && this.ticksToExplode == 0 && dinfo.Def == DamageDefOf.Flame && Rand.Value < 0.05f && base.GetComp<CompPowerBattery>().StoredEnergy > 500f)
			{
				this.ticksToExplode = Rand.Range(70, 150);
				this.StartWickSustainer();
			}
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x00134710 File Offset: 0x00132B10
		private void StartWickSustainer()
		{
			SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
			this.wickSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}
	}
}
