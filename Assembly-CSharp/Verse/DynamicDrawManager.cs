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
			if (t.def.drawerType != 0)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + t + " while drawing is in progress. Things shouldn't be spawned in Draw methods.");
				}
				this.drawThings.Add(t);
			}
		}

		public void DeRegisterDrawable(Thing t)
		{
			if (t.def.drawerType != 0)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + t + " while drawing is in progress. Things shouldn't be despawned in Draw methods.");
				}
				this.drawThings.Remove(t);
			}
		}

		public void DrawDynamicThings()
		{
			if (DebugViewSettings.drawThingsDynamic)
			{
				this.drawingNow = true;
				try
				{
					bool[] fogGrid = this.map.fogGrid.fogGrid;
					CellRect cellRect = Find.CameraDriver.CurrentViewRect;
					cellRect.ClipInsideMap(this.map);
					cellRect = cellRect.ExpandedBy(1);
					CellIndices cellIndices = this.map.cellIndices;
					foreach (Thing drawThing in this.drawThings)
					{
						IntVec3 position = drawThing.Position;
						if ((cellRect.Contains(position) || drawThing.def.drawOffscreen) && (!fogGrid[cellIndices.CellToIndex(position)] || drawThing.def.seeThroughFog) && (!(drawThing.def.hideAtSnowDepth < 1.0) || !(this.map.snowGrid.GetDepth(drawThing.Position) > drawThing.def.hideAtSnowDepth)))
						{
							try
							{
								drawThing.Draw();
							}
							catch (Exception ex)
							{
								Log.Error("Exception drawing " + drawThing + ": " + ex.ToString());
							}
						}
					}
				}
				catch (Exception arg)
				{
					Log.Error("Exception drawing dynamic things: " + arg);
				}
				this.drawingNow = false;
			}
		}

		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings));
		}
	}
}
