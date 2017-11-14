using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	public class Curve : ModuleBase
	{
		private List<KeyValuePair<double, double>> m_data = new List<KeyValuePair<double, double>>();

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

		public Curve()
			: base(1)
		{
		}

		public Curve(ModuleBase input)
			: base(1)
		{
			base.modules[0] = input;
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
			double value = base.modules[0].GetValue(x, y, z);
			int num = 0;
			while (num < this.m_data.Count && !(value < this.m_data[num].Key))
			{
				num++;
			}
			int index = Mathf.Clamp(num - 2, 0, this.m_data.Count - 1);
			int num2 = Mathf.Clamp(num - 1, 0, this.m_data.Count - 1);
			int num3 = Mathf.Clamp(num, 0, this.m_data.Count - 1);
			int index2 = Mathf.Clamp(num + 1, 0, this.m_data.Count - 1);
			if (num2 == num3)
			{
				return this.m_data[num2].Value;
			}
			double key = this.m_data[num2].Key;
			double key2 = this.m_data[num3].Key;
			double position = (value - key) / (key2 - key);
			return Utils.InterpolateCubic(this.m_data[index].Value, this.m_data[num2].Value, this.m_data[num3].Value, this.m_data[index2].Value, position);
		}
	}
}
