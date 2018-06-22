using System;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E33 RID: 3635
	public class Dialog_DebugSettingsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x06005615 RID: 22037 RVA: 0x002C6193 File Offset: 0x002C4593
		public Dialog_DebugSettingsMenu()
		{
			this.forcePause = true;
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06005616 RID: 22038 RVA: 0x002C61A4 File Offset: 0x002C45A4
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005617 RID: 22039 RVA: 0x002C61BC File Offset: 0x002C45BC
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

		// Token: 0x06005618 RID: 22040 RVA: 0x002C62A0 File Offset: 0x002C46A0
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
