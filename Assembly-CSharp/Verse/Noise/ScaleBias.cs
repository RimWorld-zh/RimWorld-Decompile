using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9C RID: 3996
	public class ScaleBias : ModuleBase
	{
		// Token: 0x04003F2F RID: 16175
		private double scale = 1.0;

		// Token: 0x04003F30 RID: 16176
		private double bias = 0.0;

		// Token: 0x06006073 RID: 24691 RVA: 0x0030EDA1 File Offset: 0x0030D1A1
		public ScaleBias() : base(1)
		{
		}

		// Token: 0x06006074 RID: 24692 RVA: 0x0030EDC9 File Offset: 0x0030D1C9
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006075 RID: 24693 RVA: 0x0030EDFA File Offset: 0x0030D1FA
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06006076 RID: 24694 RVA: 0x0030EE3C File Offset: 0x0030D23C
		// (set) Token: 0x06006077 RID: 24695 RVA: 0x0030EE57 File Offset: 0x0030D257
		public double Bias
		{
			get
			{
				return this.bias;
			}
			set
			{
				this.bias = value;
			}
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06006078 RID: 24696 RVA: 0x0030EE64 File Offset: 0x0030D264
		// (set) Token: 0x06006079 RID: 24697 RVA: 0x0030EE7F File Offset: 0x0030D27F
		public double Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x0600607A RID: 24698 RVA: 0x0030EE8C File Offset: 0x0030D28C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}
	}
}
