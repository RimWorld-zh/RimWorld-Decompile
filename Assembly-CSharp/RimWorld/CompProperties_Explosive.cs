using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000244 RID: 580
	public class CompProperties_Explosive : CompProperties
	{
		// Token: 0x06000A73 RID: 2675 RVA: 0x0005EE40 File Offset: 0x0005D240
		public CompProperties_Explosive()
		{
			this.compClass = typeof(CompExplosive);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0005EED3 File Offset: 0x0005D2D3
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.explosiveDamageType == null)
			{
				this.explosiveDamageType = DamageDefOf.Bomb;
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0005EEF4 File Offset: 0x0005D2F4
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return e;
			}
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return "CompExplosive requires Normal ticker type";
			}
			yield break;
		}

		// Token: 0x0400046F RID: 1135
		public float explosiveRadius = 1.9f;

		// Token: 0x04000470 RID: 1136
		public DamageDef explosiveDamageType;

		// Token: 0x04000471 RID: 1137
		public int damageAmountBase = -1;

		// Token: 0x04000472 RID: 1138
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04000473 RID: 1139
		public float postExplosionSpawnChance;

		// Token: 0x04000474 RID: 1140
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04000475 RID: 1141
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04000476 RID: 1142
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04000477 RID: 1143
		public float preExplosionSpawnChance;

		// Token: 0x04000478 RID: 1144
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04000479 RID: 1145
		public float chanceToStartFire;

		// Token: 0x0400047A RID: 1146
		public bool damageFalloff;

		// Token: 0x0400047B RID: 1147
		public float explosiveExpandPerStackcount;

		// Token: 0x0400047C RID: 1148
		public float explosiveExpandPerFuel;

		// Token: 0x0400047D RID: 1149
		public EffecterDef explosionEffect;

		// Token: 0x0400047E RID: 1150
		public SoundDef explosionSound;

		// Token: 0x0400047F RID: 1151
		public DamageDef startWickOnDamageTaken = null;

		// Token: 0x04000480 RID: 1152
		public float startWickHitPointsPercent = 0.2f;

		// Token: 0x04000481 RID: 1153
		public IntRange wickTicks = new IntRange(140, 150);

		// Token: 0x04000482 RID: 1154
		public float wickScale = 1f;

		// Token: 0x04000483 RID: 1155
		public float chanceNeverExplodeFromDamage = 0f;

		// Token: 0x04000484 RID: 1156
		public float destroyThingOnExplosionSize = 0f;

		// Token: 0x04000485 RID: 1157
		public DamageDef requiredDamageTypeToExplode = null;
	}
}
