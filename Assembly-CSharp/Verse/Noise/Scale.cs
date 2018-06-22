using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F97 RID: 3991
	public class Scale : ModuleBase
	{
		// Token: 0x0600605F RID: 24671 RVA: 0x0030E581 File Offset: 0x0030C981
		public Scale() : base(1)
		{
		}

		// Token: 0x06006060 RID: 24672 RVA: 0x0030E5B8 File Offset: 0x0030C9B8
		public Scale(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006061 RID: 24673 RVA: 0x0030E5F8 File Offset: 0x0030C9F8
		public Scale(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06006062 RID: 24674 RVA: 0x0030E65C File Offset: 0x0030CA5C
		// (set) Token: 0x06006063 RID: 24675 RVA: 0x0030E677 File Offset: 0x0030CA77
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.m_x = value;
			}
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06006064 RID: 24676 RVA: 0x0030E684 File Offset: 0x0030CA84
		// (set) Token: 0x06006065 RID: 24677 RVA: 0x0030E69F File Offset: 0x0030CA9F
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.m_y = value;
			}
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06006066 RID: 24678 RVA: 0x0030E6AC File Offset: 0x0030CAAC
		// (set) Token: 0x06006067 RID: 24679 RVA: 0x0030E6C7 File Offset: 0x0030CAC7
		public double Z
		{
			get
			{
				return this.m_z;
			}
			set
			{
				this.m_z = value;
			}
		}

		// Token: 0x06006068 RID: 24680 RVA: 0x0030E6D4 File Offset: 0x0030CAD4
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x * this.m_x, y * this.m_y, z * this.m_z);
		}

		// Token: 0x04003F29 RID: 16169
		private double m_x = 1.0;

		// Token: 0x04003F2A RID: 16170
		private double m_y = 1.0;

		// Token: 0x04003F2B RID: 16171
		private double m_z = 1.0;
	}
}
