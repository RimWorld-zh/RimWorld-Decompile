using System;

namespace Verse
{
	// Token: 0x02000E53 RID: 3667
	public class TableDataGetter<T>
	{
		// Token: 0x04003926 RID: 14630
		public string label;

		// Token: 0x04003927 RID: 14631
		public Func<T, string> getter;

		// Token: 0x0600566F RID: 22127 RVA: 0x002C93BF File Offset: 0x002C77BF
		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		// Token: 0x06005670 RID: 22128 RVA: 0x002C93D8 File Offset: 0x002C77D8
		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x06005671 RID: 22129 RVA: 0x002C9414 File Offset: 0x002C7814
		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x06005672 RID: 22130 RVA: 0x002C9450 File Offset: 0x002C7850
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

		// Token: 0x06005673 RID: 22131 RVA: 0x002C948C File Offset: 0x002C788C
		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}
	}
}
