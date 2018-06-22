using System;

namespace Verse
{
	// Token: 0x02000DF0 RID: 3568
	public class Projectile_Explosive : Projectile
	{
		// Token: 0x06005010 RID: 20496 RVA: 0x00297EC4 File Offset: 0x002962C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToDetonation, "ticksToDetonation", 0, false);
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x00297EDF File Offset: 0x002962DF
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

		// Token: 0x06005012 RID: 20498 RVA: 0x00297F18 File Offset: 0x00296318
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

		// Token: 0x06005013 RID: 20499 RVA: 0x00297F88 File Offset: 0x00296388
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

		// Token: 0x0400350C RID: 13580
		private int ticksToDetonation = 0;
	}
}
