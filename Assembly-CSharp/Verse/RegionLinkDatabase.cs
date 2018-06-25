using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000C8F RID: 3215
	public class RegionLinkDatabase
	{
		// Token: 0x0400300F RID: 12303
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();

		// Token: 0x06004694 RID: 18068 RVA: 0x00253FD4 File Offset: 0x002523D4
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

		// Token: 0x06004695 RID: 18069 RVA: 0x00254021 File Offset: 0x00252421
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x00254038 File Offset: 0x00252438
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
