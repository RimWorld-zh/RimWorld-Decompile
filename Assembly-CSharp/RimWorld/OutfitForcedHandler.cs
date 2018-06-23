using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050C RID: 1292
	public class OutfitForcedHandler : IExposable
	{
		// Token: 0x04000DCF RID: 3535
		private List<Apparel> forcedAps = new List<Apparel>();

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x000CC518 File Offset: 0x000CA918
		public bool SomethingIsForced
		{
			get
			{
				return this.forcedAps.Count > 0;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x000CC53C File Offset: 0x000CA93C
		public List<Apparel> ForcedApparel
		{
			get
			{
				return this.forcedAps;
			}
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x000CC557 File Offset: 0x000CA957
		public void Reset()
		{
			this.forcedAps.Clear();
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x000CC568 File Offset: 0x000CA968
		public bool AllowedToAutomaticallyDrop(Apparel ap)
		{
			return !this.forcedAps.Contains(ap);
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x000CC58C File Offset: 0x000CA98C
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

		// Token: 0x06001744 RID: 5956 RVA: 0x000CC5E4 File Offset: 0x000CA9E4
		public void ExposeData()
		{
			Scribe_Collections.Look<Apparel>(ref this.forcedAps, "forcedAps", LookMode.Reference, new object[0]);
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x000CC600 File Offset: 0x000CAA00
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
