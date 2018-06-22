using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000E31 RID: 3633
	public class Dialog_DebugOutputMenu : Dialog_DebugOptionLister
	{
		// Token: 0x06005610 RID: 22032 RVA: 0x002C5EE4 File Offset: 0x002C42E4
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

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06005611 RID: 22033 RVA: 0x002C606C File Offset: 0x002C446C
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005612 RID: 22034 RVA: 0x002C6084 File Offset: 0x002C4484
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

		// Token: 0x040038D8 RID: 14552
		private List<Dialog_DebugOutputMenu.DebugOutputOption> debugOutputs = new List<Dialog_DebugOutputMenu.DebugOutputOption>();

		// Token: 0x040038D9 RID: 14553
		private const string DefaultCategory = "General";

		// Token: 0x02000E32 RID: 3634
		private struct DebugOutputOption
		{
			// Token: 0x040038DC RID: 14556
			public string label;

			// Token: 0x040038DD RID: 14557
			public string category;

			// Token: 0x040038DE RID: 14558
			public Action action;
		}
	}
}
