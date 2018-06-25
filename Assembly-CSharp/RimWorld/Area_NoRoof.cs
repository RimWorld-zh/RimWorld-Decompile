using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000380 RID: 896
	public class Area_NoRoof : Area
	{
		// Token: 0x06000F80 RID: 3968 RVA: 0x000836D7 File Offset: 0x00081AD7
		public Area_NoRoof()
		{
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x000836E0 File Offset: 0x00081AE0
		public Area_NoRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x000836EC File Offset: 0x00081AEC
		public override string Label
		{
			get
			{
				return "NoRoof".Translate();
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x0008370C File Offset: 0x00081B0C
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.5f, 0.1f);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x00083738 File Offset: 0x00081B38
		public override int ListPriority
		{
			get
			{
				return 8000;
			}
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00083754 File Offset: 0x00081B54
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_NoRoof";
		}
	}
}
