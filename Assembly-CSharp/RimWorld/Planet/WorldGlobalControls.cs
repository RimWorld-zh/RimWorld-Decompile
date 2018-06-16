using System;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008EC RID: 2284
	public class WorldGlobalControls
	{
		// Token: 0x06003484 RID: 13444 RVA: 0x001C105C File Offset: 0x001BF45C
		public void WorldGlobalControlsOnGUI()
		{
			if (Event.current.type != EventType.Layout)
			{
				float num = (float)UI.screenWidth - 200f;
				float num2 = (float)UI.screenHeight - 4f;
				if (Current.ProgramState == ProgramState.Playing)
				{
					num2 -= 35f;
				}
				Profiler.BeginSample("World play settings");
				GlobalControlsUtility.DoPlaySettings(this.rowVisibility, true, ref num2);
				Profiler.EndSample();
				if (Current.ProgramState == ProgramState.Playing)
				{
					num2 -= 4f;
					Profiler.BeginSample("Timespeed controls");
					GlobalControlsUtility.DoTimespeedControls(num, 200f, ref num2);
					Profiler.EndSample();
					if (Find.CurrentMap != null || Find.WorldSelector.AnyObjectOrTileSelected)
					{
						num2 -= 4f;
						Profiler.BeginSample("Date");
						GlobalControlsUtility.DoDate(num, 200f, ref num2);
						Profiler.EndSample();
					}
					Profiler.BeginSample("World conditions");
					float num3 = 230f;
					float num4 = Find.World.gameConditionManager.TotalHeightAt(num3 - 15f);
					Rect rect = new Rect(num - 30f, num2 - num4, num3, num4);
					Find.World.gameConditionManager.DoConditionsUI(rect);
					num2 -= rect.height;
					Profiler.EndSample();
				}
				if (Prefs.ShowRealtimeClock)
				{
					Profiler.BeginSample("RealtimeClock");
					GlobalControlsUtility.DoRealtimeClock(num, 200f, ref num2);
					Profiler.EndSample();
				}
				Profiler.BeginSample("Distance measure");
				Find.WorldRoutePlanner.DoRoutePlannerButton(ref num2);
				Profiler.EndSample();
				if (!Find.PlaySettings.lockNorthUp)
				{
					Profiler.BeginSample("Compass");
					CompassWidget.CompassOnGUI(ref num2);
					Profiler.EndSample();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Profiler.BeginSample("Letters");
					num2 -= 10f;
					Find.LetterStack.LettersOnGUI(num2);
					Profiler.EndSample();
				}
			}
		}

		// Token: 0x04001C69 RID: 7273
		public const float Width = 200f;

		// Token: 0x04001C6A RID: 7274
		private const int VisibilityControlsPerRow = 5;

		// Token: 0x04001C6B RID: 7275
		private WidgetRow rowVisibility = new WidgetRow();
	}
}
