using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C93 RID: 3219
	public class RegionMaker
	{
		// Token: 0x06004693 RID: 18067 RVA: 0x00252E88 File Offset: 0x00251288
		public RegionMaker(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x00252EDC File Offset: 0x002512DC
		public Region TryGenerateRegionFrom(IntVec3 root)
		{
			RegionType expectedRegionType = root.GetExpectedRegionType(this.map);
			Region result;
			if (expectedRegionType == RegionType.None)
			{
				result = null;
			}
			else if (this.working)
			{
				Log.Error("Trying to generate a new region but we are currently generating one. Nested calls are not allowed.", false);
				result = null;
			}
			else
			{
				this.working = true;
				try
				{
					this.regionGrid = this.map.regionGrid;
					this.newReg = Region.MakeNewUnfilled(root, this.map);
					this.newReg.type = expectedRegionType;
					if (this.newReg.type == RegionType.Portal)
					{
						this.newReg.portal = root.GetDoor(this.map);
					}
					this.FloodFillAndAddCells(root);
					this.CreateLinks();
					this.RegisterThingsInRegionListers();
					result = this.newReg;
				}
				finally
				{
					this.working = false;
				}
			}
			return result;
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x00252FBC File Offset: 0x002513BC
		private void FloodFillAndAddCells(IntVec3 root)
		{
			this.newRegCells.Clear();
			if (this.newReg.type.IsOneCellRegion())
			{
				this.AddCell(root);
			}
			else
			{
				this.map.floodFiller.FloodFill(root, (IntVec3 x) => this.newReg.extentsLimit.Contains(x) && x.GetExpectedRegionType(this.map) == this.newReg.type, delegate(IntVec3 x)
				{
					this.AddCell(x);
				}, int.MaxValue, false, null);
			}
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x0025302C File Offset: 0x0025142C
		private void AddCell(IntVec3 c)
		{
			this.regionGrid.SetRegionAt(c, this.newReg);
			this.newRegCells.Add(c);
			if (this.newReg.extentsClose.minX > c.x)
			{
				this.newReg.extentsClose.minX = c.x;
			}
			if (this.newReg.extentsClose.maxX < c.x)
			{
				this.newReg.extentsClose.maxX = c.x;
			}
			if (this.newReg.extentsClose.minZ > c.z)
			{
				this.newReg.extentsClose.minZ = c.z;
			}
			if (this.newReg.extentsClose.maxZ < c.z)
			{
				this.newReg.extentsClose.maxZ = c.z;
			}
			if (c.x == 0 || c.x == this.map.Size.x - 1 || c.z == 0 || c.z == this.map.Size.z - 1)
			{
				this.newReg.touchesMapEdge = true;
			}
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x0025318C File Offset: 0x0025158C
		private void CreateLinks()
		{
			for (int i = 0; i < this.linksProcessedAt.Length; i++)
			{
				this.linksProcessedAt[i].Clear();
			}
			for (int j = 0; j < this.newRegCells.Count; j++)
			{
				IntVec3 c = this.newRegCells[j];
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.North, c);
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.South, c);
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.East, c);
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.West, c);
			}
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x00253220 File Offset: 0x00251620
		private void SweepInTwoDirectionsAndTryToCreateLink(Rot4 potentialOtherRegionDir, IntVec3 c)
		{
			if (potentialOtherRegionDir.IsValid)
			{
				HashSet<IntVec3> hashSet = this.linksProcessedAt[potentialOtherRegionDir.AsInt];
				if (!hashSet.Contains(c))
				{
					IntVec3 c2 = c + potentialOtherRegionDir.FacingCell;
					if (!c2.InBounds(this.map) || this.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c2) != this.newReg)
					{
						RegionType expectedRegionType = c2.GetExpectedRegionType(this.map);
						if (expectedRegionType != RegionType.None)
						{
							Rot4 rot = potentialOtherRegionDir;
							rot.Rotate(RotationDirection.Clockwise);
							int num = 0;
							int num2 = 0;
							hashSet.Add(c);
							if (!expectedRegionType.IsOneCellRegion())
							{
								for (;;)
								{
									IntVec3 intVec = c + rot.FacingCell * (num + 1);
									if (!intVec.InBounds(this.map) || this.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(intVec) != this.newReg || (intVec + potentialOtherRegionDir.FacingCell).GetExpectedRegionType(this.map) != expectedRegionType)
									{
										break;
									}
									if (!hashSet.Add(intVec))
									{
										Log.Error("We've processed the same cell twice.", false);
									}
									num++;
								}
								for (;;)
								{
									IntVec3 intVec2 = c - rot.FacingCell * (num2 + 1);
									if (!intVec2.InBounds(this.map) || this.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(intVec2) != this.newReg || (intVec2 + potentialOtherRegionDir.FacingCell).GetExpectedRegionType(this.map) != expectedRegionType)
									{
										break;
									}
									if (!hashSet.Add(intVec2))
									{
										Log.Error("We've processed the same cell twice.", false);
									}
									num2++;
								}
							}
							int length = num + num2 + 1;
							SpanDirection dir;
							IntVec3 root;
							if (potentialOtherRegionDir == Rot4.North)
							{
								dir = SpanDirection.East;
								root = c - rot.FacingCell * num2;
								root.z++;
							}
							else if (potentialOtherRegionDir == Rot4.South)
							{
								dir = SpanDirection.East;
								root = c + rot.FacingCell * num;
							}
							else if (potentialOtherRegionDir == Rot4.East)
							{
								dir = SpanDirection.North;
								root = c + rot.FacingCell * num;
								root.x++;
							}
							else
							{
								dir = SpanDirection.North;
								root = c - rot.FacingCell * num2;
							}
							EdgeSpan span = new EdgeSpan(root, dir, length);
							RegionLink regionLink = this.map.regionLinkDatabase.LinkFrom(span);
							regionLink.Register(this.newReg);
							this.newReg.links.Add(regionLink);
						}
					}
				}
			}
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x00253504 File Offset: 0x00251904
		private void RegisterThingsInRegionListers()
		{
			CellRect cellRect = this.newReg.extentsClose;
			cellRect = cellRect.ExpandedBy(1);
			cellRect.ClipInsideMap(this.map);
			RegionMaker.tmpProcessedThings.Clear();
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			while (!iterator.Done())
			{
				IntVec3 intVec = iterator.Current;
				bool flag = false;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = intVec + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(this.map))
					{
						if (this.regionGrid.GetValidRegionAt(c) == this.newReg)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					RegionListersUpdater.RegisterAllAt(intVec, this.map, RegionMaker.tmpProcessedThings);
				}
				iterator.MoveNext();
			}
			RegionMaker.tmpProcessedThings.Clear();
		}

		// Token: 0x04003009 RID: 12297
		private Map map;

		// Token: 0x0400300A RID: 12298
		private Region newReg;

		// Token: 0x0400300B RID: 12299
		private List<IntVec3> newRegCells = new List<IntVec3>();

		// Token: 0x0400300C RID: 12300
		private bool working;

		// Token: 0x0400300D RID: 12301
		private HashSet<IntVec3>[] linksProcessedAt = new HashSet<IntVec3>[]
		{
			new HashSet<IntVec3>(),
			new HashSet<IntVec3>(),
			new HashSet<IntVec3>(),
			new HashSet<IntVec3>()
		};

		// Token: 0x0400300E RID: 12302
		private RegionGrid regionGrid;

		// Token: 0x0400300F RID: 12303
		private static HashSet<Thing> tmpProcessedThings = new HashSet<Thing>();
	}
}
