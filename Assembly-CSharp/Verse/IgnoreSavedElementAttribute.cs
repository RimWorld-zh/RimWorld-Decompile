using System;

namespace Verse
{
	// Token: 0x02000E4A RID: 3658
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x06005628 RID: 22056 RVA: 0x002C5E75 File Offset: 0x002C4275
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}

		// Token: 0x040038E7 RID: 14567
		public string elementToIgnore;
	}
}
