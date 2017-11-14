using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldGlobalControls
	{
		public const float Width = 200f;

		private const int VisibilityControlsPerRow = 5;

		private WidgetRow rowVisibility = new WidgetRow();

		public void WorldGlobalControlsOnGUI()
		{
			if (Event.current.type != EventType.Layout)
			{
				float num = (float)((float)UI.screenWidth - 200.0);
				float num2 = (float)((float)UI.screenHeight - 4.0);
				if (Current.ProgramState == ProgramState.Playing)
				{
					num2 = (float)(num2 - 35.0);
				}
				GlobalControlsUtility.DoPlaySettings(this.rowVisibility, true, ref num2);
				if (Current.ProgramState == ProgramState.Playing)
				{
					num2 = (float)(num2 - 4.0);
					GlobalControlsUtility.DoTimespeedControls(num, 200f, ref num2);
					if (Find.VisibleMap != null || Find.WorldSelector.AnyObjectOrTileSelected)
					{
						num2 = (float)(num2 - 4.0);
						GlobalControlsUtility.DoDate(num, 200f, ref num2);
					}
					float num3 = 230f;
					float num4 = Find.World.gameConditionManager.TotalHeightAt((float)(num3 - 15.0));
					Rect rect = new Rect((float)(num - 30.0), num2 - num4, num3, num4);
					Find.World.gameConditionManager.DoConditionsUI(rect);
					num2 -= rect.height;
				}
				if (Prefs.ShowRealtimeClock)
				{
					GlobalControlsUtility.DoRealtimeClock(num, 200f, ref num2);
				}
				Find.WorldRoutePlanner.DoRoutePlannerButton(ref num2);
				if (!Find.PlaySettings.lockNorthUp)
				{
					CompassWidget.CompassOnGUI(ref num2);
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					num2 = (float)(num2 - 10.0);
					Find.LetterStack.LettersOnGUI(num2);
				}
			}
		}
	}
}
