using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	public class GenStep_RandomSelector : GenStep
	{
		public List<RandomGenStepSelectorOption> options;

		[CompilerGenerated]
		private static Func<RandomGenStepSelectorOption, float> <>f__am$cache0;

		public GenStep_RandomSelector()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

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

		[CompilerGenerated]
		private static float <Generate>m__0(RandomGenStepSelectorOption opt)
		{
			return opt.weight;
		}
	}
}
