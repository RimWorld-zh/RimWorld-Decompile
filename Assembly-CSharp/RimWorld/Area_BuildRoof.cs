using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037D RID: 893
	public class Area_BuildRoof : Area
	{
		// Token: 0x06000F77 RID: 3959 RVA: 0x000834CD File Offset: 0x000818CD
		public Area_BuildRoof()
		{
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000834D6 File Offset: 0x000818D6
		public Area_BuildRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x000834E0 File Offset: 0x000818E0
		public override string Label
		{
			get
			{
				return "BuildRoof".Translate();
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x00083500 File Offset: 0x00081900
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.9f, 0.5f);
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x0008352C File Offset: 0x0008192C
		public override int ListPriority
		{
			get
			{
				return 9000;
			}
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00083548 File Offset: 0x00081948
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_BuildRoof";
		}
	}
}
