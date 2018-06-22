using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F4 RID: 1012
	public class GenStep_ScenParts : GenStep
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001171 RID: 4465 RVA: 0x000972BC File Offset: 0x000956BC
		public override int SeedPart
		{
			get
			{
				return 1561683158;
			}
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x000972D6 File Offset: 0x000956D6
		public override void Generate(Map map)
		{
			Find.Scenario.GenerateIntoMap(map);
		}
	}
}
