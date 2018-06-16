using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F99 RID: 3993
	public class ScaleBias : ModuleBase
	{
		// Token: 0x06006042 RID: 24642 RVA: 0x0030C5A1 File Offset: 0x0030A9A1
		public ScaleBias() : base(1)
		{
		}

		// Token: 0x06006043 RID: 24643 RVA: 0x0030C5C9 File Offset: 0x0030A9C9
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006044 RID: 24644 RVA: 0x0030C5FA File Offset: 0x0030A9FA
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06006045 RID: 24645 RVA: 0x0030C63C File Offset: 0x0030AA3C
		// (set) Token: 0x06006046 RID: 24646 RVA: 0x0030C657 File Offset: 0x0030AA57
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

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06006047 RID: 24647 RVA: 0x0030C664 File Offset: 0x0030AA64
		// (set) Token: 0x06006048 RID: 24648 RVA: 0x0030C67F File Offset: 0x0030AA7F
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

		// Token: 0x06006049 RID: 24649 RVA: 0x0030C68C File Offset: 0x0030AA8C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}

		// Token: 0x04003F1B RID: 16155
		private double scale = 1.0;

		// Token: 0x04003F1C RID: 16156
		private double bias = 0.0;
	}
}
