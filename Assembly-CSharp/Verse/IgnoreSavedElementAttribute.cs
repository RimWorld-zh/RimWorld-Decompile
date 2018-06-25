using System;

namespace Verse
{
	// Token: 0x02000E4C RID: 3660
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x040038FE RID: 14590
		public string elementToIgnore;

		// Token: 0x0600564C RID: 22092 RVA: 0x002C7D7D File Offset: 0x002C617D
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}
	}
}
