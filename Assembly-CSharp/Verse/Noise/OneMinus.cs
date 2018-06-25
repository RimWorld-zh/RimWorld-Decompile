using System;

namespace Verse.Noise
{
	// Token: 0x02000F97 RID: 3991
	public class OneMinus : ModuleBase
	{
		// Token: 0x06006055 RID: 24661 RVA: 0x0030E5D0 File Offset: 0x0030C9D0
		public OneMinus() : base(1)
		{
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x0030E5DA File Offset: 0x0030C9DA
		public OneMinus(ModuleBase module) : base(1)
		{
			this.modules[0] = module;
		}

		// Token: 0x06006057 RID: 24663 RVA: 0x0030E5F0 File Offset: 0x0030C9F0
		public override double GetValue(double x, double y, double z)
		{
			return 1.0 - this.modules[0].GetValue(x, y, z);
		}
	}
}
