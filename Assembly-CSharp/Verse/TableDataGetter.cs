using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	public class TableDataGetter<T>
	{
		public string label;

		public Func<T, string> getter;

		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		public TableDataGetter(string label, Func<T, ThingDef> getter)
		{
			this.label = label;
			this.getter = delegate(T t)
			{
				ThingDef thingDef = getter(t);
				if (thingDef == null)
				{
					return string.Empty;
				}
				return thingDef.defName;
			};
		}

		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}

		[CompilerGenerated]
		private sealed class <TableDataGetter>c__AnonStorey0
		{
			internal Func<T, float> getter;

			public <TableDataGetter>c__AnonStorey0()
			{
			}

			internal string <>m__0(T t)
			{
				return this.getter(t).ToString("F0");
			}
		}

		[CompilerGenerated]
		private sealed class <TableDataGetter>c__AnonStorey1
		{
			internal Func<T, int> getter;

			public <TableDataGetter>c__AnonStorey1()
			{
			}

			internal string <>m__0(T t)
			{
				return this.getter(t).ToString("F0");
			}
		}

		[CompilerGenerated]
		private sealed class <TableDataGetter>c__AnonStorey2
		{
			internal Func<T, ThingDef> getter;

			public <TableDataGetter>c__AnonStorey2()
			{
			}

			internal string <>m__0(T t)
			{
				ThingDef thingDef = this.getter(t);
				if (thingDef == null)
				{
					return string.Empty;
				}
				return thingDef.defName;
			}
		}

		[CompilerGenerated]
		private sealed class <TableDataGetter>c__AnonStorey3
		{
			internal Func<T, object> getter;

			public <TableDataGetter>c__AnonStorey3()
			{
			}

			internal string <>m__0(T t)
			{
				return this.getter(t).ToString();
			}
		}
	}
}
