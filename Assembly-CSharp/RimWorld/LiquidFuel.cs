using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DB RID: 1755
	public class LiquidFuel : Filth
	{
		// Token: 0x0600262D RID: 9773 RVA: 0x00147709 File Offset: 0x00145B09
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x00147724 File Offset: 0x00145B24
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x0014773F File Offset: 0x00145B3F
		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x00147752 File Offset: 0x00145B52
		public override void Tick()
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x00147777 File Offset: 0x00145B77
		public override void ThickenFilth()
		{
			base.ThickenFilth();
			this.Refill();
		}

		// Token: 0x04001543 RID: 5443
		private int spawnTick;

		// Token: 0x04001544 RID: 5444
		private const int DryOutTime = 1500;
	}
}
