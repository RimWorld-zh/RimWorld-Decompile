using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F98 RID: 3992
	public class ScaleBias : ModuleBase
	{
		// Token: 0x04003F2C RID: 16172
		private double scale = 1.0;

		// Token: 0x04003F2D RID: 16173
		private double bias = 0.0;

		// Token: 0x06006069 RID: 24681 RVA: 0x0030E721 File Offset: 0x0030CB21
		public ScaleBias() : base(1)
		{
		}

		// Token: 0x0600606A RID: 24682 RVA: 0x0030E749 File Offset: 0x0030CB49
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600606B RID: 24683 RVA: 0x0030E77A File Offset: 0x0030CB7A
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x0600606C RID: 24684 RVA: 0x0030E7BC File Offset: 0x0030CBBC
		// (set) Token: 0x0600606D RID: 24685 RVA: 0x0030E7D7 File Offset: 0x0030CBD7
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

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x0600606E RID: 24686 RVA: 0x0030E7E4 File Offset: 0x0030CBE4
		// (set) Token: 0x0600606F RID: 24687 RVA: 0x0030E7FF File Offset: 0x0030CBFF
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

		// Token: 0x06006070 RID: 24688 RVA: 0x0030E80C File Offset: 0x0030CC0C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}
	}
}
