using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000E35 RID: 3637
	public class Dialog_DebugOutputMenu : Dialog_DebugOptionLister
	{
		// Token: 0x060055F6 RID: 22006 RVA: 0x002C4328 File Offset: 0x002C2728
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
		// (get) Token: 0x060055F7 RID: 22007 RVA: 0x002C44B0 File Offset: 0x002C28B0
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060055F8 RID: 22008 RVA: 0x002C44C8 File Offset: 0x002C28C8
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

		// Token: 0x040038CC RID: 14540
		private List<Dialog_DebugOutputMenu.DebugOutputOption> debugOutputs = new List<Dialog_DebugOutputMenu.DebugOutputOption>();

		// Token: 0x040038CD RID: 14541
		private const string DefaultCategory = "General";

		// Token: 0x02000E36 RID: 3638
		private struct DebugOutputOption
		{
			// Token: 0x040038D0 RID: 14544
			public string label;

			// Token: 0x040038D1 RID: 14545
			public string category;

			// Token: 0x040038D2 RID: 14546
			public Action action;
		}
	}
}
