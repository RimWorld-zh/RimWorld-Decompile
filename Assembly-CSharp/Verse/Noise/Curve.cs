using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse.Noise
{
	public class Curve : ModuleBase
	{
		private List<KeyValuePair<double, double>> m_data = new List<KeyValuePair<double, double>>();

		[CompilerGenerated]
		private static Comparison<KeyValuePair<double, double>> <>f__am$cache0;

		public Curve() : base(1)
		{
		}

		public Curve(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		public List<KeyValuePair<double, double>> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		public void Add(double input, double output)
		{
			KeyValuePair<double, double> item = new KeyValuePair<double, double>(input, output);
			if (!this.m_data.Contains(item))
			{
				this.m_data.Add(item);
			}
			this.m_data.Sort((KeyValuePair<double, double> lhs, KeyValuePair<double, double> rhs) => lhs.Key.CompareTo(rhs.Key));
		}

		public void Clear()
		{
			this.m_data.Clear();
		}

		public override double GetValue(double x, double y, double z)
		{
			System.Diagnostics.Debug.Assert(this.modules[0] != null);
			System.Diagnostics.Debug.Assert(this.ControlPointCount >= 4);
			double value = this.modules[0].GetValue(x, y, z);
			int i;
			for (i = 0; i < this.m_data.Count; i++)
			{
				if (value < this.m_data[i].Key)
				{
					break;
				}
			}
			int index = Mathf.Clamp(i - 2, 0, this.m_data.Count - 1);
			int num = Mathf.Clamp(i - 1, 0, this.m_data.Count - 1);
			int num2 = Mathf.Clamp(i, 0, this.m_data.Count - 1);
			int index2 = Mathf.Clamp(i + 1, 0, this.m_data.Count - 1);
			double result;
			if (num == num2)
			{
				result = this.m_data[num].Value;
			}
			else
			{
				double key = this.m_data[num].Key;
				double key2 = this.m_data[num2].Key;
				double position = (value - key) / (key2 - key);
				result = Utils.InterpolateCubic(this.m_data[index].Value, this.m_data[num].Value, this.m_data[num2].Value, this.m_data[index2].Value, position);
			}
			return result;
		}

		[CompilerGenerated]
		private static int <Add>m__0(KeyValuePair<double, double> lhs, KeyValuePair<double, double> rhs)
		{
			return lhs.Key.CompareTo(rhs.Key);
		}
	}
}
