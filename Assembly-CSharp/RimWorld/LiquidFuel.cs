using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D7 RID: 1751
	public class LiquidFuel : Filth
	{
		// Token: 0x04001541 RID: 5441
		private int spawnTick;

		// Token: 0x04001542 RID: 5442
		private const int DryOutTime = 1500;

		// Token: 0x06002625 RID: 9765 RVA: 0x001478AD File Offset: 0x00145CAD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x001478C8 File Offset: 0x00145CC8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x001478E3 File Offset: 0x00145CE3
		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x001478F6 File Offset: 0x00145CF6
		public override void Tick()
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x0014791B File Offset: 0x00145D1B
		public override void ThickenFilth()
		{
			base.ThickenFilth();
			this.Refill();
		}
	}
}
