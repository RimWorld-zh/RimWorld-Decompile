using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000E33 RID: 3635
	public class Dialog_DebugOutputMenu : Dialog_DebugOptionLister
	{
		// Token: 0x040038D8 RID: 14552
		private List<Dialog_DebugOutputMenu.DebugOutputOption> debugOutputs = new List<Dialog_DebugOutputMenu.DebugOutputOption>();

		// Token: 0x040038D9 RID: 14553
		private const string DefaultCategory = "General";

		// Token: 0x06005614 RID: 22036 RVA: 0x002C6010 File Offset: 0x002C4410
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

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06005615 RID: 22037 RVA: 0x002C6198 File Offset: 0x002C4598
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005616 RID: 22038 RVA: 0x002C61B0 File Offset: 0x002C45B0
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

		// Token: 0x02000E34 RID: 3636
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
