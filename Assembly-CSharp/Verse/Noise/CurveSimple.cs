using System;

namespace Verse.Noise
{
	// Token: 0x02000F8A RID: 3978
	public class CurveSimple : ModuleBase
	{
		// Token: 0x04003F17 RID: 16151
		private SimpleCurve curve;

		// Token: 0x06006028 RID: 24616 RVA: 0x0030D953 File Offset: 0x0030BD53
		public CurveSimple(ModuleBase input, SimpleCurve curve) : base(1)
		{
			this.modules[0] = input;
			this.curve = curve;
		}

		// Token: 0x06006029 RID: 24617 RVA: 0x0030D970 File Offset: 0x0030BD70
		public override double GetValue(double x, double y, double z)
		{
			return (double)this.curve.Evaluate((float)this.modules[0].GetValue(x, y, z));
		}
	}
}
