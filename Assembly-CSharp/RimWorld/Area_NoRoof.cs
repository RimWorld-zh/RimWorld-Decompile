using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000380 RID: 896
	public class Area_NoRoof : Area
	{
		// Token: 0x06000F81 RID: 3969 RVA: 0x000836C7 File Offset: 0x00081AC7
		public Area_NoRoof()
		{
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000836D0 File Offset: 0x00081AD0
		public Area_NoRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x000836DC File Offset: 0x00081ADC
		public override string Label
		{
			get
			{
				return "NoRoof".Translate();
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x000836FC File Offset: 0x00081AFC
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.5f, 0.1f);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x00083728 File Offset: 0x00081B28
		public override int ListPriority
		{
			get
			{
				return 8000;
			}
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00083744 File Offset: 0x00081B44
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_NoRoof";
		}
	}
}
