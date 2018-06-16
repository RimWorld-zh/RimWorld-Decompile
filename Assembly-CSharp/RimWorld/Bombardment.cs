using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BB RID: 1723
	public class Bombardment : OrbitalStrike
	{
		// Token: 0x0600250A RID: 9482 RVA: 0x0013E099 File Offset: 0x0013C499
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.anyExplosion, "anyExplosion", false, false);
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x0013E0B4 File Offset: 0x0013C4B4
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakeBombardmentMote(base.Position, base.Map);
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x0013E0D0 File Offset: 0x0013C4D0
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

		// Token: 0x0600250D RID: 9485 RVA: 0x0013E150 File Offset: 0x0013C550
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

		// Token: 0x0600250E RID: 9486 RVA: 0x0013E1E4 File Offset: 0x0013C5E4
		private void StartRandomFire()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 25f, true)
			where x.InBounds(base.Map)
			select x).RandomElement<IntVec3>();
			FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f));
		}

		// Token: 0x04001470 RID: 5232
		private bool anyExplosion;

		// Token: 0x04001471 RID: 5233
		public const int EffectiveRadius = 23;

		// Token: 0x04001472 RID: 5234
		private const int SingleExplosionRadius = 8;

		// Token: 0x04001473 RID: 5235
		private const int ExplosionCenterRadius = 15;

		// Token: 0x04001474 RID: 5236
		public const int RandomFireRadius = 25;

		// Token: 0x04001475 RID: 5237
		private const float BombMTBRealSeconds = 0.375f;

		// Token: 0x04001476 RID: 5238
		private const int StartRandomFireEveryTicks = 20;
	}
}
