using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C5B RID: 3163
	public class GenStep_RandomSelector : GenStep
	{
		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06004582 RID: 17794 RVA: 0x0024AFF0 File Offset: 0x002493F0
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x0024B00C File Offset: 0x0024940C
		public override void Generate(Map map)
		{
			RandomGenStepSelectorOption randomGenStepSelectorOption = this.options.RandomElementByWeight((RandomGenStepSelectorOption opt) => opt.weight);
			if (randomGenStepSelectorOption.genStep != null)
			{
				randomGenStepSelectorOption.genStep.Generate(map);
			}
			if (randomGenStepSelectorOption.def != null)
			{
				randomGenStepSelectorOption.def.genStep.Generate(map);
			}
		}

		// Token: 0x04002F79 RID: 12153
		public List<RandomGenStepSelectorOption> options;
	}
}
