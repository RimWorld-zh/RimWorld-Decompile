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
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo fi = fields[i];
				this.DoField(fi);
			}
			base.listing.Gap(36f);
			Text.Font = GameFont.Small;
			base.listing.Label("View", -1f);
			FieldInfo[] fields2 = typeof(DebugViewSettings).GetFields();
			for (int j = 0; j < fields2.Length; j++)
			{
				FieldInfo fi2 = fields2[j];
				this.DoField(fi2);
			}
		}

		private void DoField(FieldInfo fi)
		{
			if (!fi.IsLiteral)
			{
				string label = GenText.SplitCamelCase(fi.Name).CapitalizeFirst();
				bool flag;
				bool flag2 = flag = (bool)fi.GetValue(null);
				base.CheckboxLabeledDebug(label, ref flag2);
				if (flag2 != flag)
				{
					fi.SetValue(null, flag2);
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
