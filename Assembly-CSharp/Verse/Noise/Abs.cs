using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F83 RID: 3971
	public class Abs : ModuleBase
	{
		// Token: 0x06005FDB RID: 24539 RVA: 0x0030B0AD File Offset: 0x003094AD
		public Abs() : base(1)
		{
		}

		// Token: 0x06005FDC RID: 24540 RVA: 0x0030B0B7 File Offset: 0x003094B7
		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06005FDD RID: 24541 RVA: 0x0030B0CC File Offset: 0x003094CC
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
