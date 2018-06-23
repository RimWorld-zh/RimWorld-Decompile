using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050A RID: 1290
	public class Outfit : IExposable, ILoadReferenceable
	{
		// Token: 0x04000DC8 RID: 3528
		public int uniqueId;

		// Token: 0x04000DC9 RID: 3529
		public string label;

		// Token: 0x04000DCA RID: 3530
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04000DCB RID: 3531
		public static readonly Regex ValidNameRegex = new Regex("^[a-zA-Z0-9 '\\-]*$");

		// Token: 0x06001734 RID: 5940 RVA: 0x000CC377 File Offset: 0x000CA777
		public Outfit()
		{
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x000CC38B File Offset: 0x000CA78B
		public Outfit(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x000CC3AD File Offset: 0x000CA7AD
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[0]);
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x000CC3EC File Offset: 0x000CA7EC
		public string GetUniqueLoadID()
		{
			return "Outfit_" + this.label + this.uniqueId.ToString();
		}
	}
}
