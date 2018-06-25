using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8C RID: 3980
	public class Cache : ModuleBase
	{
		// Token: 0x04003F19 RID: 16153
		private double m_value = 0.0;

		// Token: 0x04003F1A RID: 16154
		private bool m_cached = false;

		// Token: 0x04003F1B RID: 16155
		private double m_x = 0.0;

		// Token: 0x04003F1C RID: 16156
		private double m_y = 0.0;

		// Token: 0x04003F1D RID: 16157
		private double m_z = 0.0;

		// Token: 0x0600601C RID: 24604 RVA: 0x0030DC50 File Offset: 0x0030C050
		public Cache() : base(1)
		{
		}

		// Token: 0x0600601D RID: 24605 RVA: 0x0030DCA8 File Offset: 0x0030C0A8
		public Cache(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000F71 RID: 3953
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

		// Token: 0x06006020 RID: 24608 RVA: 0x0030DD3C File Offset: 0x0030C13C
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
	}
}
