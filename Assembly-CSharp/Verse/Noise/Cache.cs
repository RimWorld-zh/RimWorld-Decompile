using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F88 RID: 3976
	public class Cache : ModuleBase
	{
		// Token: 0x06005FEB RID: 24555 RVA: 0x0030B20C File Offset: 0x0030960C
		public Cache() : base(1)
		{
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x0030B264 File Offset: 0x00309664
		public Cache(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000F6F RID: 3951
		public override ModuleBase this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				base[index] = value;
				this.m_cached = false;
			}
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x0030B2F8 File Offset: 0x003096F8
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			if (!this.m_cached || this.m_x != x || this.m_y != y || this.m_z != z)
			{
				this.m_value = this.modules[0].GetValue(x, y, z);
				this.m_x = x;
				this.m_y = y;
				this.m_z = z;
			}
			this.m_cached = true;
			return this.m_value;
		}

		// Token: 0x04003EFD RID: 16125
		private double m_value = 0.0;

		// Token: 0x04003EFE RID: 16126
		private bool m_cached = false;

		// Token: 0x04003EFF RID: 16127
		private double m_x = 0.0;

		// Token: 0x04003F00 RID: 16128
		private double m_y = 0.0;

		// Token: 0x04003F01 RID: 16129
		private double m_z = 0.0;
	}
}
