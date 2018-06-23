using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B7 RID: 1719
	public class Bombardment : OrbitalStrike
	{
		// Token: 0x0400146E RID: 5230
		private bool anyExplosion;

		// Token: 0x0400146F RID: 5231
		public const int EffectiveRadius = 23;

		// Token: 0x04001470 RID: 5232
		private const int SingleExplosionRadius = 8;

		// Token: 0x04001471 RID: 5233
		private const int ExplosionCenterRadius = 15;

		// Token: 0x04001472 RID: 5234
		public const int RandomFireRadius = 25;

		// Token: 0x04001473 RID: 5235
		private const float BombMTBRealSeconds = 0.375f;

		// Token: 0x04001474 RID: 5236
		private const int StartRandomFireEveryTicks = 20;

		// Token: 0x06002504 RID: 9476 RVA: 0x0013E259 File Offset: 0x0013C659
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.anyExplosion, "anyExplosion", false, false);
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x0013E274 File Offset: 0x0013C674
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakeBombardmentMote(base.Position, base.Map);
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x0013E290 File Offset: 0x0013C690
		public override void Tick()
		{
			base.Tick();
			if (!base.Destroyed)
			{
				if (Rand.MTBEventOccurs(0.375f, 60f, 1f))
				{
					this.CreateRandomExplosion();
				}
				if (Find.TickManager.TicksGame % 20 == 0)
				{
					this.StartRandomFire();
				}
				if (base.TicksPassed >= this.duration / 2 && !this.anyExplosion)
				{
					this.CreateRandomExplosion();
				}
			}
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x0013E310 File Offset: 0x0013C710
		private void CreateRandomExplosion()
		{
			IntVec3 intVec = (from x in GenRadial.RadialCellsAround(base.Position, 15f, true)
			where x.InBounds(base.Map)
			select x).RandomElement<IntVec3>();
			IntVec3 center = intVec;
			Map map = base.Map;
			float radius = 8f;
			DamageDef bomb = DamageDefOf.Bomb;
			Thing instigator = this.instigator;
			ThingDef def = this.def;
			ThingDef weaponDef = this.weaponDef;
			GenExplosion.DoExplosion(center, map, radius, bomb, instigator, -1, null, weaponDef, def, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			this.anyExplosion = true;
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x0013E3A4 File Offset: 0x0013C7A4
		private void StartRandomFire()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 25f, true)
			where x.InBounds(base.Map)
			select x).RandomElement<IntVec3>();
			FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f));
		}
	}
}
