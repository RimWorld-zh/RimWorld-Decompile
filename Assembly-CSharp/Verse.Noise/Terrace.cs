#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse.Noise
{
	public class Terrace : ModuleBase
	{
		private List<double> m_data = new List<double>();

		private bool m_inverted = false;

		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

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

		public Terrace() : base(1)
		{
		}

		public Terrace(ModuleBase input) : base(1)
		{
			base.modules[0] = input;
		}

		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			base.modules[0] = input;
			this.IsInverted = inverted;
		}

		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((Comparison<double>)((double lhs, double rhs) => lhs.CompareTo(rhs)));
		}

		public void Clear()
		{
			this.m_data.Clear();
		}

		public void Generate(int steps)
		{
			if (steps < 2)
			{
				throw new ArgumentException("Need at least two steps");
			}
			this.Clear();
			double num = 2.0 / ((double)steps - 1.0);
			double num2 = -1.0;
			for (int num3 = 0; num3 < steps; num3++)
			{
				this.Add(num2);
				num2 += num;
			}
		}

		public override double GetValue(double x, double y, double z)
		{
			System.Diagnostics.Debug.Assert(base.modules[0] != null);
			System.Diagnostics.Debug.Assert(this.ControlPointCount >= 2);
			double value = base.modules[0].GetValue(x, y, z);
			int num = 0;
			while (num < this.m_data.Count && !(value < this.m_data[num]))
			{
				num++;
			}
			int num2 = Mathf.Clamp(num - 1, 0, this.m_data.Count - 1);
			int num3 = Mathf.Clamp(num, 0, this.m_data.Count - 1);
			double result;
			if (num2 == num3)
			{
				result = this.m_data[num3];
			}
			else
			{
				double num4 = this.m_data[num2];
				double num5 = this.m_data[num3];
				double num6 = (value - num4) / (num5 - num4);
				if (this.m_inverted)
				{
					num6 = 1.0 - num6;
					double num7 = num4;
					num4 = num5;
					num5 = num7;
				}
				num6 *= num6;
				result = Utils.InterpolateLinear(num4, num5, num6);
			}
			return result;
		}
	}
}
