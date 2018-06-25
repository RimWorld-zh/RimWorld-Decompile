using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Dialog_ModSettings : Window
	{
		private Mod selMod = null;

		private const float TopAreaHeight = 40f;

		private const float TopButtonHeight = 35f;

		private const float TopButtonWidth = 150f;

		[CompilerGenerated]
		private static Func<Mod, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Mod, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Mod, bool> <>f__am$cache2;

		public Dialog_ModSettings()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		public override void PreClose()
		{
			base.PreClose();
			if (this.selMod != null)
			{
				this.selMod.WriteSettings();
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, 150f, 35f);
			if (Widgets.ButtonText(rect, "SelectMod".Translate(), true, false, true))
			{
				if (Dialog_ModSettings.HasSettings())
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (Mod mod2 in from mod in LoadedModManager.ModHandles
					where !mod.SettingsCategory().NullOrEmpty()
					orderby mod.SettingsCategory()
					select mod)
					{
						Mod localMod = mod2;
						list.Add(new FloatMenuOption(mod2.SettingsCategory(), delegate()
						{
							if (this.selMod != null)
							{
								this.selMod.WriteSettings();
							}
							this.selMod = localMod;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				else
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					list2.Add(new FloatMenuOption("NoConfigurableMods".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			if (this.selMod != null)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(167f, 0f, inRect.width - 150f - 17f, 35f), this.selMod.SettingsCategory());
				Text.Font = GameFont.Small;
				Rect inRect2 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - this.CloseButSize.y);
				this.selMod.DoSettingsWindowContents(inRect2);
			}
		}

		public static bool HasSettings()
		{
			return LoadedModManager.ModHandles.Any((Mod mod) => !mod.SettingsCategory().NullOrEmpty());
		}

		[CompilerGenerated]
		private static bool <DoWindowContents>m__0(Mod mod)
		{
			return !mod.SettingsCategory().NullOrEmpty();
		}

		[CompilerGenerated]
		private static string <DoWindowContents>m__1(Mod mod)
		{
			return mod.SettingsCategory();
		}

		[CompilerGenerated]
		private static bool <HasSettings>m__2(Mod mod)
		{
			return !mod.SettingsCategory().NullOrEmpty();
		}

		[CompilerGenerated]
		private sealed class <DoWindowContents>c__AnonStorey0
		{
			internal Mod localMod;

			internal Dialog_ModSettings $this;

			public <DoWindowContents>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				if (this.$this.selMod != null)
				{
					this.$this.selMod.WriteSettings();
				}
				this.$this.selMod = this.localMod;
			}
		}
	}
}
