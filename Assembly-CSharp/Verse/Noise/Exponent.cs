using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F91 RID: 3985
	public class Exponent : ModuleBase
	{
		// Token: 0x04003F23 RID: 16163
		private double m_exponent = 1.0;

		// Token: 0x0600603D RID: 24637 RVA: 0x0030E3F2 File Offset: 0x0030C7F2
		public Exponent() : base(1)
		{
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x0030E40B File Offset: 0x0030C80B
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600603F RID: 24639 RVA: 0x0030E42D File Offset: 0x0030C82D
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06006040 RID: 24640 RVA: 0x0030E458 File Offset: 0x0030C858
		// (set) Token: 0x06006041 RID: 24641 RVA: 0x0030E473 File Offset: 0x0030C873
		public double Value
		{
			get
			{
				return this.m_exponent;
			}
			set
			{
				this.m_exponent = value;
			}
		}

		// Token: 0x06006042 RID: 24642 RVA: 0x0030E480 File Offset: 0x0030C880
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double value = this.modules[0].GetValue(x, y, z);
			return Math.Pow(Math.Abs((value + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}
	}
}
