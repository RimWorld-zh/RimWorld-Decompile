using System;

namespace Verse
{
	// Token: 0x02000DF4 RID: 3572
	public class Projectile_Explosive : Projectile
	{
		// Token: 0x06004FFE RID: 20478 RVA: 0x00296908 File Offset: 0x00294D08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToDetonation, "ticksToDetonation", 0, false);
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x00296923 File Offset: 0x00294D23
		public override void Tick()
		{
			base.Tick();
			if (this.ticksToDetonation > 0)
			{
				this.ticksToDetonation--;
				if (this.ticksToDetonation <= 0)
				{
					this.Explode();
				}
			}
		}

		// Token: 0x06005000 RID: 20480 RVA: 0x0029695C File Offset: 0x00294D5C
		protected override void Impact(Thing hitThing)
		{
			if (this.def.projectile.explosionDelay == 0)
			{
				this.Explode();
			}
			else
			{
				this.landed = true;
				this.ticksToDetonation = this.def.projectile.explosionDelay;
				GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this, this.def.projectile.damageDef, this.launcher.Faction);
			}
		}

		// Token: 0x06005001 RID: 20481 RVA: 0x002969CC File Offset: 0x00294DCC
		protected virtual void Explode()
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (this.def.projectile.explosionEffect != null)
			{
				Effecter effecter = this.def.projectile.explosionEffect.Spawn();
				effecter.Trigger(new TargetInfo(base.Position, map, false), new TargetInfo(base.Position, map, false));
				effecter.Cleanup();
			}
			IntVec3 position = base.Position;
			Map map2 = map;
			float explosionRadius = this.def.projectile.explosionRadius;
			DamageDef damageDef = this.def.projectile.damageDef;
			Thing launcher = this.launcher;
			int damageAmount = this.def.projectile.DamageAmount;
			SoundDef soundExplode = this.def.projectile.soundExplode;
			ThingDef equipmentDef = this.equipmentDef;
			ThingDef def = this.def;
			Thing thing = this.intendedTarget.Thing;
			ThingDef postExplosionSpawnThingDef = this.def.projectile.postExplosionSpawnThingDef;
			float postExplosionSpawnChance = this.def.projectile.postExplosionSpawnChance;
			int postExplosionSpawnThingCount = this.def.projectile.postExplosionSpawnThingCount;
			ThingDef preExplosionSpawnThingDef = this.def.projectile.preExplosionSpawnThingDef;
			GenExplosion.DoExplosion(position, map2, explosionRadius, damageDef, launcher, damageAmount, soundExplode, equipmentDef, def, thing, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, this.def.projectile.applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, this.def.projectile.preExplosionSpawnChance, this.def.projectile.preExplosionSpawnThingCount, this.def.projectile.explosionChanceToStartFire, this.def.projectile.explosionDamageFalloff);
		}

		// Token: 0x04003507 RID: 13575
		private int ticksToDetonation = 0;
	}
}
