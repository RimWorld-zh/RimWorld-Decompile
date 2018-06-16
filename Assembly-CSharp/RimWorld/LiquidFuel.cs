using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DB RID: 1755
	public class LiquidFuel : Filth
	{
		// Token: 0x0600262B RID: 9771 RVA: 0x00147691 File Offset: 0x00145A91
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x001476AC File Offset: 0x00145AAC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x001476C7 File Offset: 0x00145AC7
		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x001476DA File Offset: 0x00145ADA
		public override void Tick()
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x001476FF File Offset: 0x00145AFF
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
