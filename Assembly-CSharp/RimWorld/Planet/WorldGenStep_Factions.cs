using System;
using Verse;

namespace RimWorld.Planet
{
	public class WorldGenStep_Factions : WorldGenStep
	{
		public WorldGenStep_Factions()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		public override void GenerateFresh(string seed)
		{
			FactionGenerator.GenerateFactionsIntoWorld();
		}

		public override void GenerateWithoutWorldData(string seed)
		{
		}
	}
}
