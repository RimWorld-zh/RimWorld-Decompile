using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F73 RID: 3955
	public class Billow : ModuleBase
	{
		// Token: 0x06005F56 RID: 24406 RVA: 0x00308DB0 File Offset: 0x003071B0
		public Billow() : base(0)
		{
		}

		// Token: 0x06005F57 RID: 24407 RVA: 0x00308E08 File Offset: 0x00307208
		public Billow(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06005F58 RID: 24408 RVA: 0x00308E8C File Offset: 0x0030728C
		// (set) Token: 0x06005F59 RID: 24409 RVA: 0x00308EA7 File Offset: 0x003072A7
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

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06005F5A RID: 24410 RVA: 0x00308EB4 File Offset: 0x003072B4
		// (set) Token: 0x06005F5B RID: 24411 RVA: 0x00308ECF File Offset: 0x003072CF
		public double Lacunarity
		{
			get
			{
				return this.m_lacunarity;
			}
			set
			{
				this.m_lacunarity = value;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06005F5C RID: 24412 RVA: 0x00308EDC File Offset: 0x003072DC
		// (set) Token: 0x06005F5D RID: 24413 RVA: 0x00308EF7 File Offset: 0x003072F7
		public QualityMode Quality
		{
			get
			{
				return this.m_quality;
			}
			set
			{
				this.m_quality = value;
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x06005F5E RID: 24414 RVA: 0x00308F04 File Offset: 0x00307304
		// (set) Token: 0x06005F5F RID: 24415 RVA: 0x00308F1F File Offset: 0x0030731F
		public int OctaveCount
		{
			get
			{
				return this.m_octaveCount;
			}
			set
			{
				this.m_octaveCount = Mathf.Clamp(value, 1, 30);
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06005F60 RID: 24416 RVA: 0x00308F34 File Offset: 0x00307334
		// (set) Token: 0x06005F61 RID: 24417 RVA: 0x00308F4F File Offset: 0x0030734F
		public double Persistence
		{
			get
			{
				return this.m_persistence;
			}
			set
			{
				this.m_persistence = value;
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06005F62 RID: 24418 RVA: 0x00308F5C File Offset: 0x0030735C
		// (set) Token: 0x06005F63 RID: 24419 RVA: 0x00308F77 File Offset: 0x00307377
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

		// Token: 0x06005F64 RID: 24420 RVA: 0x00308F84 File Offset: 0x00307384
		public override double GetValue(double x, double y, double z)
		{
			double num = 0.0;
			double num2 = 1.0;
			x *= this.m_frequency;
			y *= this.m_frequency;
			z *= this.m_frequency;
			for (int i = 0; i < this.m_octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long seed = (long)(this.m_seed + i) & (long)((ulong)-1);
				double num3 = Utils.GradientCoherentNoise3D(x2, y2, z2, seed, this.m_quality);
				num3 = 2.0 * Math.Abs(num3) - 1.0;
				num += num3 * num2;
				x *= this.m_lacunarity;
				y *= this.m_lacunarity;
				z *= this.m_lacunarity;
				num2 *= this.m_persistence;
			}
			return num + 0.5;
		}

		// Token: 0x04003EBB RID: 16059
		private double m_frequency = 1.0;

		// Token: 0x04003EBC RID: 16060
		private double m_lacunarity = 2.0;

		// Token: 0x04003EBD RID: 16061
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003EBE RID: 16062
		private int m_octaveCount = 6;

		// Token: 0x04003EBF RID: 16063
		private double m_persistence = 0.5;

		// Token: 0x04003EC0 RID: 16064
		private int m_seed = 0;
	}
}
