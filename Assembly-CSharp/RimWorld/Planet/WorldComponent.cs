using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005B5 RID: 1461
	public abstract class WorldComponent : IExposable
	{
		// Token: 0x06001C07 RID: 7175 RVA: 0x000EEC92 File Offset: 0x000ED092
		public WorldComponent(World world)
		{
			this.world = world;
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x000EECA2 File Offset: 0x000ED0A2
		public virtual void WorldComponentUpdate()
		{
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x000EECA5 File Offset: 0x000ED0A5
		public virtual void WorldComponentTick()
		{
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x000EECA8 File Offset: 0x000ED0A8
		public virtual void ExposeData()
		{
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x000EECAB File Offset: 0x000ED0AB
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x040010C2 RID: 4290
		public World world;
	}
}
