using System.Collections.Generic;

namespace Verse
{
	public class ProjectileProperties
	{
		public float speed = 4f;

		public bool flyOverhead = false;

		public bool alwaysFreeIntercept = false;

		public DamageDef damageDef = null;

		public int damageAmountBase = 1;

		public SoundDef soundHitThickRoof = null;

		public SoundDef soundExplode = null;

		public SoundDef soundImpactAnticipate = null;

		public SoundDef soundAmbient = null;

		public float explosionRadius = 0f;

		public int explosionDelay = 0;

		public ThingDef preExplosionSpawnThingDef = null;

		public float preExplosionSpawnChance = 1f;

		public int preExplosionSpawnThingCount = 1;

		public ThingDef postExplosionSpawnThingDef = null;

		public float postExplosionSpawnChance = 1f;

		public int postExplosionSpawnThingCount = 1;

		public bool applyDamageToExplosionCellsNeighbors;

		public float explosionChanceToStartFire;

		public bool explosionDealMoreDamageAtCenter;

		public EffecterDef explosionEffect;

		public bool ai_IsIncendiary = false;

		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (!this.alwaysFreeIntercept)
				yield break;
			if (!this.flyOverhead)
				yield break;
			yield return "alwaysFreeIntercept and flyOverhead are both true";
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
