using System;

namespace Verse
{
	// Token: 0x02000E49 RID: 3657
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x040038F6 RID: 14582
		public string elementToIgnore;

		// Token: 0x06005648 RID: 22088 RVA: 0x002C7A65 File Offset: 0x002C5E65
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}
	}
}
