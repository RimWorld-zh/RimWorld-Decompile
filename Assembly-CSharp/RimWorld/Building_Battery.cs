using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200069A RID: 1690
	[StaticConstructorOnStartup]
	public class Building_Battery : Building
	{
		// Token: 0x060023D4 RID: 9172 RVA: 0x0013412A File Offset: 0x0013252A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToExplode, "ticksToExplode", 0, false);
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x00134148 File Offset: 0x00132548
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

		// Token: 0x060023D6 RID: 9174 RVA: 0x0013421C File Offset: 0x0013261C
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

		// Token: 0x060023D7 RID: 9175 RVA: 0x001342DC File Offset: 0x001326DC
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && this.ticksToExplode == 0 && dinfo.Def == DamageDefOf.Flame && Rand.Value < 0.05f && base.GetComp<CompPowerBattery>().StoredEnergy > 500f)
			{
				this.ticksToExplode = Rand.Range(70, 150);
				this.StartWickSustainer();
			}
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x00134358 File Offset: 0x00132758
		private void StartWickSustainer()
		{
			SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
			this.wickSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		// Token: 0x04001401 RID: 5121
		private int ticksToExplode = 0;

		// Token: 0x04001402 RID: 5122
		private Sustainer wickSustainer = null;

		// Token: 0x04001403 RID: 5123
		private static readonly Vector2 BarSize = new Vector2(1.3f, 0.4f);

		// Token: 0x04001404 RID: 5124
		private const float MinEnergyToExplode = 500f;

		// Token: 0x04001405 RID: 5125
		private const float EnergyToLoseWhenExplode = 400f;

		// Token: 0x04001406 RID: 5126
		private const float ExplodeChancePerDamage = 0.05f;

		// Token: 0x04001407 RID: 5127
		private static readonly Material BatteryBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f), false);

		// Token: 0x04001408 RID: 5128
		private static readonly Material BatteryBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
