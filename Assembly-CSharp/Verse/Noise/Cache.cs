using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F87 RID: 3975
	public class Cache : ModuleBase
	{
		// Token: 0x06005FE9 RID: 24553 RVA: 0x0030B2E8 File Offset: 0x003096E8
		public Cache() : base(1)
		{
		}

		// Token: 0x06005FEA RID: 24554 RVA: 0x0030B340 File Offset: 0x00309740
		public Cache(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000F6E RID: 3950
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

		// Token: 0x06005FED RID: 24557 RVA: 0x0030B3D4 File Offset: 0x003097D4
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

		// Token: 0x04003EFC RID: 16124
		private double m_value = 0.0;

		// Token: 0x04003EFD RID: 16125
		private bool m_cached = false;

		// Token: 0x04003EFE RID: 16126
		private double m_x = 0.0;

		// Token: 0x04003EFF RID: 16127
		private double m_y = 0.0;

		// Token: 0x04003F00 RID: 16128
		private double m_z = 0.0;
	}
}
