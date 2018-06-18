using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000510 RID: 1296
	public class OutfitForcedHandler : IExposable
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x000CC520 File Offset: 0x000CA920
		public bool SomethingIsForced
		{
			get
			{
				return this.forcedAps.Count > 0;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x000CC544 File Offset: 0x000CA944
		public List<Apparel> ForcedApparel
		{
			get
			{
				return this.forcedAps;
			}
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x000CC55F File Offset: 0x000CA95F
		public void Reset()
		{
			this.forcedAps.Clear();
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x000CC570 File Offset: 0x000CA970
		public bool AllowedToAutomaticallyDrop(Apparel ap)
		{
			return !this.forcedAps.Contains(ap);
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x000CC594 File Offset: 0x000CA994
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

		// Token: 0x0600174D RID: 5965 RVA: 0x000CC5EC File Offset: 0x000CA9EC
		public void ExposeData()
		{
			Scribe_Collections.Look<Apparel>(ref this.forcedAps, "forcedAps", LookMode.Reference, new object[0]);
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x000CC608 File Offset: 0x000CAA08
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
