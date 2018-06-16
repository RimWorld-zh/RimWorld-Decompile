using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F98 RID: 3992
	public class Scale : ModuleBase
	{
		// Token: 0x06006038 RID: 24632 RVA: 0x0030C401 File Offset: 0x0030A801
		public Scale() : base(1)
		{
		}

		// Token: 0x06006039 RID: 24633 RVA: 0x0030C438 File Offset: 0x0030A838
		public Scale(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x0030C478 File Offset: 0x0030A878
		public Scale(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x0600603B RID: 24635 RVA: 0x0030C4DC File Offset: 0x0030A8DC
		// (set) Token: 0x0600603C RID: 24636 RVA: 0x0030C4F7 File Offset: 0x0030A8F7
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

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x0600603D RID: 24637 RVA: 0x0030C504 File Offset: 0x0030A904
		// (set) Token: 0x0600603E RID: 24638 RVA: 0x0030C51F File Offset: 0x0030A91F
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

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x0600603F RID: 24639 RVA: 0x0030C52C File Offset: 0x0030A92C
		// (set) Token: 0x06006040 RID: 24640 RVA: 0x0030C547 File Offset: 0x0030A947
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

		// Token: 0x06006041 RID: 24641 RVA: 0x0030C554 File Offset: 0x0030A954
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x * this.m_x, y * this.m_y, z * this.m_z);
		}

		// Token: 0x04003F18 RID: 16152
		private double m_x = 1.0;

		// Token: 0x04003F19 RID: 16153
		private double m_y = 1.0;

		// Token: 0x04003F1A RID: 16154
		private double m_z = 1.0;
	}
}
