using System;

namespace Verse.Noise
{
	// Token: 0x02000F8F RID: 3983
	public class CurveSimple : ModuleBase
	{
		// Token: 0x04003F22 RID: 16162
		private SimpleCurve curve;

		// Token: 0x06006032 RID: 24626 RVA: 0x0030E217 File Offset: 0x0030C617
		public CurveSimple(ModuleBase input, SimpleCurve curve) : base(1)
		{
			this.modules[0] = input;
			this.curve = curve;
		}

		// Token: 0x06006033 RID: 24627 RVA: 0x0030E234 File Offset: 0x0030C634
		public override double GetValue(double x, double y, double z)
		{
			return (double)this.curve.Evaluate((float)this.modules[0].GetValue(x, y, z));
		}
	}
}
