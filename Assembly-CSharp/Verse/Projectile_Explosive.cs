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
			ThingDef preExplosionSpawnThingDef = base.def.projectile.preExplosionSpawnThingDef;
			float explosionSpawnChance = base.def.projectile.explosionSpawnChance;
			GenExplosion.DoExplosion(base.Position, map, base.def.projectile.explosionRadius, base.def.projectile.damageDef, base.launcher, base.def.projectile.soundExplode, base.def, base.equipmentDef, base.def.projectile.postExplosionSpawnThingDef, base.def.projectile.explosionSpawnChance, 1, false, preExplosionSpawnThingDef, explosionSpawnChance, 1);
		}
	}
}
