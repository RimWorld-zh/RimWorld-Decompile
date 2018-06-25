using System;

namespace Verse
{
	// Token: 0x02000CC3 RID: 3267
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x040030D1 RID: 12497
		public string internalPath;

		// Token: 0x040030D2 RID: 12498
		public T contentItem;

		// Token: 0x06004819 RID: 18457 RVA: 0x0025F4A4 File Offset: 0x0025D8A4
		public LoadedContentItem(string path, T contentItem)
		{
			this.internalPath = path;
			this.contentItem = contentItem;
		}
	}
}
