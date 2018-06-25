using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050E RID: 1294
	public class OutfitForcedHandler : IExposable
	{
		// Token: 0x04000DCF RID: 3535
		private List<Apparel> forcedAps = new List<Apparel>();

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x000CC668 File Offset: 0x000CAA68
		public bool SomethingIsForced
		{
			get
			{
				return this.forcedAps.Count > 0;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x000CC68C File Offset: 0x000CAA8C
		public List<Apparel> ForcedApparel
		{
			get
			{
				return this.forcedAps;
			}
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x000CC6A7 File Offset: 0x000CAAA7
		public void Reset()
		{
			this.forcedAps.Clear();
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x000CC6B8 File Offset: 0x000CAAB8
		public bool AllowedToAutomaticallyDrop(Apparel ap)
		{
			return !this.forcedAps.Contains(ap);
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x000CC6DC File Offset: 0x000CAADC
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

		// Token: 0x06001748 RID: 5960 RVA: 0x000CC734 File Offset: 0x000CAB34
		public void ExposeData()
		{
			Scribe_Collections.Look<Apparel>(ref this.forcedAps, "forcedAps", LookMode.Reference, new object[0]);
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x000CC750 File Offset: 0x000CAB50
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
	}
}
