using System;

namespace Verse
{
	// Token: 0x02000E4B RID: 3659
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x0600562A RID: 22058 RVA: 0x002C5E75 File Offset: 0x002C4275
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}

		// Token: 0x040038E9 RID: 14569
		public string elementToIgnore;
	}
}
