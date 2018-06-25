using System;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E36 RID: 3638
	public class Dialog_DebugSettingsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x06005619 RID: 22041 RVA: 0x002C64AB File Offset: 0x002C48AB
		public Dialog_DebugSettingsMenu()
		{
			this.forcePause = true;
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x0600561A RID: 22042 RVA: 0x002C64BC File Offset: 0x002C48BC
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600561B RID: 22043 RVA: 0x002C64D4 File Offset: 0x002C48D4
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

		// Token: 0x0600561C RID: 22044 RVA: 0x002C65B8 File Offset: 0x002C49B8
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
