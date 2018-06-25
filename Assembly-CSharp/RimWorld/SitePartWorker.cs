using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020002D5 RID: 725
	public class SitePartWorker : SiteWorkerBase
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x0006A6E4 File Offset: 0x00068AE4
		public SitePartDef Def
		{
			get
			{
				return (SitePartDef)this.def;
			}
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0006A704 File Offset: 0x00068B04
		public virtual void SitePartWorkerTick(Site site)
		{
		}
	}
}
