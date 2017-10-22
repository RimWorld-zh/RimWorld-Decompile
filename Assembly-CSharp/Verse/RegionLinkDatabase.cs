using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	public class RegionLinkDatabase
	{
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();

		public RegionLink LinkFrom(EdgeSpan span)
		{
			ulong key = span.UniqueHashCode();
			RegionLink regionLink = default(RegionLink);
			if (!this.links.TryGetValue(key, out regionLink))
			{
				regionLink = new RegionLink();
				regionLink.span = span;
				this.links.Add(key, regionLink);
			}
			return regionLink;
		}

		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		public void DebugLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Dictionary<ulong, RegionLink>.Enumerator enumerator = this.links.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					stringBuilder.AppendLine(enumerator.Current.ToString());
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
