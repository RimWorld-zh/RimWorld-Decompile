using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000804 RID: 2052
	public class Dialog_ModSettings : Window
	{
		// Token: 0x04001853 RID: 6227
		private Mod selMod = null;

		// Token: 0x04001854 RID: 6228
		private const float TopAreaHeight = 40f;

		// Token: 0x04001855 RID: 6229
		private const float TopButtonHeight = 35f;

		// Token: 0x04001856 RID: 6230
		private const float TopButtonWidth = 150f;

		// Token: 0x06002DD2 RID: 11730 RVA: 0x001824BA File Offset: 0x001808BA
		public Dialog_ModSettings()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002DD3 RID: 11731 RVA: 0x001824F0 File Offset: 0x001808F0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x00182514 File Offset: 0x00180914
		public override void PreClose()
		{
			base.PreClose();
			if (this.selMod != null)
			{
				this.selMod.WriteSettings();
			}
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x00182534 File Offset: 0x00180934
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

		// Token: 0x06002DD6 RID: 11734 RVA: 0x00182738 File Offset: 0x00180B38
		public static bool HasSettings()
		{
			return LoadedModManager.ModHandles.Any((Mod mod) => !mod.SettingsCategory().NullOrEmpty());
		}
	}
}
