using System;

namespace Verse.Noise
{
	// Token: 0x02000F8B RID: 3979
	public class CurveSimple : ModuleBase
	{
		// Token: 0x06006001 RID: 24577 RVA: 0x0030B7D3 File Offset: 0x00309BD3
		public CurveSimple(ModuleBase input, SimpleCurve curve) : base(1)
		{
			this.modules[0] = input;
			this.curve = curve;
		}

		// Token: 0x06006002 RID: 24578 RVA: 0x0030B7F0 File Offset: 0x00309BF0
		public override double GetValue(double x, double y, double z)
		{
			return (double)this.curve.Evaluate((float)this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x04003F06 RID: 16134
		private SimpleCurve curve;
	}
}
