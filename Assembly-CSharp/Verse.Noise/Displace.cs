#define DEBUG
using System.Diagnostics;

namespace Verse.Noise
{
	public class Displace : ModuleBase
	{
		public ModuleBase X
		{
			get
			{
				return base.modules[1];
			}
			set
			{
				Debug.Assert(value != null);
				base.modules[1] = value;
			}
		}

		public ModuleBase Y
		{
			get
			{
				return base.modules[2];
			}
			set
			{
				Debug.Assert(value != null);
				base.modules[2] = value;
			}
		}

		public ModuleBase Z
		{
			get
			{
				return base.modules[3];
			}
			set
			{
				Debug.Assert(value != null);
				base.modules[3] = value;
			}
		}

		public Displace() : base(4)
		{
		}

		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			base.modules[0] = input;
			base.modules[1] = x;
			base.modules[2] = y;
			base.modules[3] = z;
		}

		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(base.modules[0] != null);
			Debug.Assert(base.modules[1] != null);
			Debug.Assert(base.modules[2] != null);
			Debug.Assert(base.modules[3] != null);
			double x2 = x + base.modules[1].GetValue(x, y, z);
			double y2 = y + base.modules[2].GetValue(x, y, z);
			double z2 = z + base.modules[3].GetValue(x, y, z);
			return base.modules[0].GetValue(x2, y2, z2);
		}
	}
}
