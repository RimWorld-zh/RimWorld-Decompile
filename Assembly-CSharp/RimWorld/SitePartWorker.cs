using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020002D3 RID: 723
	public class SitePartWorker : SiteWorkerBase
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x0006A598 File Offset: 0x00068998
		public SitePartDef Def
		{
			get
			{
				return (SitePartDef)this.def;
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0006A5B8 File Offset: 0x000689B8
		public virtual void SitePartWorkerTick(Site site)
		{
		}
	}
}
