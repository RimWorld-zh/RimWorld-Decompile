using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9B RID: 3995
	public class Scale : ModuleBase
	{
		// Token: 0x04003F2C RID: 16172
		private double m_x = 1.0;

		// Token: 0x04003F2D RID: 16173
		private double m_y = 1.0;

		// Token: 0x04003F2E RID: 16174
		private double m_z = 1.0;

		// Token: 0x06006069 RID: 24681 RVA: 0x0030EC01 File Offset: 0x0030D001
		public Scale() : base(1)
		{
		}

		// Token: 0x0600606A RID: 24682 RVA: 0x0030EC38 File Offset: 0x0030D038
		public Scale(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600606B RID: 24683 RVA: 0x0030EC78 File Offset: 0x0030D078
		public Scale(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x0600606C RID: 24684 RVA: 0x0030ECDC File Offset: 0x0030D0DC
		// (set) Token: 0x0600606D RID: 24685 RVA: 0x0030ECF7 File Offset: 0x0030D0F7
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

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x0600606E RID: 24686 RVA: 0x0030ED04 File Offset: 0x0030D104
		// (set) Token: 0x0600606F RID: 24687 RVA: 0x0030ED1F File Offset: 0x0030D11F
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

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06006070 RID: 24688 RVA: 0x0030ED2C File Offset: 0x0030D12C
		// (set) Token: 0x06006071 RID: 24689 RVA: 0x0030ED47 File Offset: 0x0030D147
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

		// Token: 0x06006072 RID: 24690 RVA: 0x0030ED54 File Offset: 0x0030D154
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x * this.m_x, y * this.m_y, z * this.m_z);
		}
	}
}
