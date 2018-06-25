using System;

namespace Verse.Noise
{
	// Token: 0x02000F98 RID: 3992
	public class OneMinus : ModuleBase
	{
		// Token: 0x06006055 RID: 24661 RVA: 0x0030E814 File Offset: 0x0030CC14
		public OneMinus() : base(1)
		{
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x0030E81E File Offset: 0x0030CC1E
		public OneMinus(ModuleBase module) : base(1)
		{
			this.modules[0] = module;
		}

		// Token: 0x06006057 RID: 24663 RVA: 0x0030E834 File Offset: 0x0030CC34
		public override double GetValue(double x, double y, double z)
		{
			return 1.0 - this.modules[0].GetValue(x, y, z);
		}
	}
}
