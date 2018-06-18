using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C8F RID: 3215
	public class RegionLink
	{
		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x0600467C RID: 18044 RVA: 0x002527D8 File Offset: 0x00250BD8
		// (set) Token: 0x0600467D RID: 18045 RVA: 0x002527F5 File Offset: 0x00250BF5
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

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x0600467E RID: 18046 RVA: 0x00252804 File Offset: 0x00250C04
		// (set) Token: 0x0600467F RID: 18047 RVA: 0x00252821 File Offset: 0x00250C21
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

		// Token: 0x06004680 RID: 18048 RVA: 0x00252830 File Offset: 0x00250C30
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

		// Token: 0x06004681 RID: 18049 RVA: 0x00252938 File Offset: 0x00250D38
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

		// Token: 0x06004682 RID: 18050 RVA: 0x002529B0 File Offset: 0x00250DB0
		public Region GetOtherRegion(Region reg)
		{
			return (reg != this.RegionA) ? this.RegionA : this.RegionB;
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x002529E4 File Offset: 0x00250DE4
		public ulong UniqueHashCode()
		{
			return this.span.UniqueHashCode();
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x00252A04 File Offset: 0x00250E04
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

		// Token: 0x04003001 RID: 12289
		public Region[] regions = new Region[2];

		// Token: 0x04003002 RID: 12290
		public EdgeSpan span;
	}
}
