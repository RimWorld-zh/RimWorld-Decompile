using System;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E35 RID: 3637
	public class Dialog_DebugSettingsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x06005619 RID: 22041 RVA: 0x002C62BF File Offset: 0x002C46BF
		public Dialog_DebugSettingsMenu()
		{
			this.forcePause = true;
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x0600561A RID: 22042 RVA: 0x002C62D0 File Offset: 0x002C46D0
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600561B RID: 22043 RVA: 0x002C62E8 File Offset: 0x002C46E8
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

		// Token: 0x0600561C RID: 22044 RVA: 0x002C63CC File Offset: 0x002C47CC
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
