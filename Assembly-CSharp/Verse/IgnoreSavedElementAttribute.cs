using System;

namespace Verse
{
	// Token: 0x02000E4B RID: 3659
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x040038F6 RID: 14582
		public string elementToIgnore;

		// Token: 0x0600564C RID: 22092 RVA: 0x002C7B91 File Offset: 0x002C5F91
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}
	}
}
