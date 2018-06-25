using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005B3 RID: 1459
	public abstract class WorldComponent : IExposable
	{
		// Token: 0x040010C3 RID: 4291
		public World world;

		// Token: 0x06001C03 RID: 7171 RVA: 0x000EF10A File Offset: 0x000ED50A
		public WorldComponent(World world)
		{
			this.world = world;
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x000EF11A File Offset: 0x000ED51A
		public virtual void WorldComponentUpdate()
		{
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000EF11D File Offset: 0x000ED51D
		public virtual void WorldComponentTick()
		{
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000EF120 File Offset: 0x000ED520
		public virtual void ExposeData()
		{
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000EF123 File Offset: 0x000ED523
		public virtual void FinalizeInit()
		{
		}
	}
}
