using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C90 RID: 3216
	public class RegionLink
	{
		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x0600467E RID: 18046 RVA: 0x00252800 File Offset: 0x00250C00
		// (set) Token: 0x0600467F RID: 18047 RVA: 0x0025281D File Offset: 0x00250C1D
		public Region RegionA
		{
			get
			{
				return this.regions[0];
			}
			set
			{
				this.regions[0] = value;
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06004680 RID: 18048 RVA: 0x0025282C File Offset: 0x00250C2C
		// (set) Token: 0x06004681 RID: 18049 RVA: 0x00252849 File Offset: 0x00250C49
		public Region RegionB
		{
			get
			{
				return this.regions[1];
			}
			set
			{
				this.regions[1] = value;
			}
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x00252858 File Offset: 0x00250C58
		public void Register(Region reg)
		{
			if (this.regions[0] == reg || this.regions[1] == reg)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to double-register region ",
					reg.ToString(),
					" in ",
					this
				}), false);
			}
			else if (this.RegionA == null || !this.RegionA.valid)
			{
				this.RegionA = reg;
			}
			else if (this.RegionB == null || !this.RegionB.valid)
			{
				this.RegionB = reg;
			}
			else
			{
				Log.Error(string.Concat(new object[]
				{
					"Could not register region ",
					reg.ToString(),
					" in link ",
					this,
					": > 2 regions on link!\nRegionA: ",
					this.RegionA.DebugString,
					"\nRegionB: ",
					this.RegionB.DebugString
				}), false);
			}
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x00252960 File Offset: 0x00250D60
		public void Deregister(Region reg)
		{
			if (this.RegionA == reg)
			{
				this.RegionA = null;
				if (this.RegionB == null)
				{
					reg.Map.regionLinkDatabase.Notify_LinkHasNoRegions(this);
				}
			}
			else if (this.RegionB == reg)
			{
				this.RegionB = null;
				if (this.RegionA == null)
				{
					reg.Map.regionLinkDatabase.Notify_LinkHasNoRegions(this);
				}
			}
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x002529D8 File Offset: 0x00250DD8
		public Region GetOtherRegion(Region reg)
		{
			return (reg != this.RegionA) ? this.RegionA : this.RegionB;
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x00252A0C File Offset: 0x00250E0C
		public ulong UniqueHashCode()
		{
			return this.span.UniqueHashCode();
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x00252A2C File Offset: 0x00250E2C
		public override string ToString()
		{
			string text = (from r in this.regions
			where r != null
			select r.id.ToString()).ToCommaList(false);
			string text2 = string.Concat(new object[]
			{
				"span=",
				this.span.ToString(),
				" hash=",
				this.UniqueHashCode()
			});
			return string.Concat(new string[]
			{
				"(",
				text2,
				", regions=",
				text,
				")"
			});
		}

		// Token: 0x04003003 RID: 12291
		public Region[] regions = new Region[2];

		// Token: 0x04003004 RID: 12292
		public EdgeSpan span;
	}
}
