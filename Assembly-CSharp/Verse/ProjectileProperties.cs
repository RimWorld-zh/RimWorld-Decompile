using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	public class ProjectileProperties
	{
		public float speed = 4f;

		public bool flyOverhead;

		public bool alwaysFreeIntercept;

		public DamageDef damageDef;

		public int damageAmountBase = 1;

		public SoundDef soundHitThickRoof;

		public SoundDef soundExplode;

		public SoundDef soundImpactAnticipate;

		public SoundDef soundAmbient;

		public float explosionRadius;

		public int explosionDelay;

		public ThingDef preExplosionSpawnThingDef;

		public ThingDef postExplosionSpawnThingDef;

		public float explosionSpawnChance = 1f;

		[DebuggerHidden]
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			ProjectileProperties.<ConfigErrors>c__Iterator1C8 <ConfigErrors>c__Iterator1C = new ProjectileProperties.<ConfigErrors>c__Iterator1C8();
			<ConfigErrors>c__Iterator1C.<>f__this = this;
			ProjectileProperties.<ConfigErrors>c__Iterator1C8 expr_0E = <ConfigErrors>c__Iterator1C;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
