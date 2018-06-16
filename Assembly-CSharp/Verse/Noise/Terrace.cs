using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F9E RID: 3998
	public class Terrace : ModuleBase
	{
		// Token: 0x0600605E RID: 24670 RVA: 0x0030CBF8 File Offset: 0x0030AFF8
		public Terrace() : base(1)
		{
		}

		// Token: 0x0600605F RID: 24671 RVA: 0x0030CC14 File Offset: 0x0030B014
		public Terrace(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006060 RID: 24672 RVA: 0x0030CC39 File Offset: 0x0030B039
		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.IsInverted = inverted;
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06006061 RID: 24673 RVA: 0x0030CC68 File Offset: 0x0030B068
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06006062 RID: 24674 RVA: 0x0030CC88 File Offset: 0x0030B088
		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06006063 RID: 24675 RVA: 0x0030CCA4 File Offset: 0x0030B0A4
		// (set) Token: 0x06006064 RID: 24676 RVA: 0x0030CCBF File Offset: 0x0030B0BF
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

		// Token: 0x06006065 RID: 24677 RVA: 0x0030CCCC File Offset: 0x0030B0CC
		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((double lhs, double rhs) => lhs.CompareTo(rhs));
		}

		// Token: 0x06006066 RID: 24678 RVA: 0x0030CD21 File Offset: 0x0030B121
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06006067 RID: 24679 RVA: 0x0030CD30 File Offset: 0x0030B130
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

		// Token: 0x06006068 RID: 24680 RVA: 0x0030CD98 File Offset: 0x0030B198
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

		// Token: 0x04003F23 RID: 16163
		private List<double> m_data = new List<double>();

		// Token: 0x04003F24 RID: 16164
		private bool m_inverted = false;
	}
}
