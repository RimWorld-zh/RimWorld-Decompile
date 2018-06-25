using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C8F RID: 3215
	public class RegionLink
	{
		// Token: 0x04003012 RID: 12306
		public Region[] regions = new Region[2];

		// Token: 0x04003013 RID: 12307
		public EdgeSpan span;

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06004688 RID: 18056 RVA: 0x00253F64 File Offset: 0x00252364
		// (set) Token: 0x06004689 RID: 18057 RVA: 0x00253F81 File Offset: 0x00252381
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
		// (get) Token: 0x0600468A RID: 18058 RVA: 0x00253F90 File Offset: 0x00252390
		// (set) Token: 0x0600468B RID: 18059 RVA: 0x00253FAD File Offset: 0x002523AD
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

		// Token: 0x0600468C RID: 18060 RVA: 0x00253FBC File Offset: 0x002523BC
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

		// Token: 0x0600468D RID: 18061 RVA: 0x002540C4 File Offset: 0x002524C4
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

		// Token: 0x0600468E RID: 18062 RVA: 0x0025413C File Offset: 0x0025253C
		public Region GetOtherRegion(Region reg)
		{
			return (reg != this.RegionA) ? this.RegionA : this.RegionB;
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x00254170 File Offset: 0x00252570
		public ulong UniqueHashCode()
		{
			return this.span.UniqueHashCode();
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x00254190 File Offset: 0x00252590
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
	}
}
