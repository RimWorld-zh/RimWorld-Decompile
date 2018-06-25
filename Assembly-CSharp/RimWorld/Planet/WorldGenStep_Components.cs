using System;
using Verse;

namespace RimWorld.Planet
{
	public class WorldGenStep_Components : WorldGenStep
	{
		public WorldGenStep_Components()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		public override void GenerateFresh(string seed)
		{
			Find.World.ConstructComponents();
		}

		public override void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFromScribe(seed);
		}

		public override void GenerateFromScribe(string seed)
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
