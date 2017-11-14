namespace Verse
{
	public class Projectile_Explosive : Projectile
	{
		private int ticksToDetonation;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToDetonation, "ticksToDetonation", 0, false);
		}

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

		protected override void Impact(Thing hitThing)
		{
			if (base.def.projectile.explosionDelay == 0)
			{
				this.Explode();
			}
			else
			{
				base.landed = true;
				this.ticksToDetonation = base.def.projectile.explosionDelay;
				GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this, base.def.projectile.damageDef, base.launcher.Faction);
			}
		}

		protected virtual void Explode()
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (base.def.projectile.explosionEffect != null)
			{
				Effecter effecter = base.def.projectile.explosionEffect.Spawn();
				effecter.Trigger(new TargetInfo(base.Position, map, false), new TargetInfo(base.Position, map, false));
				effecter.Cleanup();
			}
			IntVec3 position = base.Position;
			Map map2 = map;
			float explosionRadius = base.def.projectile.explosionRadius;
			DamageDef damageDef = base.def.projectile.damageDef;
			Thing launcher = base.launcher;
			int damageAmountBase = base.def.projectile.damageAmountBase;
			SoundDef soundExplode = base.def.projectile.soundExplode;
			ThingDef equipmentDef = base.equipmentDef;
			ThingDef def = base.def;
			ThingDef postExplosionSpawnThingDef = base.def.projectile.postExplosionSpawnThingDef;
			float postExplosionSpawnChance = base.def.projectile.postExplosionSpawnChance;
			int postExplosionSpawnThingCount = base.def.projectile.postExplosionSpawnThingCount;
			ThingDef preExplosionSpawnThingDef = base.def.projectile.preExplosionSpawnThingDef;
			GenExplosion.DoExplosion(position, map2, explosionRadius, damageDef, launcher, damageAmountBase, soundExplode, equipmentDef, def, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, base.def.projectile.applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, base.def.projectile.preExplosionSpawnChance, base.def.projectile.preExplosionSpawnThingCount, base.def.projectile.explosionChanceToStartFire, base.def.projectile.explosionDealMoreDamageAtCenter);
		}
	}
}
