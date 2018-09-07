using System;
using System.Collections.Generic;

namespace Verse
{
	public sealed class DynamicDrawManager
	{
		private Map map;

		private HashSet<Thing> drawThings = new HashSet<Thing>();

		private bool drawingNow;

		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		public void RegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + t + " while drawing is in progress. Things shouldn't be spawned in Draw methods.", false);
				}
				this.drawThings.Add(t);
			}
		}

		public void DeRegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + t + " while drawing is in progress. Things shouldn't be despawned in Draw methods.", false);
				}
				this.drawThings.Remove(t);
			}
		}

		public void DrawDynamicThings()
		{
			if (!DebugViewSettings.drawThingsDynamic)
			{
				return;
			}
			this.drawingNow = true;
			try
			{
				bool[] fogGrid = this.map.fogGrid.fogGrid;
				CellRect cellRect = Find.CameraDriver.CurrentViewRect;
				cellRect.ClipInsideMap(this.map);
				cellRect = cellRect.ExpandedBy(1);
				CellIndices cellIndices = this.map.cellIndices;
				foreach (Thing thing in this.drawThings)
				{
					IntVec3 position = thing.Position;
					if (cellRect.Contains(position) || thing.def.drawOffscreen)
					{
						if (!fogGrid[cellIndices.CellToIndex(position)] || thing.def.seeThroughFog)
						{
							if (thing.def.hideAtSnowDepth >= 1f || this.map.snowGrid.GetDepth(thing.Position) <= thing.def.hideAtSnowDepth)
							{
								try
								{
									thing.Draw();
								}
								catch (Exception ex)
								{
									Log.Error(string.Concat(new object[]
									{
										"Exception drawing ",
										thing,
										": ",
										ex.ToString()
									}), false);
								}
							}
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception drawing dynamic things: " + arg, false);
			}
			this.drawingNow = false;
		}

		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings), false);
		}
	}
}
