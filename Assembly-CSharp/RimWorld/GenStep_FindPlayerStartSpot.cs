using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class GenStep_FindPlayerStartSpot : GenStep
	{
		private const int MinRoomCellCount = 10;

		public GenStep_FindPlayerStartSpot()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 1187186631;
			}
		}

		public override void Generate(Map map, GenStepParams parms)
		{
			DeepProfiler.Start("RebuildAllRegions");
			map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			DeepProfiler.End();
			MapGenerator.PlayerStartSpot = CellFinderLoose.TryFindCentralCell(map, 7, 10, (IntVec3 x) => !x.Roofed(map));
		}

		[CompilerGenerated]
		private sealed class <Generate>c__AnonStorey0
		{
			internal Map map;

			public <Generate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return !x.Roofed(this.map);
			}
		}
	}
}
