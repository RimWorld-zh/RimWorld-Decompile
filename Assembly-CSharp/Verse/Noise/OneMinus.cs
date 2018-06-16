using System;

namespace Verse.Noise
{
	// Token: 0x02000F94 RID: 3988
	public class OneMinus : ModuleBase
	{
		// Token: 0x06006024 RID: 24612 RVA: 0x0030BDD0 File Offset: 0x0030A1D0
		public OneMinus() : base(1)
		{
		}

		// Token: 0x06006025 RID: 24613 RVA: 0x0030BDDA File Offset: 0x0030A1DA
		public OneMinus(ModuleBase module) : base(1)
		{
			this.modules[0] = module;
		}

		// Token: 0x06006026 RID: 24614 RVA: 0x0030BDF0 File Offset: 0x0030A1F0
		public override double GetValue(double x, double y, double z)
		{
			return 1.0 - this.modules[0].GetValue(x, y, z);
		}
	}
}
