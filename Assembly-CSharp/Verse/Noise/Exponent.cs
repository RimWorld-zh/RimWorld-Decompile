using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8C RID: 3980
	public class Exponent : ModuleBase
	{
		// Token: 0x04003F18 RID: 16152
		private double m_exponent = 1.0;

		// Token: 0x06006033 RID: 24627 RVA: 0x0030DB2E File Offset: 0x0030BF2E
		public Exponent() : base(1)
		{
		}

		// Token: 0x06006034 RID: 24628 RVA: 0x0030DB47 File Offset: 0x0030BF47
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006035 RID: 24629 RVA: 0x0030DB69 File Offset: 0x0030BF69
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06006036 RID: 24630 RVA: 0x0030DB94 File Offset: 0x0030BF94
		// (set) Token: 0x06006037 RID: 24631 RVA: 0x0030DBAF File Offset: 0x0030BFAF
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

		// Token: 0x06006038 RID: 24632 RVA: 0x0030DBBC File Offset: 0x0030BFBC
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double value = this.modules[0].GetValue(x, y, z);
			return Math.Pow(Math.Abs((value + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}
	}
}
