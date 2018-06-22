using System;

namespace Verse
{
	// Token: 0x02000CC1 RID: 3265
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x06004816 RID: 18454 RVA: 0x0025F3C8 File Offset: 0x0025D7C8
		public LoadedContentItem(string path, T contentItem)
		{
			this.internalPath = path;
			this.contentItem = contentItem;
		}

		// Token: 0x040030D1 RID: 12497
		public string internalPath;

		// Token: 0x040030D2 RID: 12498
		public T contentItem;
	}
}
