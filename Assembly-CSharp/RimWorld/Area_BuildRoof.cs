using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037F RID: 895
	public class Area_BuildRoof : Area
	{
		// Token: 0x06000F7A RID: 3962 RVA: 0x0008362D File Offset: 0x00081A2D
		public Area_BuildRoof()
		{
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00083636 File Offset: 0x00081A36
		public Area_BuildRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x00083640 File Offset: 0x00081A40
		public override string Label
		{
			get
			{
				return "BuildRoof".Translate();
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x00083660 File Offset: 0x00081A60
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.9f, 0.5f);
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x0008368C File Offset: 0x00081A8C
		public override int ListPriority
		{
			get
			{
				return 9000;
			}
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x000836A8 File Offset: 0x00081AA8
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_BuildRoof";
		}
	}
}
