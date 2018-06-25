using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000381 RID: 897
	public class Area_SnowClear : Area
	{
		// Token: 0x06000F86 RID: 3974 RVA: 0x00083783 File Offset: 0x00081B83
		public Area_SnowClear()
		{
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0008378C File Offset: 0x00081B8C
		public Area_SnowClear(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x00083798 File Offset: 0x00081B98
		public override string Label
		{
			get
			{
				return "SnowClear".Translate();
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x000837B8 File Offset: 0x00081BB8
		public override Color Color
		{
			get
			{
				return new Color(0.8f, 0.1f, 0.1f);
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x000837E4 File Offset: 0x00081BE4
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00083800 File Offset: 0x00081C00
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_SnowClear";
		}
	}
}
