using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F98 RID: 3992
	public class ScaleBias : ModuleBase
	{
		// Token: 0x06006040 RID: 24640 RVA: 0x0030C67D File Offset: 0x0030AA7D
		public ScaleBias() : base(1)
		{
		}

		// Token: 0x06006041 RID: 24641 RVA: 0x0030C6A5 File Offset: 0x0030AAA5
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006042 RID: 24642 RVA: 0x0030C6D6 File Offset: 0x0030AAD6
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x06006043 RID: 24643 RVA: 0x0030C718 File Offset: 0x0030AB18
		// (set) Token: 0x06006044 RID: 24644 RVA: 0x0030C733 File Offset: 0x0030AB33
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

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06006045 RID: 24645 RVA: 0x0030C740 File Offset: 0x0030AB40
		// (set) Token: 0x06006046 RID: 24646 RVA: 0x0030C75B File Offset: 0x0030AB5B
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

		// Token: 0x06006047 RID: 24647 RVA: 0x0030C768 File Offset: 0x0030AB68
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}

		// Token: 0x04003F1A RID: 16154
		private double scale = 1.0;

		// Token: 0x04003F1B RID: 16155
		private double bias = 0.0;
	}
}
