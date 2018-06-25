using System;

namespace Verse.Noise
{
	// Token: 0x02000F8A RID: 3978
	public class Arbitrary : ModuleBase
	{
		// Token: 0x04003F18 RID: 16152
		private Func<double, double> processor;

		// Token: 0x06006014 RID: 24596 RVA: 0x0030DAF4 File Offset: 0x0030BEF4
		public Arbitrary() : base(1)
		{
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x0030DAFE File Offset: 0x0030BEFE
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x0030DB18 File Offset: 0x0030BF18
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}
	}
}
