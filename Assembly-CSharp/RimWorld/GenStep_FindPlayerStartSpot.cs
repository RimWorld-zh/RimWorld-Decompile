using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F8 RID: 1016
	public class GenStep_FindPlayerStartSpot : GenStep
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x00097920 File Offset: 0x00095D20
		public override int SeedPart
		{
			get
			{
				return 1187186631;
			}
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0009793C File Offset: 0x00095D3C
		public override void Generate(Map map)
		{
			DeepProfiler.Start("RebuildAllRegions");
			map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			DeepProfiler.End();
			MapGenerator.PlayerStartSpot = CellFinderLoose.TryFindCentralCell(map, 7, 10, (IntVec3 x) => !x.Roofed(map));
		}

		// Token: 0x04000AA0 RID: 2720
		private const int MinRoomCellCount = 10;
	}
}
