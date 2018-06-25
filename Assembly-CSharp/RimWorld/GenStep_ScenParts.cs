using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F6 RID: 1014
	public class GenStep_ScenParts : GenStep
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x0009740C File Offset: 0x0009580C
		public override int SeedPart
		{
			get
			{
				return 1561683158;
			}
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00097426 File Offset: 0x00095826
		public override void Generate(Map map)
		{
			Find.Scenario.GenerateIntoMap(map);
		}
	}
}
