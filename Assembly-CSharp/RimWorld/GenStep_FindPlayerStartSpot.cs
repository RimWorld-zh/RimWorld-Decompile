using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003FA RID: 1018
	public class GenStep_FindPlayerStartSpot : GenStep
	{
		// Token: 0x04000AA0 RID: 2720
		private const int MinRoomCellCount = 10;

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x00097A70 File Offset: 0x00095E70
		public override int SeedPart
		{
			get
			{
				return 1187186631;
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00097A8C File Offset: 0x00095E8C
		public override void Generate(Map map)
		{
			DeepProfiler.Start("RebuildAllRegions");
			map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			DeepProfiler.End();
			MapGenerator.PlayerStartSpot = CellFinderLoose.TryFindCentralCell(map, 7, 10, (IntVec3 x) => !x.Roofed(map));
		}
	}
}
