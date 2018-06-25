using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C5 RID: 1733
	public class Gas : Thing
	{
		// Token: 0x040014DE RID: 5342
		public int destroyTick;

		// Token: 0x040014DF RID: 5343
		public float graphicRotation = 0f;

		// Token: 0x040014E0 RID: 5344
		public float graphicRotationSpeed = 0f;

		// Token: 0x06002577 RID: 9591 RVA: 0x001417F0 File Offset: 0x0013FBF0
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

		// Token: 0x06002578 RID: 9592 RVA: 0x0014188A File Offset: 0x0013FC8A
		public override void Tick()
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x001418BC File Offset: 0x0013FCBC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}
	}
}
