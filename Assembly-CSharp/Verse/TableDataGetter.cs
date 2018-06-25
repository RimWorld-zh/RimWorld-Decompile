using System;

namespace Verse
{
	// Token: 0x02000E54 RID: 3668
	public class TableDataGetter<T>
	{
		// Token: 0x0400392E RID: 14638
		public string label;

		// Token: 0x0400392F RID: 14639
		public Func<T, string> getter;

		// Token: 0x0600566F RID: 22127 RVA: 0x002C95AB File Offset: 0x002C79AB
		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		// Token: 0x06005670 RID: 22128 RVA: 0x002C95C4 File Offset: 0x002C79C4
		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x06005671 RID: 22129 RVA: 0x002C9600 File Offset: 0x002C7A00
		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x06005672 RID: 22130 RVA: 0x002C963C File Offset: 0x002C7A3C
		public TableDataGetter(string label, Func<T, ThingDef> getter)
		{
			this.label = label;
			this.getter = delegate(T t)
			{
				ThingDef thingDef = getter(t);
				string result;
				if (thingDef == null)
				{
					result = "";
				}
				else
				{
					result = thingDef.defName;
				}
				return result;
			};
		}

		// Token: 0x06005673 RID: 22131 RVA: 0x002C9678 File Offset: 0x002C7A78
		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}
	}
}
