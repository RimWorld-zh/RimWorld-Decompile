using System;

namespace Verse
{
	// Token: 0x02000E52 RID: 3666
	public class TableDataGetter<T>
	{
		// Token: 0x0600564B RID: 22091 RVA: 0x002C7683 File Offset: 0x002C5A83
		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		// Token: 0x0600564C RID: 22092 RVA: 0x002C769C File Offset: 0x002C5A9C
		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x002C76D8 File Offset: 0x002C5AD8
		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x002C7714 File Offset: 0x002C5B14
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

		// Token: 0x0600564F RID: 22095 RVA: 0x002C7750 File Offset: 0x002C5B50
		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}

		// Token: 0x04003917 RID: 14615
		public string label;

		// Token: 0x04003918 RID: 14616
		public Func<T, string> getter;
	}
}
