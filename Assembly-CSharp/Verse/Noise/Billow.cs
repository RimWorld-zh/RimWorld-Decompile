using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F74 RID: 3956
	public class Billow : ModuleBase
	{
		// Token: 0x06005F58 RID: 24408 RVA: 0x00308CD4 File Offset: 0x003070D4
		public Billow() : base(0)
		{
		}

		// Token: 0x06005F59 RID: 24409 RVA: 0x00308D2C File Offset: 0x0030712C
		public Billow(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06005F5A RID: 24410 RVA: 0x00308DB0 File Offset: 0x003071B0
		// (set) Token: 0x06005F5B RID: 24411 RVA: 0x00308DCB File Offset: 0x003071CB
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

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06005F5C RID: 24412 RVA: 0x00308DD8 File Offset: 0x003071D8
		// (set) Token: 0x06005F5D RID: 24413 RVA: 0x00308DF3 File Offset: 0x003071F3
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

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x06005F5E RID: 24414 RVA: 0x00308E00 File Offset: 0x00307200
		// (set) Token: 0x06005F5F RID: 24415 RVA: 0x00308E1B File Offset: 0x0030721B
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

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06005F60 RID: 24416 RVA: 0x00308E28 File Offset: 0x00307228
		// (set) Token: 0x06005F61 RID: 24417 RVA: 0x00308E43 File Offset: 0x00307243
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

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06005F62 RID: 24418 RVA: 0x00308E58 File Offset: 0x00307258
		// (set) Token: 0x06005F63 RID: 24419 RVA: 0x00308E73 File Offset: 0x00307273
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

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06005F64 RID: 24420 RVA: 0x00308E80 File Offset: 0x00307280
		// (set) Token: 0x06005F65 RID: 24421 RVA: 0x00308E9B File Offset: 0x0030729B
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

		// Token: 0x06005F66 RID: 24422 RVA: 0x00308EA8 File Offset: 0x003072A8
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

		// Token: 0x04003EBC RID: 16060
		private double m_frequency = 1.0;

		// Token: 0x04003EBD RID: 16061
		private double m_lacunarity = 2.0;

		// Token: 0x04003EBE RID: 16062
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003EBF RID: 16063
		private int m_octaveCount = 6;

		// Token: 0x04003EC0 RID: 16064
		private double m_persistence = 0.5;

		// Token: 0x04003EC1 RID: 16065
		private int m_seed = 0;
	}
}
