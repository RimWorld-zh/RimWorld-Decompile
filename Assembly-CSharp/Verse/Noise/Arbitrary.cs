using System;

namespace Verse.Noise
{
	// Token: 0x02000F85 RID: 3973
	public class Arbitrary : ModuleBase
	{
		// Token: 0x0600600A RID: 24586 RVA: 0x0030D230 File Offset: 0x0030B630
		public Arbitrary() : base(1)
		{
		}

		// Token: 0x0600600B RID: 24587 RVA: 0x0030D23A File Offset: 0x0030B63A
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x0030D254 File Offset: 0x0030B654
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x04003F0D RID: 16141
		private Func<double, double> processor;
	}
}
