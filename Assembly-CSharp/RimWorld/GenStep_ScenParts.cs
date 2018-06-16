using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F4 RID: 1012
	public class GenStep_ScenParts : GenStep
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001171 RID: 4465 RVA: 0x000970D8 File Offset: 0x000954D8
		public override int SeedPart
		{
			get
			{
				return 1561683158;
			}
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x000970F2 File Offset: 0x000954F2
		public override void Generate(Map map)
		{
			Find.Scenario.GenerateIntoMap(map);
		}
	}
}
