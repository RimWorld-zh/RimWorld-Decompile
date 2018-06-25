using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E8F RID: 3727
	public class ThingOverlays
	{
		// Token: 0x06005804 RID: 22532 RVA: 0x002D266C File Offset: 0x002D0A6C
		public void ThingOverlaysOnGUI()
		{
			if (Event.current.type == EventType.Repaint)
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.HasGUIOverlay);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (currentViewRect.Contains(thing.Position))
					{
						if (!Find.CurrentMap.fogGrid.IsFogged(thing.Position))
						{
							try
							{
								thing.DrawGUIOverlay();
							}
							catch (Exception ex)
							{
								Log.Error(string.Concat(new object[]
								{
									"Exception drawing ThingOverlay for ",
									thing,
									": ",
									ex
								}), false);
							}
						}
					}
				}
			}
		}
	}
}
