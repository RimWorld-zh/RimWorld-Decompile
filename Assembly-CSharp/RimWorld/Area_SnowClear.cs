using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037F RID: 895
	public class Area_SnowClear : Area
	{
		// Token: 0x06000F83 RID: 3971 RVA: 0x00083437 File Offset: 0x00081837
		public Area_SnowClear()
		{
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00083440 File Offset: 0x00081840
		public Area_SnowClear(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x0008344C File Offset: 0x0008184C
		public override string Label
		{
			get
			{
				return "SnowClear".Translate();
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0008346C File Offset: 0x0008186C
		public override Color Color
		{
			get
			{
				return new Color(0.8f, 0.1f, 0.1f);
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x00083498 File Offset: 0x00081898
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x000834B4 File Offset: 0x000818B4
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_SnowClear";
		}
	}
}
