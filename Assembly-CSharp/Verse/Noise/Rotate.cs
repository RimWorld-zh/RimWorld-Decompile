using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F96 RID: 3990
	public class Rotate : ModuleBase
	{
		// Token: 0x06006054 RID: 24660 RVA: 0x0030E0C0 File Offset: 0x0030C4C0
		public Rotate() : base(1)
		{
			this.SetAngles(0.0, 0.0, 0.0);
		}

		// Token: 0x06006055 RID: 24661 RVA: 0x0030E1AC File Offset: 0x0030C5AC
		public Rotate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x0030E280 File Offset: 0x0030C680
		public Rotate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.SetAngles(x, y, z);
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06006057 RID: 24663 RVA: 0x0030E35C File Offset: 0x0030C75C
		// (set) Token: 0x06006058 RID: 24664 RVA: 0x0030E377 File Offset: 0x0030C777
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

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x06006059 RID: 24665 RVA: 0x0030E390 File Offset: 0x0030C790
		// (set) Token: 0x0600605A RID: 24666 RVA: 0x0030E3AB File Offset: 0x0030C7AB
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

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x0600605B RID: 24667 RVA: 0x0030E3C4 File Offset: 0x0030C7C4
		// (set) Token: 0x0600605C RID: 24668 RVA: 0x0030E3DF File Offset: 0x0030C7DF
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

		// Token: 0x0600605D RID: 24669 RVA: 0x0030E3F8 File Offset: 0x0030C7F8
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

		// Token: 0x0600605E RID: 24670 RVA: 0x0030E4F8 File Offset: 0x0030C8F8
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double x2 = this.m_x1Matrix * x + this.m_y1Matrix * y + this.m_z1Matrix * z;
			double y2 = this.m_x2Matrix * x + this.m_y2Matrix * y + this.m_z2Matrix * z;
			double z2 = this.m_x3Matrix * x + this.m_y3Matrix * y + this.m_z3Matrix * z;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x04003F1D RID: 16157
		private double m_x = 0.0;

		// Token: 0x04003F1E RID: 16158
		private double m_x1Matrix = 0.0;

		// Token: 0x04003F1F RID: 16159
		private double m_x2Matrix = 0.0;

		// Token: 0x04003F20 RID: 16160
		private double m_x3Matrix = 0.0;

		// Token: 0x04003F21 RID: 16161
		private double m_y = 0.0;

		// Token: 0x04003F22 RID: 16162
		private double m_y1Matrix = 0.0;

		// Token: 0x04003F23 RID: 16163
		private double m_y2Matrix = 0.0;

		// Token: 0x04003F24 RID: 16164
		private double m_y3Matrix = 0.0;

		// Token: 0x04003F25 RID: 16165
		private double m_z = 0.0;

		// Token: 0x04003F26 RID: 16166
		private double m_z1Matrix = 0.0;

		// Token: 0x04003F27 RID: 16167
		private double m_z2Matrix = 0.0;

		// Token: 0x04003F28 RID: 16168
		private double m_z3Matrix = 0.0;
	}
}
