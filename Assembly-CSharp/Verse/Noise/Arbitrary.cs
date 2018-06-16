using System;

namespace Verse.Noise
{
	// Token: 0x02000F86 RID: 3974
	public class Arbitrary : ModuleBase
	{
		// Token: 0x06005FE3 RID: 24547 RVA: 0x0030B0B0 File Offset: 0x003094B0
		public Arbitrary() : base(1)
		{
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x0030B0BA File Offset: 0x003094BA
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x0030B0D4 File Offset: 0x003094D4
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x04003EFC RID: 16124
		private Func<double, double> processor;
	}
}
