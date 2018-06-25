using System;
using Verse;

namespace RimWorld
{
	public class GenStep_ScenParts : GenStep
	{
		public GenStep_ScenParts()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 1561683158;
			}
		}

		public override void Generate(Map map)
		{
			Find.Scenario.GenerateIntoMap(map);
		}
	}
}
