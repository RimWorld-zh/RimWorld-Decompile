using System;

namespace Verse.Noise
{
	// Token: 0x02000F83 RID: 3971
	public class Voronoi : ModuleBase
	{
		// Token: 0x04003EF5 RID: 16117
		private double m_displacement = 1.0;

		// Token: 0x04003EF6 RID: 16118
		private double m_frequency = 1.0;

		// Token: 0x04003EF7 RID: 16119
		private int m_seed = 0;

		// Token: 0x04003EF8 RID: 16120
		private bool m_distance = false;

		// Token: 0x06005FD3 RID: 24531 RVA: 0x0030C4E4 File Offset: 0x0030A8E4
		public Voronoi() : base(0)
		{
		}

		// Token: 0x06005FD4 RID: 24532 RVA: 0x0030C51C File Offset: 0x0030A91C
		public Voronoi(double frequency, double displacement, int seed, bool distance) : base(0)
		{
			this.Frequency = frequency;
			this.Displacement = displacement;
			this.Seed = seed;
			this.UseDistance = distance;
			this.Seed = seed;
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x06005FD5 RID: 24533 RVA: 0x0030C584 File Offset: 0x0030A984
		// (set) Token: 0x06005FD6 RID: 24534 RVA: 0x0030C59F File Offset: 0x0030A99F
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

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06005FD7 RID: 24535 RVA: 0x0030C5AC File Offset: 0x0030A9AC
		// (set) Token: 0x06005FD8 RID: 24536 RVA: 0x0030C5C7 File Offset: 0x0030A9C7
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

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06005FD9 RID: 24537 RVA: 0x0030C5D4 File Offset: 0x0030A9D4
		// (set) Token: 0x06005FDA RID: 24538 RVA: 0x0030C5EF File Offset: 0x0030A9EF
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

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06005FDB RID: 24539 RVA: 0x0030C5FC File Offset: 0x0030A9FC
		// (set) Token: 0x06005FDC RID: 24540 RVA: 0x0030C617 File Offset: 0x0030AA17
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

		// Token: 0x06005FDD RID: 24541 RVA: 0x0030C624 File Offset: 0x0030AA24
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
