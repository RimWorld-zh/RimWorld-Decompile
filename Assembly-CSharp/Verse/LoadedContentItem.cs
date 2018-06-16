using System;

namespace Verse
{
	// Token: 0x02000CC5 RID: 3269
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x06004807 RID: 18439 RVA: 0x0025DFD8 File Offset: 0x0025C3D8
		public LoadedContentItem(string path, T contentItem)
		{
			this.internalPath = path;
			this.contentItem = contentItem;
		}

		// Token: 0x040030C8 RID: 12488
		public string internalPath;

		// Token: 0x040030C9 RID: 12489
		public T contentItem;
	}
}
