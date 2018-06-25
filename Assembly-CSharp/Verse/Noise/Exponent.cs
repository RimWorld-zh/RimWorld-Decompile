using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F90 RID: 3984
	public class Exponent : ModuleBase
	{
		// Token: 0x04003F1B RID: 16155
		private double m_exponent = 1.0;

		// Token: 0x0600603D RID: 24637 RVA: 0x0030E1AE File Offset: 0x0030C5AE
		public Exponent() : base(1)
		{
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x0030E1C7 File Offset: 0x0030C5C7
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600603F RID: 24639 RVA: 0x0030E1E9 File Offset: 0x0030C5E9
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06006040 RID: 24640 RVA: 0x0030E214 File Offset: 0x0030C614
		// (set) Token: 0x06006041 RID: 24641 RVA: 0x0030E22F File Offset: 0x0030C62F
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

		// Token: 0x06006042 RID: 24642 RVA: 0x0030E23C File Offset: 0x0030C63C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double value = this.modules[0].GetValue(x, y, z);
			return Math.Pow(Math.Abs((value + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}
	}
}
