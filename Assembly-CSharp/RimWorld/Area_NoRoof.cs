using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037E RID: 894
	public class Area_NoRoof : Area
	{
		// Token: 0x06000F7D RID: 3965 RVA: 0x00083577 File Offset: 0x00081977
		public Area_NoRoof()
		{
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00083580 File Offset: 0x00081980
		public Area_NoRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x0008358C File Offset: 0x0008198C
		public override string Label
		{
			get
			{
				return "NoRoof".Translate();
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x000835AC File Offset: 0x000819AC
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.5f, 0.1f);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x000835D8 File Offset: 0x000819D8
		public override int ListPriority
		{
			get
			{
				return 8000;
			}
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000835F4 File Offset: 0x000819F4
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_NoRoof";
		}
	}
}
