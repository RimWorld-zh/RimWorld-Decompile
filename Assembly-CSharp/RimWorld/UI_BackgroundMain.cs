using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200083C RID: 2108
	[StaticConstructorOnStartup]
	public class UI_BackgroundMain : UIMenuBackground
	{
		// Token: 0x040019CC RID: 6604
		private static readonly Vector2 BGPlanetSize = new Vector2(2048f, 1280f);

		// Token: 0x040019CD RID: 6605
		private static readonly Texture2D BGPlanet = ContentFinder<Texture2D>.Get("UI/HeroArt/BGPlanet", true);

		// Token: 0x06002FB6 RID: 12214 RVA: 0x00199898 File Offset: 0x00197C98
		public override void BackgroundOnGUI()
		{
			bool flag = true;
			if ((float)UI.screenWidth > (float)UI.screenHeight * (UI_BackgroundMain.BGPlanetSize.x / UI_BackgroundMain.BGPlanetSize.y))
			{
				flag = false;
			}
			Rect position;
			if (flag)
			{
				float height = (float)UI.screenHeight;
				float num = (float)UI.screenHeight * (UI_BackgroundMain.BGPlanetSize.x / UI_BackgroundMain.BGPlanetSize.y);
				position = new Rect((float)(UI.screenWidth / 2) - num / 2f, 0f, num, height);
			}
			else
			{
				float width = (float)UI.screenWidth;
				float num2 = (float)UI.screenWidth * (UI_BackgroundMain.BGPlanetSize.y / UI_BackgroundMain.BGPlanetSize.x);
				position = new Rect(0f, (float)(UI.screenHeight / 2) - num2 / 2f, width, num2);
			}
			GUI.DrawTexture(position, UI_BackgroundMain.BGPlanet, ScaleMode.ScaleToFit);
		}
	}
}
