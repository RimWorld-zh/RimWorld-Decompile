using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037E RID: 894
	public class Area_Home : Area
	{
		// Token: 0x06000F72 RID: 3954 RVA: 0x0008353B File Offset: 0x0008193B
		public Area_Home()
		{
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00083544 File Offset: 0x00081944
		public Area_Home(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x00083550 File Offset: 0x00081950
		public override string Label
		{
			get
			{
				return "Home".Translate();
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x00083570 File Offset: 0x00081970
		public override Color Color
		{
			get
			{
				return new Color(0.3f, 0.3f, 0.9f);
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000F76 RID: 3958 RVA: 0x0008359C File Offset: 0x0008199C
		public override int ListPriority
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000835B8 File Offset: 0x000819B8
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000835D0 File Offset: 0x000819D0
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_Home";
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x000835FF File Offset: 0x000819FF
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
