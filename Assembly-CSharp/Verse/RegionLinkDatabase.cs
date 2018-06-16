using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000C91 RID: 3217
	public class RegionLinkDatabase
	{
		// Token: 0x0600468A RID: 18058 RVA: 0x00252B50 File Offset: 0x00250F50
		public RegionLink LinkFrom(EdgeSpan span)
		{
			ulong key = span.UniqueHashCode();
			RegionLink regionLink;
			if (!this.links.TryGetValue(key, out regionLink))
			{
				regionLink = new RegionLink();
				regionLink.span = span;
				this.links.Add(key, regionLink);
			}
			return regionLink;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x00252B9D File Offset: 0x00250F9D
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x00252BB4 File Offset: 0x00250FB4
		public void DebugLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ulong, RegionLink> keyValuePair in this.links)
			{
				stringBuilder.AppendLine(keyValuePair.ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04003007 RID: 12295
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();
	}
}
