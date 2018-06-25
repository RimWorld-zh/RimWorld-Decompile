using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050C RID: 1292
	public class Outfit : IExposable, ILoadReferenceable
	{
		// Token: 0x04000DCB RID: 3531
		public int uniqueId;

		// Token: 0x04000DCC RID: 3532
		public string label;

		// Token: 0x04000DCD RID: 3533
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04000DCE RID: 3534
		public static readonly Regex ValidNameRegex = new Regex("^[a-zA-Z0-9 '\\-]*$");

		// Token: 0x06001737 RID: 5943 RVA: 0x000CC6C7 File Offset: 0x000CAAC7
		public Outfit()
		{
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x000CC6DB File Offset: 0x000CAADB
		public Outfit(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x000CC6FD File Offset: 0x000CAAFD
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[0]);
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x000CC73C File Offset: 0x000CAB3C
		public string GetUniqueLoadID()
		{
			return "Outfit_" + this.label + this.uniqueId.ToString();
		}
	}
}
