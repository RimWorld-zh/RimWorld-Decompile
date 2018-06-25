using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020007B9 RID: 1977
	public static class CopyPasteUI
	{
		// Token: 0x04001794 RID: 6036
		public const float CopyPasteIconHeight = 24f;

		// Token: 0x04001795 RID: 6037
		public const float CopyPasteIconWidth = 18f;

		// Token: 0x04001796 RID: 6038
		public const float CopyPasteColumnWidth = 36f;

		// Token: 0x06002BD8 RID: 11224 RVA: 0x0017361C File Offset: 0x00171A1C
		public static void DoCopyPasteButtons(Rect rect, Action copyAction, Action pasteAction)
		{
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height / 2f - 12f), 18f, 24f);
			if (Widgets.ButtonImage(rect2, TexButton.Copy))
			{
				copyAction();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegion(rect2, "Copy".Translate());
			if (pasteAction != null)
			{
				Rect rect3 = rect2;
				rect3.x = rect2.xMax;
				if (Widgets.ButtonImage(rect3, TexButton.Paste))
				{
					pasteAction();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegion(rect3, "Paste".Translate());
			}
		}
	}
}
