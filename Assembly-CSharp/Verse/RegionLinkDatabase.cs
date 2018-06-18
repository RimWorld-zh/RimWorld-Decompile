using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000C90 RID: 3216
	public class RegionLinkDatabase
	{
		// Token: 0x06004688 RID: 18056 RVA: 0x00252B28 File Offset: 0x00250F28
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

		// Token: 0x06004689 RID: 18057 RVA: 0x00252B75 File Offset: 0x00250F75
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x00252B8C File Offset: 0x00250F8C
		public void DebugLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ulong, RegionLink> keyValuePair in this.links)
			{
				stringBuilder.AppendLine(keyValuePair.ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04003005 RID: 12293
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();
	}
}
