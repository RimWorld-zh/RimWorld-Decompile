using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000510 RID: 1296
	public class OutfitForcedHandler : IExposable
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001747 RID: 5959 RVA: 0x000CC4CC File Offset: 0x000CA8CC
		public bool SomethingIsForced
		{
			get
			{
				return this.forcedAps.Count > 0;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x000CC4F0 File Offset: 0x000CA8F0
		public List<Apparel> ForcedApparel
		{
			get
			{
				return this.forcedAps;
			}
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x000CC50B File Offset: 0x000CA90B
		public void Reset()
		{
			this.forcedAps.Clear();
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x000CC51C File Offset: 0x000CA91C
		public bool AllowedToAutomaticallyDrop(Apparel ap)
		{
			return !this.forcedAps.Contains(ap);
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x000CC540 File Offset: 0x000CA940
		public void SetForced(Apparel ap, bool forced)
		{
			if (forced)
			{
				if (!this.forcedAps.Contains(ap))
				{
					this.forcedAps.Add(ap);
				}
			}
			else if (this.forcedAps.Contains(ap))
			{
				this.forcedAps.Remove(ap);
			}
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x000CC598 File Offset: 0x000CA998
		public void ExposeData()
		{
			Scribe_Collections.Look<Apparel>(ref this.forcedAps, "forcedAps", LookMode.Reference, new object[0]);
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x000CC5B4 File Offset: 0x000CA9B4
		public bool IsForced(Apparel ap)
		{
			bool result;
			if (ap.Destroyed)
			{
				Log.Error("Apparel was forced while Destroyed: " + ap, false);
				if (this.forcedAps.Contains(ap))
				{
					this.forcedAps.Remove(ap);
				}
				result = false;
			}
			else
			{
				result = this.forcedAps.Contains(ap);
			}
			return result;
		}

		// Token: 0x04000DD2 RID: 3538
		private List<Apparel> forcedAps = new List<Apparel>();
	}
}
