using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037D RID: 893
	public class Area_BuildRoof : Area
	{
		// Token: 0x06000F77 RID: 3959 RVA: 0x000832E1 File Offset: 0x000816E1
		public Area_BuildRoof()
		{
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000832EA File Offset: 0x000816EA
		public Area_BuildRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x000832F4 File Offset: 0x000816F4
		public override string Label
		{
			get
			{
				return "BuildRoof".Translate();
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x00083314 File Offset: 0x00081714
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.9f, 0.5f);
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x00083340 File Offset: 0x00081740
		public override int ListPriority
		{
			get
			{
				return 9000;
			}
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0008335C File Offset: 0x0008175C
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_BuildRoof";
		}
	}
}
