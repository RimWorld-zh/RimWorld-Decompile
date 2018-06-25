using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C5 RID: 1733
	public class Gas : Thing
	{
		// Token: 0x040014E2 RID: 5346
		public int destroyTick;

		// Token: 0x040014E3 RID: 5347
		public float graphicRotation = 0f;

		// Token: 0x040014E4 RID: 5348
		public float graphicRotationSpeed = 0f;

		// Token: 0x06002576 RID: 9590 RVA: 0x00141A50 File Offset: 0x0013FE50
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			for (;;)
			{
				Thing gas = base.Position.GetGas(map);
				if (gas == null)
				{
					break;
				}
				gas.Destroy(DestroyMode.Vanish);
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.destroyTick = Find.TickManager.TicksGame + this.def.gas.expireSeconds.RandomInRange.SecondsToTicks();
			this.graphicRotationSpeed = Rand.Range(-this.def.gas.rotationSpeed, this.def.gas.rotationSpeed) / 60f;
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x00141AEA File Offset: 0x0013FEEA
		public override void Tick()
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x00141B1C File Offset: 0x0013FF1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}
	}
}
