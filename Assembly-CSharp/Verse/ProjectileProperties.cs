using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B1E RID: 2846
	public class ProjectileProperties
	{
		// Token: 0x0400285A RID: 10330
		public float speed = 4f;

		// Token: 0x0400285B RID: 10331
		public bool flyOverhead = false;

		// Token: 0x0400285C RID: 10332
		public bool alwaysFreeIntercept = false;

		// Token: 0x0400285D RID: 10333
		public DamageDef damageDef = null;

		// Token: 0x0400285E RID: 10334
		private int damageAmountBase = -1;

		// Token: 0x0400285F RID: 10335
		public float stoppingPower = 0f;

		// Token: 0x04002860 RID: 10336
		public SoundDef soundHitThickRoof = null;

		// Token: 0x04002861 RID: 10337
		public SoundDef soundExplode = null;

		// Token: 0x04002862 RID: 10338
		public SoundDef soundImpactAnticipate = null;

		// Token: 0x04002863 RID: 10339
		public SoundDef soundAmbient = null;

		// Token: 0x04002864 RID: 10340
		public float explosionRadius = 0f;

		// Token: 0x04002865 RID: 10341
		public int explosionDelay = 0;

		// Token: 0x04002866 RID: 10342
		public ThingDef preExplosionSpawnThingDef = null;

		// Token: 0x04002867 RID: 10343
		public float preExplosionSpawnChance = 1f;

		// Token: 0x04002868 RID: 10344
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04002869 RID: 10345
		public ThingDef postExplosionSpawnThingDef = null;

		// Token: 0x0400286A RID: 10346
		public float postExplosionSpawnChance = 1f;

		// Token: 0x0400286B RID: 10347
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x0400286C RID: 10348
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x0400286D RID: 10349
		public float explosionChanceToStartFire;

		// Token: 0x0400286E RID: 10350
		public bool explosionDamageFalloff;

		// Token: 0x0400286F RID: 10351
		public EffecterDef explosionEffect;

		// Token: 0x04002870 RID: 10352
		public bool ai_IsIncendiary = false;

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06003EC5 RID: 16069 RVA: 0x00211388 File Offset: 0x0020F788
		public int DamageAmount
		{
			get
			{
				int result;
				if (this.damageAmountBase != -1)
				{
					result = this.damageAmountBase;
				}
				else if (this.damageDef != null)
				{
					result = this.damageDef.defaultDamage;
				}
				else
				{
					Log.ErrorOnce("Failed to find sane damage amount", 91094882, false);
					result = 1;
				}
				return result;
			}
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x002113E4 File Offset: 0x0020F7E4
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (this.alwaysFreeIntercept && this.flyOverhead)
			{
				yield return "alwaysFreeIntercept and flyOverhead are both true";
			}
			if (this.damageAmountBase == -1 && this.damageDef != null && this.damageDef.defaultDamage == -1)
			{
				yield return "no damage amount specified for projectile";
			}
			yield break;
		}
	}
}
