using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000C90 RID: 3216
	public class RegionLinkDatabase
	{
		// Token: 0x04003016 RID: 12310
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();

		// Token: 0x06004694 RID: 18068 RVA: 0x002542B4 File Offset: 0x002526B4
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

		// Token: 0x06004695 RID: 18069 RVA: 0x00254301 File Offset: 0x00252701
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x00254318 File Offset: 0x00252718
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
