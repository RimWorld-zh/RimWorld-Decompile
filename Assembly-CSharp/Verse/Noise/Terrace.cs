using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F9D RID: 3997
	public class Terrace : ModuleBase
	{
		// Token: 0x0600605C RID: 24668 RVA: 0x0030CCD4 File Offset: 0x0030B0D4
		public Terrace() : base(1)
		{
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x0030CCF0 File Offset: 0x0030B0F0
		public Terrace(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600605E RID: 24670 RVA: 0x0030CD15 File Offset: 0x0030B115
		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.IsInverted = inverted;
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x0600605F RID: 24671 RVA: 0x0030CD44 File Offset: 0x0030B144
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06006060 RID: 24672 RVA: 0x0030CD64 File Offset: 0x0030B164
		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06006061 RID: 24673 RVA: 0x0030CD80 File Offset: 0x0030B180
		// (set) Token: 0x06006062 RID: 24674 RVA: 0x0030CD9B File Offset: 0x0030B19B
		public bool IsInverted
		{
			get
			{
				return this.m_inverted;
			}
			set
			{
				this.m_inverted = value;
			}
		}

		// Token: 0x06006063 RID: 24675 RVA: 0x0030CDA8 File Offset: 0x0030B1A8
		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((double lhs, double rhs) => lhs.CompareTo(rhs));
		}

		// Token: 0x06006064 RID: 24676 RVA: 0x0030CDFD File Offset: 0x0030B1FD
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06006065 RID: 24677 RVA: 0x0030CE0C File Offset: 0x0030B20C
		public void Generate(int steps)
		{
			if (steps < 2)
			{
				throw new ArgumentException("Need at least two steps");
			}
			this.Clear();
			double num = 2.0 / ((double)steps - 1.0);
			double num2 = -1.0;
			for (int i = 0; i < steps; i++)
			{
				this.Add(num2);
				num2 += num;
			}
		}

		// Token: 0x06006066 RID: 24678 RVA: 0x0030CE74 File Offset: 0x0030B274
		public override double GetValue(double x, double y, double z)
		{
			System.Diagnostics.Debug.Assert(this.modules[0] != null);
			System.Diagnostics.Debug.Assert(this.ControlPointCount >= 2);
			double value = this.modules[0].GetValue(x, y, z);
			int i;
			for (i = 0; i < this.m_data.Count; i++)
			{
				if (value < this.m_data[i])
				{
					break;
				}
			}
			int num = Mathf.Clamp(i - 1, 0, this.m_data.Count - 1);
			int num2 = Mathf.Clamp(i, 0, this.m_data.Count - 1);
			double result;
			if (num == num2)
			{
				result = this.m_data[num2];
			}
			else
			{
				double num3 = this.m_data[num];
				double num4 = this.m_data[num2];
				double num5 = (value - num3) / (num4 - num3);
				if (this.m_inverted)
				{
					num5 = 1.0 - num5;
					double num6 = num3;
					num3 = num4;
					num4 = num6;
				}
				num5 *= num5;
				result = Utils.InterpolateLinear(num3, num4, num5);
			}
			return result;
		}

		// Token: 0x04003F22 RID: 16162
		private List<double> m_data = new List<double>();

		// Token: 0x04003F23 RID: 16163
		private bool m_inverted = false;
	}
}
