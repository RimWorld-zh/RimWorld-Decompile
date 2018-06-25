using System;

namespace Verse.Noise
{
	// Token: 0x02000F89 RID: 3977
	public class Arbitrary : ModuleBase
	{
		// Token: 0x04003F10 RID: 16144
		private Func<double, double> processor;

		// Token: 0x06006014 RID: 24596 RVA: 0x0030D8B0 File Offset: 0x0030BCB0
		public Arbitrary() : base(1)
		{
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x0030D8BA File Offset: 0x0030BCBA
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x0030D8D4 File Offset: 0x0030BCD4
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}
	}
}
