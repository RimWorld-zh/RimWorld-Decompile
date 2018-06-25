using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005B3 RID: 1459
	public abstract class WorldComponent : IExposable
	{
		// Token: 0x040010BF RID: 4287
		public World world;

		// Token: 0x06001C04 RID: 7172 RVA: 0x000EEEA2 File Offset: 0x000ED2A2
		public WorldComponent(World world)
		{
			this.world = world;
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000EEEB2 File Offset: 0x000ED2B2
		public virtual void WorldComponentUpdate()
		{
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000EEEB5 File Offset: 0x000ED2B5
		public virtual void WorldComponentTick()
		{
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000EEEB8 File Offset: 0x000ED2B8
		public virtual void ExposeData()
		{
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x000EEEBB File Offset: 0x000ED2BB
		public virtual void FinalizeInit()
		{
		}
	}
}
