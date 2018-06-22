using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8F RID: 3983
	public class Invert : ModuleBase
	{
		// Token: 0x0600603F RID: 24639 RVA: 0x0030DD5C File Offset: 0x0030C15C
		public Invert() : base(1)
		{
		}

		// Token: 0x06006040 RID: 24640 RVA: 0x0030DD66 File Offset: 0x0030C166
		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006041 RID: 24641 RVA: 0x0030DD7C File Offset: 0x0030C17C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
