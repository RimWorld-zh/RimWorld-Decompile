using System;

namespace Verse
{
	// Token: 0x02000DF2 RID: 3570
	public class Projectile_Explosive : Projectile
	{
		// Token: 0x0400350C RID: 13580
		private int ticksToDetonation = 0;

		// Token: 0x06005014 RID: 20500 RVA: 0x00297FF0 File Offset: 0x002963F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToDetonation, "ticksToDetonation", 0, false);
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x0029800B File Offset: 0x0029640B
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

		// Token: 0x06005016 RID: 20502 RVA: 0x00298044 File Offset: 0x00296444
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

		// Token: 0x06005017 RID: 20503 RVA: 0x002980B4 File Offset: 0x002964B4
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
	}
}
