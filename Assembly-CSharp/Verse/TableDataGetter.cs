using System;

namespace Verse
{
	// Token: 0x02000E51 RID: 3665
	public class TableDataGetter<T>
	{
		// Token: 0x0600566B RID: 22123 RVA: 0x002C9293 File Offset: 0x002C7693
		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		// Token: 0x0600566C RID: 22124 RVA: 0x002C92AC File Offset: 0x002C76AC
		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x0600566D RID: 22125 RVA: 0x002C92E8 File Offset: 0x002C76E8
		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x0600566E RID: 22126 RVA: 0x002C9324 File Offset: 0x002C7724
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

		// Token: 0x0600566F RID: 22127 RVA: 0x002C9360 File Offset: 0x002C7760
		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}

		// Token: 0x04003926 RID: 14630
		public string label;

		// Token: 0x04003927 RID: 14631
		public Func<T, string> getter;
	}
}
