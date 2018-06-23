using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C57 RID: 3159
	public class GenStep_RandomSelector : GenStep
	{
		// Token: 0x04002F81 RID: 12161
		public List<RandomGenStepSelectorOption> options;

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06004589 RID: 17801 RVA: 0x0024C398 File Offset: 0x0024A798
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x0024C3B4 File Offset: 0x0024A7B4
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
