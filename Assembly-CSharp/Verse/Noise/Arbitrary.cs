using System;

namespace Verse.Noise
{
	// Token: 0x02000F85 RID: 3973
	public class Arbitrary : ModuleBase
	{
		// Token: 0x06005FE1 RID: 24545 RVA: 0x0030B18C File Offset: 0x0030958C
		public Arbitrary() : base(1)
		{
		}

		// Token: 0x06005FE2 RID: 24546 RVA: 0x0030B196 File Offset: 0x00309596
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x0030B1B0 File Offset: 0x003095B0
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x04003EFB RID: 16123
		private Func<double, double> processor;
	}
}
