using System;
using RimWorld.Planet;

namespace RimWorld
{
	public class SitePartWorker : SiteWorkerBase
	{
		public SitePartWorker()
		{
		}

		public SitePartDef Def
		{
			get
			{
				return (SitePartDef)this.def;
			}
		}

		public virtual void SitePartWorkerTick(Site site)
		{
		}
	}
}
