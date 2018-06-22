using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C3 RID: 1731
	public class Gas : Thing
	{
		// Token: 0x06002573 RID: 9587 RVA: 0x001416A0 File Offset: 0x0013FAA0
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

		// Token: 0x06002574 RID: 9588 RVA: 0x0014173A File Offset: 0x0013FB3A
		public override void Tick()
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed;
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x0014176C File Offset: 0x0013FB6C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}

		// Token: 0x040014DE RID: 5342
		public int destroyTick;

		// Token: 0x040014DF RID: 5343
		public float graphicRotation = 0f;

		// Token: 0x040014E0 RID: 5344
		public float graphicRotationSpeed = 0f;
	}
}
