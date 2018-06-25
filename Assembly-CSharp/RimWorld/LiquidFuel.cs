using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D9 RID: 1753
	public class LiquidFuel : Filth
	{
		// Token: 0x04001541 RID: 5441
		private int spawnTick;

		// Token: 0x04001542 RID: 5442
		private const int DryOutTime = 1500;

		// Token: 0x06002629 RID: 9769 RVA: 0x001479FD File Offset: 0x00145DFD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x00147A18 File Offset: 0x00145E18
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x00147A33 File Offset: 0x00145E33
		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x00147A46 File Offset: 0x00145E46
		public override void Tick()
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x00147A6B File Offset: 0x00145E6B
		public override void ThickenFilth()
		{
			base.ThickenFilth();
			this.Refill();
		}
	}
}
