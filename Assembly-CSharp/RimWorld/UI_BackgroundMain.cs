using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class UI_BackgroundMain : UIMenuBackground
	{
		private static readonly Vector2 BGPlanetSize = new Vector2(2048f, 1280f);

		private static readonly Texture2D BGPlanet = ContentFinder<Texture2D>.Get("UI/HeroArt/BGPlanet", true);

		public override void BackgroundOnGUI()
		{
			bool flag = true;
			float num = (float)UI.screenWidth;
			float num2 = (float)UI.screenHeight;
			Vector2 bGPlanetSize = UI_BackgroundMain.BGPlanetSize;
			float x = bGPlanetSize.x;
			Vector2 bGPlanetSize2 = UI_BackgroundMain.BGPlanetSize;
			if (num > num2 * (x / bGPlanetSize2.y))
			{
				flag = false;
			}
			Rect position;
			if (flag)
			{
				float height = (float)UI.screenHeight;
				float num3 = (float)UI.screenHeight;
				Vector2 bGPlanetSize3 = UI_BackgroundMain.BGPlanetSize;
				float x2 = bGPlanetSize3.x;
				Vector2 bGPlanetSize4 = UI_BackgroundMain.BGPlanetSize;
				float num4 = num3 * (x2 / bGPlanetSize4.y);
				position = new Rect((float)((float)(UI.screenWidth / 2) - num4 / 2.0), 0f, num4, height);
			}
			else
			{
				float width = (float)UI.screenWidth;
				float num5 = (float)UI.screenWidth;
				Vector2 bGPlanetSize5 = UI_BackgroundMain.BGPlanetSize;
				float y = bGPlanetSize5.y;
				Vector2 bGPlanetSize6 = UI_BackgroundMain.BGPlanetSize;
				float num6 = num5 * (y / bGPlanetSize6.x);
				position = new Rect(0f, (float)((float)(UI.screenHeight / 2) - num6 / 2.0), width, num6);
			}
			GUI.DrawTexture(position, UI_BackgroundMain.BGPlanet, ScaleMode.ScaleToFit);
		}
	}
}
