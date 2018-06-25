using System;

namespace Verse.Noise
{
	// Token: 0x02000F8E RID: 3982
	public class CurveSimple : ModuleBase
	{
		// Token: 0x04003F1A RID: 16154
		private SimpleCurve curve;

		// Token: 0x06006032 RID: 24626 RVA: 0x0030DFD3 File Offset: 0x0030C3D3
		public CurveSimple(ModuleBase input, SimpleCurve curve) : base(1)
		{
			this.modules[0] = input;
			this.curve = curve;
		}

		// Token: 0x06006033 RID: 24627 RVA: 0x0030DFF0 File Offset: 0x0030C3F0
		public override double GetValue(double x, double y, double z)
		{
			return (double)this.curve.Evaluate((float)this.modules[0].GetValue(x, y, z));
		}
	}
}
