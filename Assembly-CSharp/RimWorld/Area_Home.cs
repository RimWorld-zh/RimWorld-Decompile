using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037C RID: 892
	public class Area_Home : Area
	{
		// Token: 0x06000F6F RID: 3951 RVA: 0x000833DB File Offset: 0x000817DB
		public Area_Home()
		{
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x000833E4 File Offset: 0x000817E4
		public Area_Home(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x000833F0 File Offset: 0x000817F0
		public override string Label
		{
			get
			{
				return "Home".Translate();
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x00083410 File Offset: 0x00081810
		public override Color Color
		{
			get
			{
				return new Color(0.3f, 0.3f, 0.9f);
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x0008343C File Offset: 0x0008183C
		public override int ListPriority
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00083458 File Offset: 0x00081858
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00083470 File Offset: 0x00081870
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_Home";
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0008349F File Offset: 0x0008189F
		protected override void Set(IntVec3 c, bool val)
		{
			if (base[c] != val)
			{
				base.Set(c, val);
				base.Map.listerFilthInHomeArea.Notify_HomeAreaChanged(c);
			}
		}
	}
}
