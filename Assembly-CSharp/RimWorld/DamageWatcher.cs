using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000382 RID: 898
	public class DamageWatcher : IExposable
	{
		// Token: 0x04000980 RID: 2432
		private float everDamage = 0f;

		// Token: 0x04000981 RID: 2433
		private float recentDamage = 0f;

		// Token: 0x04000982 RID: 2434
		private int lastSeriousDamageTick = 0;

		// Token: 0x04000983 RID: 2435
		private const int UpdateInterval = 2000;

		// Token: 0x04000984 RID: 2436
		private const float ColonistDamageFactor = 5f;

		// Token: 0x04000985 RID: 2437
		private const float DamageFallPerInterval = 70f;

		// Token: 0x04000986 RID: 2438
		public const float MaxRecentDamage = 8000f;

		// Token: 0x04000987 RID: 2439
		private const float SeriousDamageThreshold = 7500f;

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x00083854 File Offset: 0x00081C54
		public float DamageTakenEver
		{
			get
			{
				return this.everDamage;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x00083870 File Offset: 0x00081C70
		public float DamageTakenRecently
		{
			get
			{
				return this.recentDamage;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0008388C File Offset: 0x00081C8C
		public float DaysSinceSeriousDamage
		{
			get
			{
				return Find.TickManager.TicksGame.TicksToDays() - this.lastSeriousDamageTick.TicksToDays();
			}
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x000838BC File Offset: 0x00081CBC
		public void Notify_DamageTaken(Thing damagee, float amount)
		{
			if (damagee.Faction == Faction.OfPlayer)
			{
				if (damagee.def.category == ThingCategory.Pawn && damagee.def.race.Humanlike)
				{
					amount *= 5f;
				}
				this.recentDamage += amount;
				this.everDamage += amount;
				if (this.recentDamage > 8000f)
				{
					this.recentDamage = 8000f;
				}
				if (this.recentDamage >= 7500f)
				{
					this.lastSeriousDamageTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00083968 File Offset: 0x00081D68
		public void DamageWatcherTick()
		{
			if (Find.TickManager.TicksGame % 2000 == 0)
			{
				this.recentDamage -= 70f;
				if (this.recentDamage < 0f)
				{
					this.recentDamage = 0f;
				}
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x000839BC File Offset: 0x00081DBC
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.everDamage, "everDamage", 0f, false);
			Scribe_Values.Look<float>(ref this.recentDamage, "recentDamage", 0f, false);
			Scribe_Values.Look<int>(ref this.lastSeriousDamageTick, "lastSeriousDamageTick", 0, false);
		}
	}
}
