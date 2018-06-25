using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F93 RID: 3987
	public class Invert : ModuleBase
	{
		// Token: 0x06006049 RID: 24649 RVA: 0x0030E3DC File Offset: 0x0030C7DC
		public Invert() : base(1)
		{
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x0030E3E6 File Offset: 0x0030C7E6
		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x0030E3FC File Offset: 0x0030C7FC
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
