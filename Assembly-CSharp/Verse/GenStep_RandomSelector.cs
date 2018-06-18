using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C5A RID: 3162
	public class GenStep_RandomSelector : GenStep
	{
		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06004580 RID: 17792 RVA: 0x0024AFC8 File Offset: 0x002493C8
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x0024AFE4 File Offset: 0x002493E4
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

		// Token: 0x04002F77 RID: 12151
		public List<RandomGenStepSelectorOption> options;
	}
}
