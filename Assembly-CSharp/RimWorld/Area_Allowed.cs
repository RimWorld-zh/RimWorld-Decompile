using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037D RID: 893
	public class Area_Allowed : Area
	{
		// Token: 0x0400097B RID: 2427
		private string labelInt;

		// Token: 0x0400097C RID: 2428
		private Color colorInt = Color.red;

		// Token: 0x06000F68 RID: 3944 RVA: 0x0008332C File Offset: 0x0008172C
		public Area_Allowed()
		{
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x00083340 File Offset: 0x00081740
		public Area_Allowed(AreaManager areaManager, string label = null) : base(areaManager)
		{
			this.areaManager = areaManager;
			if (!label.NullOrEmpty())
			{
				this.labelInt = label;
			}
			else
			{
				int num = 1;
				for (;;)
				{
					this.labelInt = "AreaDefaultLabel".Translate(new object[]
					{
						num
					});
					if (areaManager.GetLabeled(this.labelInt) == null)
					{
						break;
					}
					num++;
				}
			}
			this.colorInt = new Color(Rand.Value, Rand.Value, Rand.Value);
			this.colorInt = Color.Lerp(this.colorInt, Color.gray, 0.5f);
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x000833FC File Offset: 0x000817FC
		public override string Label
		{
			get
			{
				return this.labelInt;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x00083418 File Offset: 0x00081818
		public override Color Color
		{
			get
			{
				return this.colorInt;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x00083434 File Offset: 0x00081834
		public override bool Mutable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000F6D RID: 3949 RVA: 0x0008344C File Offset: 0x0008184C
		public override int ListPriority
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00083468 File Offset: 0x00081868
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.labelInt, "label", null, false);
			Scribe_Values.Look<Color>(ref this.colorInt, "color", default(Color), false);
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x000834A8 File Offset: 0x000818A8
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x000834BE File Offset: 0x000818BE
		public override void SetLabel(string label)
		{
			this.labelInt = label;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x000834C8 File Offset: 0x000818C8
		public override string GetUniqueLoadID()
		{
			return string.Concat(new object[]
			{
				"Area_",
				this.ID,
				"_Named_",
				this.labelInt
			});
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00083510 File Offset: 0x00081910
		public override string ToString()
		{
			return this.labelInt;
		}
	}
}
