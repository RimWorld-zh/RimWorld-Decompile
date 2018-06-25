using System;

namespace Verse
{
	// Token: 0x02000CC4 RID: 3268
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x040030D8 RID: 12504
		public string internalPath;

		// Token: 0x040030D9 RID: 12505
		public T contentItem;

		// Token: 0x06004819 RID: 18457 RVA: 0x0025F784 File Offset: 0x0025DB84
		public LoadedContentItem(string path, T contentItem)
		{
			this.internalPath = path;
			this.contentItem = contentItem;
		}
	}
}
