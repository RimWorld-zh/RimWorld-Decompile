using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9D RID: 3997
	public class ScaleBias : ModuleBase
	{
		// Token: 0x04003F37 RID: 16183
		private double scale = 1.0;

		// Token: 0x04003F38 RID: 16184
		private double bias = 0.0;

		// Token: 0x06006073 RID: 24691 RVA: 0x0030EFE5 File Offset: 0x0030D3E5
		public ScaleBias() : base(1)
		{
		}

		// Token: 0x06006074 RID: 24692 RVA: 0x0030F00D File Offset: 0x0030D40D
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006075 RID: 24693 RVA: 0x0030F03E File Offset: 0x0030D43E
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06006076 RID: 24694 RVA: 0x0030F080 File Offset: 0x0030D480
		// (set) Token: 0x06006077 RID: 24695 RVA: 0x0030F09B File Offset: 0x0030D49B
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
		// (get) Token: 0x06006078 RID: 24696 RVA: 0x0030F0A8 File Offset: 0x0030D4A8
		// (set) Token: 0x06006079 RID: 24697 RVA: 0x0030F0C3 File Offset: 0x0030D4C3
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

		// Token: 0x0600607A RID: 24698 RVA: 0x0030F0D0 File Offset: 0x0030D4D0
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}
	}
}
