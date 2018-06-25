using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9A RID: 3994
	public class Rotate : ModuleBase
	{
		// Token: 0x04003F20 RID: 16160
		private double m_x = 0.0;

		// Token: 0x04003F21 RID: 16161
		private double m_x1Matrix = 0.0;

		// Token: 0x04003F22 RID: 16162
		private double m_x2Matrix = 0.0;

		// Token: 0x04003F23 RID: 16163
		private double m_x3Matrix = 0.0;

		// Token: 0x04003F24 RID: 16164
		private double m_y = 0.0;

		// Token: 0x04003F25 RID: 16165
		private double m_y1Matrix = 0.0;

		// Token: 0x04003F26 RID: 16166
		private double m_y2Matrix = 0.0;

		// Token: 0x04003F27 RID: 16167
		private double m_y3Matrix = 0.0;

		// Token: 0x04003F28 RID: 16168
		private double m_z = 0.0;

		// Token: 0x04003F29 RID: 16169
		private double m_z1Matrix = 0.0;

		// Token: 0x04003F2A RID: 16170
		private double m_z2Matrix = 0.0;

		// Token: 0x04003F2B RID: 16171
		private double m_z3Matrix = 0.0;

		// Token: 0x0600605E RID: 24670 RVA: 0x0030E740 File Offset: 0x0030CB40
		public Rotate() : base(1)
		{
			this.SetAngles(0.0, 0.0, 0.0);
		}

		// Token: 0x0600605F RID: 24671 RVA: 0x0030E82C File Offset: 0x0030CC2C
		public Rotate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006060 RID: 24672 RVA: 0x0030E900 File Offset: 0x0030CD00
		public Rotate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.SetAngles(x, y, z);
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06006061 RID: 24673 RVA: 0x0030E9DC File Offset: 0x0030CDDC
		// (set) Token: 0x06006062 RID: 24674 RVA: 0x0030E9F7 File Offset: 0x0030CDF7
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.SetAngles(value, this.m_y, this.m_z);
			}
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06006063 RID: 24675 RVA: 0x0030EA10 File Offset: 0x0030CE10
		// (set) Token: 0x06006064 RID: 24676 RVA: 0x0030EA2B File Offset: 0x0030CE2B
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.SetAngles(this.m_x, value, this.m_z);
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x06006065 RID: 24677 RVA: 0x0030EA44 File Offset: 0x0030CE44
		// (set) Token: 0x06006066 RID: 24678 RVA: 0x0030EA5F File Offset: 0x0030CE5F
		public double Z
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.SetAngles(this.m_x, this.m_y, value);
			}
		}

		// Token: 0x06006067 RID: 24679 RVA: 0x0030EA78 File Offset: 0x0030CE78
		private void SetAngles(double x, double y, double z)
		{
			double num = Math.Cos(x * 0.017453292519943295);
			double num2 = Math.Cos(y * 0.017453292519943295);
			double num3 = Math.Cos(z * 0.017453292519943295);
			double num4 = Math.Sin(x * 0.017453292519943295);
			double num5 = Math.Sin(y * 0.017453292519943295);
			double num6 = Math.Sin(z * 0.017453292519943295);
			this.m_x1Matrix = num5 * num4 * num6 + num2 * num3;
			this.m_y1Matrix = num * num6;
			this.m_z1Matrix = num5 * num3 - num2 * num4 * num6;
			this.m_x2Matrix = num5 * num4 * num3 - num2 * num6;
			this.m_y2Matrix = num * num3;
			this.m_z2Matrix = -num2 * num4 * num3 - num5 * num6;
			this.m_x3Matrix = -num5 * num;
			this.m_y3Matrix = num4;
			this.m_z3Matrix = num2 * num;
			this.m_x = x;
			this.m_y = y;
			this.m_z = z;
		}

		// Token: 0x06006068 RID: 24680 RVA: 0x0030EB78 File Offset: 0x0030CF78
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double x2 = this.m_x1Matrix * x + this.m_y1Matrix * y + this.m_z1Matrix * z;
			double y2 = this.m_x2Matrix * x + this.m_y2Matrix * y + this.m_z2Matrix * z;
			double z2 = this.m_x3Matrix * x + this.m_y3Matrix * y + this.m_z3Matrix * z;
			return this.modules[0].GetValue(x2, y2, z2);
		}
	}
}
