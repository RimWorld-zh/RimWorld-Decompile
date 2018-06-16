using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050E RID: 1294
	public class Outfit : IExposable, ILoadReferenceable
	{
		// Token: 0x0600173C RID: 5948 RVA: 0x000CC32B File Offset: 0x000CA72B
		public Outfit()
		{
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x000CC33F File Offset: 0x000CA73F
		public Outfit(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x000CC361 File Offset: 0x000CA761
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[0]);
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x000CC3A0 File Offset: 0x000CA7A0
		public string GetUniqueLoadID()
		{
			return "Outfit_" + this.label + this.uniqueId.ToString();
		}

		// Token: 0x04000DCB RID: 3531
		public int uniqueId;

		// Token: 0x04000DCC RID: 3532
		public string label;

		// Token: 0x04000DCD RID: 3533
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04000DCE RID: 3534
		public static readonly Regex ValidNameRegex = new Regex("^[a-zA-Z0-9 '\\-]*$");
	}
}
