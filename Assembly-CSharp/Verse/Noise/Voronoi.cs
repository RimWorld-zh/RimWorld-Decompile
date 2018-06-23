using System;

namespace Verse.Noise
{
	// Token: 0x02000F7E RID: 3966
	public class Voronoi : ModuleBase
	{
		// Token: 0x04003EEA RID: 16106
		private double m_displacement = 1.0;

		// Token: 0x04003EEB RID: 16107
		private double m_frequency = 1.0;

		// Token: 0x04003EEC RID: 16108
		private int m_seed = 0;

		// Token: 0x04003EED RID: 16109
		private bool m_distance = false;

		// Token: 0x06005FC9 RID: 24521 RVA: 0x0030BC20 File Offset: 0x0030A020
		public Voronoi() : base(0)
		{
		}

		// Token: 0x06005FCA RID: 24522 RVA: 0x0030BC58 File Offset: 0x0030A058
		public Voronoi(double frequency, double displacement, int seed, bool distance) : base(0)
		{
			this.Frequency = frequency;
			this.Displacement = displacement;
			this.Seed = seed;
			this.UseDistance = distance;
			this.Seed = seed;
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06005FCB RID: 24523 RVA: 0x0030BCC0 File Offset: 0x0030A0C0
		// (set) Token: 0x06005FCC RID: 24524 RVA: 0x0030BCDB File Offset: 0x0030A0DB
		public double Displacement
		{
			get
			{
				return this.m_displacement;
			}
			set
			{
				this.m_displacement = value;
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06005FCD RID: 24525 RVA: 0x0030BCE8 File Offset: 0x0030A0E8
		// (set) Token: 0x06005FCE RID: 24526 RVA: 0x0030BD03 File Offset: 0x0030A103
		public double Frequency
		{
			get
			{
				return this.m_frequency;
			}
			set
			{
				this.m_frequency = value;
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06005FCF RID: 24527 RVA: 0x0030BD10 File Offset: 0x0030A110
		// (set) Token: 0x06005FD0 RID: 24528 RVA: 0x0030BD2B File Offset: 0x0030A12B
		public int Seed
		{
			get
			{
				return this.m_seed;
			}
			set
			{
				this.m_seed = value;
			}
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06005FD1 RID: 24529 RVA: 0x0030BD38 File Offset: 0x0030A138
		// (set) Token: 0x06005FD2 RID: 24530 RVA: 0x0030BD53 File Offset: 0x0030A153
		public bool UseDistance
		{
			get
			{
				return this.m_distance;
			}
			set
			{
				this.m_distance = value;
			}
		}

		// Token: 0x06005FD3 RID: 24531 RVA: 0x0030BD60 File Offset: 0x0030A160
		public override double GetValue(double x, double y, double z)
		{
			x *= this.m_frequency;
			y *= this.m_frequency;
			z *= this.m_frequency;
			int num = (x <= 0.0) ? ((int)x - 1) : ((int)x);
			int num2 = (y <= 0.0) ? ((int)y - 1) : ((int)y);
			int num3 = (z <= 0.0) ? ((int)z - 1) : ((int)z);
			double num4 = 2147483647.0;
			double num5 = 0.0;
			double num6 = 0.0;
			double num7 = 0.0;
			for (int i = num3 - 2; i <= num3 + 2; i++)
			{
				for (int j = num2 - 2; j <= num2 + 2; j++)
				{
					for (int k = num - 2; k <= num + 2; k++)
					{
						double num8 = (double)k + Utils.ValueNoise3D(k, j, i, this.m_seed);
						double num9 = (double)j + Utils.ValueNoise3D(k, j, i, this.m_seed + 1);
						double num10 = (double)i + Utils.ValueNoise3D(k, j, i, this.m_seed + 2);
						double num11 = num8 - x;
						double num12 = num9 - y;
						double num13 = num10 - z;
						double num14 = num11 * num11 + num12 * num12 + num13 * num13;
						if (num14 < num4)
						{
							num4 = num14;
							num5 = num8;
							num6 = num9;
							num7 = num10;
						}
					}
				}
			}
			double num18;
			if (this.m_distance)
			{
				double num15 = num5 - x;
				double num16 = num6 - y;
				double num17 = num7 - z;
				num18 = Math.Sqrt(num15 * num15 + num16 * num16 + num17 * num17) * 1.7320508075688772 - 1.0;
			}
			else
			{
				num18 = 0.0;
			}
			return num18 + this.m_displacement * Utils.ValueNoise3D((int)Math.Floor(num5), (int)Math.Floor(num6), (int)Math.Floor(num7), 0);
		}
	}
}
