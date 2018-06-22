using System;

namespace Verse.Noise
{
	// Token: 0x02000F93 RID: 3987
	public class OneMinus : ModuleBase
	{
		// Token: 0x0600604B RID: 24651 RVA: 0x0030DF50 File Offset: 0x0030C350
		public OneMinus() : base(1)
		{
		}

		// Token: 0x0600604C RID: 24652 RVA: 0x0030DF5A File Offset: 0x0030C35A
		public OneMinus(ModuleBase module) : base(1)
		{
			this.modules[0] = module;
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x0030DF70 File Offset: 0x0030C370
		public override double GetValue(double x, double y, double z)
		{
			return 1.0 - this.modules[0].GetValue(x, y, z);
		}
	}
}
