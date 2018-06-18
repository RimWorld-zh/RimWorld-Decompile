using System;

namespace Verse.Noise
{
	// Token: 0x02000F93 RID: 3987
	public class OneMinus : ModuleBase
	{
		// Token: 0x06006022 RID: 24610 RVA: 0x0030BEAC File Offset: 0x0030A2AC
		public OneMinus() : base(1)
		{
		}

		// Token: 0x06006023 RID: 24611 RVA: 0x0030BEB6 File Offset: 0x0030A2B6
		public OneMinus(ModuleBase module) : base(1)
		{
			this.modules[0] = module;
		}

		// Token: 0x06006024 RID: 24612 RVA: 0x0030BECC File Offset: 0x0030A2CC
		public override double GetValue(double x, double y, double z)
		{
			return 1.0 - this.modules[0].GetValue(x, y, z);
		}
	}
}
