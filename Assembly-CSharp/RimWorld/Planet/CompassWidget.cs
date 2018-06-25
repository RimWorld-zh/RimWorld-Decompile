using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008D9 RID: 2265
	[StaticConstructorOnStartup]
	public static class CompassWidget
	{
		// Token: 0x04001BE1 RID: 7137
		private const float Padding = 10f;

		// Token: 0x04001BE2 RID: 7138
		private const float Size = 64f;

		// Token: 0x04001BE3 RID: 7139
		private static readonly Texture2D CompassTex = ContentFinder<Texture2D>.Get("UI/Misc/Compass", true);

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x060033DA RID: 13274 RVA: 0x001BBCC8 File Offset: 0x001BA0C8
		private static float Angle
		{
			get
			{
				Vector2 b = GenWorldUI.WorldToUIPosition(Find.WorldGrid.NorthPolePos);
				Vector2 a = new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
				b.y = (float)UI.screenHeight - b.y;
				return a.AngleTo(b);
			}
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x001BBD28 File Offset: 0x001BA128
		public static void CompassOnGUI(ref float curBaseY)
		{
			Vector2 center = new Vector2((float)UI.screenWidth - 10f - 32f, curBaseY - 10f - 32f);
			CompassWidget.CompassOnGUI(center);
			curBaseY -= 84f;
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x001BBD70 File Offset: 0x001BA170
		private static void CompassOnGUI(Vector2 center)
		{
			Widgets.DrawTextureRotated(center, CompassWidget.CompassTex, CompassWidget.Angle, 1f);
			Rect rect = new Rect(center.x - 32f, center.y - 32f, 64f, 64f);
			TooltipHandler.TipRegion(rect, "CompassTip".Translate());
			if (Widgets.ButtonInvisible(rect, false))
			{
				Find.WorldCameraDriver.RotateSoNorthIsUp(true);
			}
		}
	}
}
