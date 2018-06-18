using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020007BB RID: 1979
	public static class CopyPasteUI
	{
		// Token: 0x06002BDC RID: 11228 RVA: 0x00173090 File Offset: 0x00171490
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

		// Token: 0x04001792 RID: 6034
		public const float CopyPasteIconHeight = 24f;

		// Token: 0x04001793 RID: 6035
		public const float CopyPasteIconWidth = 18f;

		// Token: 0x04001794 RID: 6036
		public const float CopyPasteColumnWidth = 36f;
	}
}
