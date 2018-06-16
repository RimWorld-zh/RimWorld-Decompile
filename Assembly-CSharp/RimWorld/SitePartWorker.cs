using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020002D3 RID: 723
	public class SitePartWorker : SiteWorkerBase
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x0006A530 File Offset: 0x00068930
		public SitePartDef Def
		{
			get
			{
				return (SitePartDef)this.def;
			}
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0006A550 File Offset: 0x00068950
		public virtual void SitePartWorkerTick(Site site)
		{
		}
	}
}
