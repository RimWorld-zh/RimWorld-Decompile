using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D9 RID: 1753
	public class LiquidFuel : Filth
	{
		// Token: 0x04001545 RID: 5445
		private int spawnTick;

		// Token: 0x04001546 RID: 5446
		private const int DryOutTime = 1500;

		// Token: 0x06002628 RID: 9768 RVA: 0x00147C5D File Offset: 0x0014605D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x00147C78 File Offset: 0x00146078
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x00147C93 File Offset: 0x00146093
		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x00147CA6 File Offset: 0x001460A6
		public override void Tick()
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x00147CCB File Offset: 0x001460CB
		public override void ThickenFilth()
		{
			base.ThickenFilth();
			this.Refill();
		}
	}
}
