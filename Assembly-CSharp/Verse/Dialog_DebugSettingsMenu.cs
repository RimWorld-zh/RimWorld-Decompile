using System;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E36 RID: 3638
	public class Dialog_DebugSettingsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x060055F9 RID: 22009 RVA: 0x002C45D7 File Offset: 0x002C29D7
		public Dialog_DebugSettingsMenu()
		{
			this.forcePause = true;
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x060055FA RID: 22010 RVA: 0x002C45E8 File Offset: 0x002C29E8
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060055FB RID: 22011 RVA: 0x002C4600 File Offset: 0x002C2A00
		protected override void DoListingItems()
		{
			if (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent)
			{
				Event.current.Use();
				this.Close(true);
			}
			Text.Font = GameFont.Small;
			this.listing.Label("Gameplay", -1f, null);
			foreach (FieldInfo fi in typeof(DebugSettings).GetFields())
			{
				this.DoField(fi);
			}
			this.listing.Gap(36f);
			Text.Font = GameFont.Small;
			this.listing.Label("View", -1f, null);
			foreach (FieldInfo fi2 in typeof(DebugViewSettings).GetFields())
			{
				this.DoField(fi2);
			}
		}

		// Token: 0x060055FC RID: 22012 RVA: 0x002C46E4 File Offset: 0x002C2AE4
		private void DoField(FieldInfo fi)
		{
			if (!fi.IsLiteral)
			{
				string label = GenText.SplitCamelCase(fi.Name).CapitalizeFirst();
				bool flag = (bool)fi.GetValue(null);
				bool flag2 = flag;
				base.CheckboxLabeledDebug(label, ref flag);
				if (flag != flag2)
				{
					fi.SetValue(null, flag);
					MethodInfo method = fi.DeclaringType.GetMethod(fi.Name + "Toggled", BindingFlags.Static | BindingFlags.Public);
					if (method != null)
					{
						method.Invoke(null, null);
					}
				}
			}
		}
	}
}
