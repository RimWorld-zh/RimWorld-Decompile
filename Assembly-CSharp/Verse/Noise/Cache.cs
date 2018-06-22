using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F87 RID: 3975
	public class Cache : ModuleBase
	{
		// Token: 0x06006012 RID: 24594 RVA: 0x0030D38C File Offset: 0x0030B78C
		public Cache() : base(1)
		{
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x0030D3E4 File Offset: 0x0030B7E4
		public Cache(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000F72 RID: 3954
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

		// Token: 0x06006016 RID: 24598 RVA: 0x0030D478 File Offset: 0x0030B878
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

		// Token: 0x04003F0E RID: 16142
		private double m_value = 0.0;

		// Token: 0x04003F0F RID: 16143
		private bool m_cached = false;

		// Token: 0x04003F10 RID: 16144
		private double m_x = 0.0;

		// Token: 0x04003F11 RID: 16145
		private double m_y = 0.0;

		// Token: 0x04003F12 RID: 16146
		private double m_z = 0.0;
	}
}
