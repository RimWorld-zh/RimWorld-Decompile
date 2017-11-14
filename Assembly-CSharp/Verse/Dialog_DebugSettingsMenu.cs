using RimWorld;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	public class Dialog_DebugSettingsMenu : Dialog_DebugOptionLister
	{
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		public Dialog_DebugSettingsMenu()
		{
			base.forcePause = true;
		}

		protected override void DoListingItems()
		{
			if (KeyBindingDefOf.ToggleDebugSettingsMenu.KeyDownEvent)
			{
				Event.current.Use();
				this.Close(true);
			}
			Text.Font = GameFont.Small;
			base.listing.Label("Gameplay", -1f);
			FieldInfo[] fields = typeof(DebugSettings).GetFields();
			foreach (FieldInfo fi in fields)
			{
				this.DoField(fi);
			}
			base.listing.Gap(36f);
			Text.Font = GameFont.Small;
			base.listing.Label("View", -1f);
			FieldInfo[] fields2 = typeof(DebugViewSettings).GetFields();
			foreach (FieldInfo fi2 in fields2)
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
