using System;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class Dialog_DebugSettingsMenu : Dialog_DebugOptionLister
	{
		public Dialog_DebugSettingsMenu()
		{
			this.forcePause = true;
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
