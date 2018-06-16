using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F84 RID: 3972
	public class Abs : ModuleBase
	{
		// Token: 0x06005FDD RID: 24541 RVA: 0x0030AFD1 File Offset: 0x003093D1
		public Abs() : base(1)
		{
		}

		// Token: 0x06005FDE RID: 24542 RVA: 0x0030AFDB File Offset: 0x003093DB
		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06005FDF RID: 24543 RVA: 0x0030AFF0 File Offset: 0x003093F0
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
