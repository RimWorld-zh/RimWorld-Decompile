using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005B1 RID: 1457
	public abstract class WorldComponent : IExposable
	{
		// Token: 0x06001C00 RID: 7168 RVA: 0x000EED52 File Offset: 0x000ED152
		public WorldComponent(World world)
		{
			this.world = world;
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x000EED62 File Offset: 0x000ED162
		public virtual void WorldComponentUpdate()
		{
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x000EED65 File Offset: 0x000ED165
		public virtual void WorldComponentTick()
		{
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x000EED68 File Offset: 0x000ED168
		public virtual void ExposeData()
		{
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x000EED6B File Offset: 0x000ED16B
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x040010BF RID: 4287
		public World world;
	}
}
