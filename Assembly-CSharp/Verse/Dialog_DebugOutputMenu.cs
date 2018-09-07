using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Verse
{
	public class Dialog_DebugOutputMenu : Dialog_DebugOptionLister
	{
		private List<Dialog_DebugOutputMenu.DebugOutputOption> debugOutputs = new List<Dialog_DebugOutputMenu.DebugOutputOption>();

		private const string DefaultCategory = "General";

		[CompilerGenerated]
		private static Func<Dialog_DebugOutputMenu.DebugOutputOption, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Dialog_DebugOutputMenu.DebugOutputOption, string> <>f__am$cache1;

		public Dialog_DebugOutputMenu()
		{
			this.forcePause = true;
			foreach (Type type in GenTypes.AllTypesWithAttribute<HasDebugOutputAttribute>())
			{
				MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < methods.Length; i++)
				{
					MethodInfo mi = methods[i];
					DebugOutputAttribute debugOutputAttribute;
					if (mi.TryGetAttribute(out debugOutputAttribute))
					{
						string label = GenText.SplitCamelCase(mi.Name);
						Action action = delegate()
						{
							mi.Invoke(null, null);
						};
						CategoryAttribute categoryAttribute = null;
						string category;
						if (mi.TryGetAttribute(out categoryAttribute))
						{
							category = categoryAttribute.name;
						}
						else
						{
							category = "General";
						}
						this.debugOutputs.Add(new Dialog_DebugOutputMenu.DebugOutputOption
						{
							label = label,
							category = category,
							action = action
						});
					}
				}
			}
			this.debugOutputs = (from r in this.debugOutputs
			orderby r.category, r.label
			select r).ToList<Dialog_DebugOutputMenu.DebugOutputOption>();
		}

		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		protected override void DoListingItems()
		{
			string b = null;
			foreach (Dialog_DebugOutputMenu.DebugOutputOption debugOutputOption in this.debugOutputs)
			{
				if (debugOutputOption.category != b)
				{
					base.DoLabel(debugOutputOption.category);
					b = debugOutputOption.category;
				}
				Log.openOnMessage = true;
				try
				{
					base.DebugAction(debugOutputOption.label, debugOutputOption.action);
				}
				finally
				{
					Log.openOnMessage = false;
				}
			}
		}

		[CompilerGenerated]
		private static string <Dialog_DebugOutputMenu>m__0(Dialog_DebugOutputMenu.DebugOutputOption r)
		{
			return r.category;
		}

		[CompilerGenerated]
		private static string <Dialog_DebugOutputMenu>m__1(Dialog_DebugOutputMenu.DebugOutputOption r)
		{
			return r.label;
		}

		private struct DebugOutputOption
		{
			public string label;

			public string category;

			public Action action;
		}

		[CompilerGenerated]
		private sealed class <Dialog_DebugOutputMenu>c__AnonStorey0
		{
			internal MethodInfo mi;

			public <Dialog_DebugOutputMenu>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.mi.Invoke(null, null);
			}
		}
	}
}
