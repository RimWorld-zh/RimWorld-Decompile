using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020007B7 RID: 1975
	public static class CopyPasteUI
	{
		// Token: 0x04001790 RID: 6032
		public const float CopyPasteIconHeight = 24f;

		// Token: 0x04001791 RID: 6033
		public const float CopyPasteIconWidth = 18f;

		// Token: 0x04001792 RID: 6034
		public const float CopyPasteColumnWidth = 36f;

		// Token: 0x06002BD5 RID: 11221 RVA: 0x00173268 File Offset: 0x00171668
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
