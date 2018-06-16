using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000806 RID: 2054
	public class Dialog_ModSettings : Window
	{
		// Token: 0x06002DD4 RID: 11732 RVA: 0x00181E9A File Offset: 0x0018029A
		public Dialog_ModSettings()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002DD5 RID: 11733 RVA: 0x00181ED0 File Offset: 0x001802D0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x00181EF4 File Offset: 0x001802F4
		public override void PreClose()
		{
			base.PreClose();
			if (this.selMod != null)
			{
				this.selMod.WriteSettings();
			}
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x00181F14 File Offset: 0x00180314
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

		// Token: 0x06002DD8 RID: 11736 RVA: 0x00182118 File Offset: 0x00180518
		public static bool HasSettings()
		{
			return LoadedModManager.ModHandles.Any((Mod mod) => !mod.SettingsCategory().NullOrEmpty());
		}

		// Token: 0x04001851 RID: 6225
		private Mod selMod = null;

		// Token: 0x04001852 RID: 6226
		private const float TopAreaHeight = 40f;

		// Token: 0x04001853 RID: 6227
		private const float TopButtonHeight = 35f;

		// Token: 0x04001854 RID: 6228
		private const float TopButtonWidth = 150f;
	}
}
