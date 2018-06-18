using System;

namespace Verse.Noise
{
	// Token: 0x02000F8A RID: 3978
	public class CurveSimple : ModuleBase
	{
		// Token: 0x06005FFF RID: 24575 RVA: 0x0030B8AF File Offset: 0x00309CAF
		public CurveSimple(ModuleBase input, SimpleCurve curve) : base(1)
		{
			this.modules[0] = input;
			this.curve = curve;
		}

		// Token: 0x06006000 RID: 24576 RVA: 0x0030B8CC File Offset: 0x00309CCC
		public override double GetValue(double x, double y, double z)
		{
			return (double)this.curve.Evaluate((float)this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x04003F05 RID: 16133
		private SimpleCurve curve;
	}
}
