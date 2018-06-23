using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000C8D RID: 3213
	public class RegionLinkDatabase
	{
		// Token: 0x0400300F RID: 12303
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();

		// Token: 0x06004691 RID: 18065 RVA: 0x00253EF8 File Offset: 0x002522F8
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

		// Token: 0x06004692 RID: 18066 RVA: 0x00253F45 File Offset: 0x00252345
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x00253F5C File Offset: 0x0025235C
		public void DebugLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ulong, RegionLink> keyValuePair in this.links)
			{
				stringBuilder.AppendLine(keyValuePair.ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}
	}
}
