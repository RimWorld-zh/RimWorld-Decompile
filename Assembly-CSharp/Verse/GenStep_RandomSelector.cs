using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C59 RID: 3161
	public class GenStep_RandomSelector : GenStep
	{
		// Token: 0x04002F81 RID: 12161
		public List<RandomGenStepSelectorOption> options;

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x0600458C RID: 17804 RVA: 0x0024C474 File Offset: 0x0024A874
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x0024C490 File Offset: 0x0024A890
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
	}
}
