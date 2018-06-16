using System;

namespace Verse.Noise
{
	// Token: 0x02000F7F RID: 3967
	public class Voronoi : ModuleBase
	{
		// Token: 0x06005FA2 RID: 24482 RVA: 0x00309AA0 File Offset: 0x00307EA0
		public Voronoi() : base(0)
		{
		}

		// Token: 0x06005FA3 RID: 24483 RVA: 0x00309AD8 File Offset: 0x00307ED8
		public Voronoi(double frequency, double displacement, int seed, bool distance) : base(0)
		{
			this.Frequency = frequency;
			this.Displacement = displacement;
			this.Seed = seed;
			this.UseDistance = distance;
			this.Seed = seed;
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x06005FA4 RID: 24484 RVA: 0x00309B40 File Offset: 0x00307F40
		// (set) Token: 0x06005FA5 RID: 24485 RVA: 0x00309B5B File Offset: 0x00307F5B
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

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06005FA6 RID: 24486 RVA: 0x00309B68 File Offset: 0x00307F68
		// (set) Token: 0x06005FA7 RID: 24487 RVA: 0x00309B83 File Offset: 0x00307F83
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

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x06005FA8 RID: 24488 RVA: 0x00309B90 File Offset: 0x00307F90
		// (set) Token: 0x06005FA9 RID: 24489 RVA: 0x00309BAB File Offset: 0x00307FAB
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

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06005FAA RID: 24490 RVA: 0x00309BB8 File Offset: 0x00307FB8
		// (set) Token: 0x06005FAB RID: 24491 RVA: 0x00309BD3 File Offset: 0x00307FD3
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

		// Token: 0x06005FAC RID: 24492 RVA: 0x00309BE0 File Offset: 0x00307FE0
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

		// Token: 0x04003ED9 RID: 16089
		private double m_displacement = 1.0;

		// Token: 0x04003EDA RID: 16090
		private double m_frequency = 1.0;

		// Token: 0x04003EDB RID: 16091
		private int m_seed = 0;

		// Token: 0x04003EDC RID: 16092
		private bool m_distance = false;
	}
}
