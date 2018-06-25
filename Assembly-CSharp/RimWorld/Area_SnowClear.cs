using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000381 RID: 897
	public class Area_SnowClear : Area
	{
		// Token: 0x06000F87 RID: 3975 RVA: 0x00083773 File Offset: 0x00081B73
		public Area_SnowClear()
		{
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0008377C File Offset: 0x00081B7C
		public Area_SnowClear(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x00083788 File Offset: 0x00081B88
		public override string Label
		{
			get
			{
				return "SnowClear".Translate();
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x000837A8 File Offset: 0x00081BA8
		public override Color Color
		{
			get
			{
				return new Color(0.8f, 0.1f, 0.1f);
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x000837D4 File Offset: 0x00081BD4
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x000837F0 File Offset: 0x00081BF0
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_SnowClear";
		}
	}
}
