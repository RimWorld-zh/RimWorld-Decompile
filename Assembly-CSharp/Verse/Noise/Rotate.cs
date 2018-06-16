using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F97 RID: 3991
	public class Rotate : ModuleBase
	{
		// Token: 0x0600602D RID: 24621 RVA: 0x0030BF40 File Offset: 0x0030A340
		public Rotate() : base(1)
		{
			this.SetAngles(0.0, 0.0, 0.0);
		}

		// Token: 0x0600602E RID: 24622 RVA: 0x0030C02C File Offset: 0x0030A42C
		public Rotate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600602F RID: 24623 RVA: 0x0030C100 File Offset: 0x0030A500
		public Rotate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.SetAngles(x, y, z);
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06006030 RID: 24624 RVA: 0x0030C1DC File Offset: 0x0030A5DC
		// (set) Token: 0x06006031 RID: 24625 RVA: 0x0030C1F7 File Offset: 0x0030A5F7
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

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06006032 RID: 24626 RVA: 0x0030C210 File Offset: 0x0030A610
		// (set) Token: 0x06006033 RID: 24627 RVA: 0x0030C22B File Offset: 0x0030A62B
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

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06006034 RID: 24628 RVA: 0x0030C244 File Offset: 0x0030A644
		// (set) Token: 0x06006035 RID: 24629 RVA: 0x0030C25F File Offset: 0x0030A65F
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

		// Token: 0x06006036 RID: 24630 RVA: 0x0030C278 File Offset: 0x0030A678
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

		// Token: 0x06006037 RID: 24631 RVA: 0x0030C378 File Offset: 0x0030A778
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double x2 = this.m_x1Matrix * x + this.m_y1Matrix * y + this.m_z1Matrix * z;
			double y2 = this.m_x2Matrix * x + this.m_y2Matrix * y + this.m_z2Matrix * z;
			double z2 = this.m_x3Matrix * x + this.m_y3Matrix * y + this.m_z3Matrix * z;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x04003F0C RID: 16140
		private double m_x = 0.0;

		// Token: 0x04003F0D RID: 16141
		private double m_x1Matrix = 0.0;

		// Token: 0x04003F0E RID: 16142
		private double m_x2Matrix = 0.0;

		// Token: 0x04003F0F RID: 16143
		private double m_x3Matrix = 0.0;

		// Token: 0x04003F10 RID: 16144
		private double m_y = 0.0;

		// Token: 0x04003F11 RID: 16145
		private double m_y1Matrix = 0.0;

		// Token: 0x04003F12 RID: 16146
		private double m_y2Matrix = 0.0;

		// Token: 0x04003F13 RID: 16147
		private double m_y3Matrix = 0.0;

		// Token: 0x04003F14 RID: 16148
		private double m_z = 0.0;

		// Token: 0x04003F15 RID: 16149
		private double m_z1Matrix = 0.0;

		// Token: 0x04003F16 RID: 16150
		private double m_z2Matrix = 0.0;

		// Token: 0x04003F17 RID: 16151
		private double m_z3Matrix = 0.0;
	}
}
