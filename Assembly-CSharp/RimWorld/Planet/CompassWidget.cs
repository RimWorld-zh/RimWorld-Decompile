using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008D7 RID: 2263
	[StaticConstructorOnStartup]
	public static class CompassWidget
	{
		// Token: 0x04001BDB RID: 7131
		private const float Padding = 10f;

		// Token: 0x04001BDC RID: 7132
		private const float Size = 64f;

		// Token: 0x04001BDD RID: 7133
		private static readonly Texture2D CompassTex = ContentFinder<Texture2D>.Get("UI/Misc/Compass", true);

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x060033D6 RID: 13270 RVA: 0x001BB8B4 File Offset: 0x001B9CB4
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

		// Token: 0x060033D7 RID: 13271 RVA: 0x001BB914 File Offset: 0x001B9D14
		public static void CompassOnGUI(ref float curBaseY)
		{
			Vector2 center = new Vector2((float)UI.screenWidth - 10f - 32f, curBaseY - 10f - 32f);
			CompassWidget.CompassOnGUI(center);
			curBaseY -= 84f;
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x001BB95C File Offset: 0x001B9D5C
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
