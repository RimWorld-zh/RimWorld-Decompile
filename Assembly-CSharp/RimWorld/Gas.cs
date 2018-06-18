using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C7 RID: 1735
	public class Gas : Thing
	{
		// Token: 0x0600257B RID: 9595 RVA: 0x00141554 File Offset: 0x0013F954
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

		// Token: 0x0600257C RID: 9596 RVA: 0x001415EE File Offset: 0x0013F9EE
		public override void Tick()
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed;
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00141620 File Offset: 0x0013FA20
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}

		// Token: 0x040014E0 RID: 5344
		public int destroyTick;

		// Token: 0x040014E1 RID: 5345
		public float graphicRotation = 0f;

		// Token: 0x040014E2 RID: 5346
		public float graphicRotationSpeed = 0f;
	}
}
