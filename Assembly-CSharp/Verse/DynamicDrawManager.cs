using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C0F RID: 3087
	public sealed class DynamicDrawManager
	{
		// Token: 0x04002E24 RID: 11812
		private Map map;

		// Token: 0x04002E25 RID: 11813
		private HashSet<Thing> drawThings = new HashSet<Thing>();

		// Token: 0x04002E26 RID: 11814
		private bool drawingNow;

		// Token: 0x06004382 RID: 17282 RVA: 0x0023AE61 File Offset: 0x00239261
		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0023AE7C File Offset: 0x0023927C
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

		// Token: 0x06004384 RID: 17284 RVA: 0x0023AECC File Offset: 0x002392CC
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

		// Token: 0x06004385 RID: 17285 RVA: 0x0023AF1C File Offset: 0x0023931C
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
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x0023B10C File Offset: 0x0023950C
		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings), false);
		}
	}
}
