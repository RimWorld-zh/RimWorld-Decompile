using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C0E RID: 3086
	public sealed class DynamicDrawManager
	{
		// Token: 0x04002E1D RID: 11805
		private Map map;

		// Token: 0x04002E1E RID: 11806
		private HashSet<Thing> drawThings = new HashSet<Thing>();

		// Token: 0x04002E1F RID: 11807
		private bool drawingNow;

		// Token: 0x06004382 RID: 17282 RVA: 0x0023AB81 File Offset: 0x00238F81
		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0023AB9C File Offset: 0x00238F9C
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

		// Token: 0x06004384 RID: 17284 RVA: 0x0023ABEC File Offset: 0x00238FEC
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

		// Token: 0x06004385 RID: 17285 RVA: 0x0023AC3C File Offset: 0x0023903C
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

		// Token: 0x06004386 RID: 17286 RVA: 0x0023AE2C File Offset: 0x0023922C
		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings), false);
		}
	}
}
