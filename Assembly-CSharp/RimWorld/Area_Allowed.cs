using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037B RID: 891
	public class Area_Allowed : Area
	{
		// Token: 0x06000F64 RID: 3940 RVA: 0x00082FF0 File Offset: 0x000813F0
		public Area_Allowed()
		{
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00083004 File Offset: 0x00081404
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
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x000830C0 File Offset: 0x000814C0
		public override string Label
		{
			get
			{
				return this.labelInt;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x000830DC File Offset: 0x000814DC
		public override Color Color
		{
			get
			{
				return this.colorInt;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x000830F8 File Offset: 0x000814F8
		public override bool Mutable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x00083110 File Offset: 0x00081510
		public override int ListPriority
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0008312C File Offset: 0x0008152C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.labelInt, "label", null, false);
			Scribe_Values.Look<Color>(ref this.colorInt, "color", default(Color), false);
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0008316C File Offset: 0x0008156C
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x00083182 File Offset: 0x00081582
		public override void SetLabel(string label)
		{
			this.labelInt = label;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0008318C File Offset: 0x0008158C
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

		// Token: 0x06000F6E RID: 3950 RVA: 0x000831D4 File Offset: 0x000815D4
		public override string ToString()
		{
			return this.labelInt;
		}

		// Token: 0x04000979 RID: 2425
		private string labelInt;

		// Token: 0x0400097A RID: 2426
		private Color colorInt = Color.red;
	}
}
