using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037E RID: 894
	public class Area_Home : Area
	{
		// Token: 0x06000F73 RID: 3955 RVA: 0x0008352B File Offset: 0x0008192B
		public Area_Home()
		{
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00083534 File Offset: 0x00081934
		public Area_Home(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x00083540 File Offset: 0x00081940
		public override string Label
		{
			get
			{
				return "Home".Translate();
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000F76 RID: 3958 RVA: 0x00083560 File Offset: 0x00081960
		public override Color Color
		{
			get
			{
				return new Color(0.3f, 0.3f, 0.9f);
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0008358C File Offset: 0x0008198C
		public override int ListPriority
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000835A8 File Offset: 0x000819A8
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x000835C0 File Offset: 0x000819C0
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_Home";
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x000835EF File Offset: 0x000819EF
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
