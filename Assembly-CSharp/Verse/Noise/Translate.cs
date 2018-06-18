using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9E RID: 3998
	public class Translate : ModuleBase
	{
		// Token: 0x06006068 RID: 24680 RVA: 0x0030CFBD File Offset: 0x0030B3BD
		public Translate() : base(1)
		{
		}

		// Token: 0x06006069 RID: 24681 RVA: 0x0030CFF4 File Offset: 0x0030B3F4
		public Translate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600606A RID: 24682 RVA: 0x0030D034 File Offset: 0x0030B434
		public Translate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x0600606B RID: 24683 RVA: 0x0030D098 File Offset: 0x0030B498
		// (set) Token: 0x0600606C RID: 24684 RVA: 0x0030D0B3 File Offset: 0x0030B4B3
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.m_x = value;
			}
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x0600606D RID: 24685 RVA: 0x0030D0C0 File Offset: 0x0030B4C0
		// (set) Token: 0x0600606E RID: 24686 RVA: 0x0030D0DB File Offset: 0x0030B4DB
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.m_y = value;
			}
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x0600606F RID: 24687 RVA: 0x0030D0E8 File Offset: 0x0030B4E8
		// (set) Token: 0x06006070 RID: 24688 RVA: 0x0030D103 File Offset: 0x0030B503
		public double Z
		{
			get
			{
				return this.m_z;
			}
			set
			{
				this.m_z = value;
			}
		}

		// Token: 0x06006071 RID: 24689 RVA: 0x0030D110 File Offset: 0x0030B510
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x + this.m_x, y + this.m_y, z + this.m_z);
		}

		// Token: 0x04003F25 RID: 16165
		private double m_x = 1.0;

		// Token: 0x04003F26 RID: 16166
		private double m_y = 1.0;

		// Token: 0x04003F27 RID: 16167
		private double m_z = 1.0;
	}
}
