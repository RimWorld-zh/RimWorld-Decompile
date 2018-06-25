using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9B RID: 3995
	public class Rotate : ModuleBase
	{
		// Token: 0x04003F28 RID: 16168
		private double m_x = 0.0;

		// Token: 0x04003F29 RID: 16169
		private double m_x1Matrix = 0.0;

		// Token: 0x04003F2A RID: 16170
		private double m_x2Matrix = 0.0;

		// Token: 0x04003F2B RID: 16171
		private double m_x3Matrix = 0.0;

		// Token: 0x04003F2C RID: 16172
		private double m_y = 0.0;

		// Token: 0x04003F2D RID: 16173
		private double m_y1Matrix = 0.0;

		// Token: 0x04003F2E RID: 16174
		private double m_y2Matrix = 0.0;

		// Token: 0x04003F2F RID: 16175
		private double m_y3Matrix = 0.0;

		// Token: 0x04003F30 RID: 16176
		private double m_z = 0.0;

		// Token: 0x04003F31 RID: 16177
		private double m_z1Matrix = 0.0;

		// Token: 0x04003F32 RID: 16178
		private double m_z2Matrix = 0.0;

		// Token: 0x04003F33 RID: 16179
		private double m_z3Matrix = 0.0;

		// Token: 0x0600605E RID: 24670 RVA: 0x0030E984 File Offset: 0x0030CD84
		public Rotate() : base(1)
		{
			this.SetAngles(0.0, 0.0, 0.0);
		}

		// Token: 0x0600605F RID: 24671 RVA: 0x0030EA70 File Offset: 0x0030CE70
		public Rotate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006060 RID: 24672 RVA: 0x0030EB44 File Offset: 0x0030CF44
		public Rotate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.SetAngles(x, y, z);
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06006061 RID: 24673 RVA: 0x0030EC20 File Offset: 0x0030D020
		// (set) Token: 0x06006062 RID: 24674 RVA: 0x0030EC3B File Offset: 0x0030D03B
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
		// (get) Token: 0x06006063 RID: 24675 RVA: 0x0030EC54 File Offset: 0x0030D054
		// (set) Token: 0x06006064 RID: 24676 RVA: 0x0030EC6F File Offset: 0x0030D06F
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
		// (get) Token: 0x06006065 RID: 24677 RVA: 0x0030EC88 File Offset: 0x0030D088
		// (set) Token: 0x06006066 RID: 24678 RVA: 0x0030ECA3 File Offset: 0x0030D0A3
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

		// Token: 0x06006067 RID: 24679 RVA: 0x0030ECBC File Offset: 0x0030D0BC
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

		// Token: 0x06006068 RID: 24680 RVA: 0x0030EDBC File Offset: 0x0030D1BC
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
