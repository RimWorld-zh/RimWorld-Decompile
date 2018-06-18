using System;

namespace Verse
{
	// Token: 0x02000CC4 RID: 3268
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x06004805 RID: 18437 RVA: 0x0025DFB0 File Offset: 0x0025C3B0
		public LoadedContentItem(string path, T contentItem)
		{
			this.internalPath = path;
			this.contentItem = contentItem;
		}

		// Token: 0x040030C6 RID: 12486
		public string internalPath;

		// Token: 0x040030C7 RID: 12487
		public T contentItem;
	}
}
