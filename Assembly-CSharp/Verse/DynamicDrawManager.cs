using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C0F RID: 3087
	public sealed class DynamicDrawManager
	{
		// Token: 0x06004376 RID: 17270 RVA: 0x002396DD File Offset: 0x00237ADD
		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x002396F8 File Offset: 0x00237AF8
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

		// Token: 0x06004378 RID: 17272 RVA: 0x00239748 File Offset: 0x00237B48
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

		// Token: 0x06004379 RID: 17273 RVA: 0x00239798 File Offset: 0x00237B98
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

		// Token: 0x0600437A RID: 17274 RVA: 0x00239988 File Offset: 0x00237D88
		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings), false);
		}

		// Token: 0x04002E13 RID: 11795
		private Map map;

		// Token: 0x04002E14 RID: 11796
		private HashSet<Thing> drawThings = new HashSet<Thing>();

		// Token: 0x04002E15 RID: 11797
		private bool drawingNow;
	}
}
