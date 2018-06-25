using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000FA2 RID: 4002
	public class Terrace : ModuleBase
	{
		// Token: 0x04003F3F RID: 16191
		private List<double> m_data = new List<double>();

		// Token: 0x04003F40 RID: 16192
		private bool m_inverted = false;

		// Token: 0x0600608F RID: 24719 RVA: 0x0030F63C File Offset: 0x0030DA3C
		public Terrace() : base(1)
		{
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x0030F658 File Offset: 0x0030DA58
		public Terrace(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x0030F67D File Offset: 0x0030DA7D
		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.IsInverted = inverted;
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06006092 RID: 24722 RVA: 0x0030F6AC File Offset: 0x0030DAAC
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06006093 RID: 24723 RVA: 0x0030F6CC File Offset: 0x0030DACC
		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06006094 RID: 24724 RVA: 0x0030F6E8 File Offset: 0x0030DAE8
		// (set) Token: 0x06006095 RID: 24725 RVA: 0x0030F703 File Offset: 0x0030DB03
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

		// Token: 0x06006096 RID: 24726 RVA: 0x0030F710 File Offset: 0x0030DB10
		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((double lhs, double rhs) => lhs.CompareTo(rhs));
		}

		// Token: 0x06006097 RID: 24727 RVA: 0x0030F765 File Offset: 0x0030DB65
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06006098 RID: 24728 RVA: 0x0030F774 File Offset: 0x0030DB74
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

		// Token: 0x06006099 RID: 24729 RVA: 0x0030F7DC File Offset: 0x0030DBDC
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
	}
}
