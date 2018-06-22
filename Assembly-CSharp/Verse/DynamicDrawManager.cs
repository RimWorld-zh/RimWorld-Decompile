using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C0C RID: 3084
	public sealed class DynamicDrawManager
	{
		// Token: 0x0600437F RID: 17279 RVA: 0x0023AAA5 File Offset: 0x00238EA5
		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004380 RID: 17280 RVA: 0x0023AAC0 File Offset: 0x00238EC0
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

		// Token: 0x06004381 RID: 17281 RVA: 0x0023AB10 File Offset: 0x00238F10
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

		// Token: 0x06004382 RID: 17282 RVA: 0x0023AB60 File Offset: 0x00238F60
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

		// Token: 0x06004383 RID: 17283 RVA: 0x0023AD50 File Offset: 0x00239150
		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings), false);
		}

		// Token: 0x04002E1D RID: 11805
		private Map map;

		// Token: 0x04002E1E RID: 11806
		private HashSet<Thing> drawThings = new HashSet<Thing>();

		// Token: 0x04002E1F RID: 11807
		private bool drawingNow;
	}
}
