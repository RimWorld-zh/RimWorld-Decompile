using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005B5 RID: 1461
	public abstract class WorldComponent : IExposable
	{
		// Token: 0x06001C09 RID: 7177 RVA: 0x000EECFE File Offset: 0x000ED0FE
		public WorldComponent(World world)
		{
			this.world = world;
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x000EED0E File Offset: 0x000ED10E
		public virtual void WorldComponentUpdate()
		{
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x000EED11 File Offset: 0x000ED111
		public virtual void WorldComponentTick()
		{
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x000EED14 File Offset: 0x000ED114
		public virtual void ExposeData()
		{
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x000EED17 File Offset: 0x000ED117
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x040010C2 RID: 4290
		public World world;
	}
}
