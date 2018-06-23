using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037B RID: 891
	public class Area_Allowed : Area
	{
		// Token: 0x0400097B RID: 2427
		private string labelInt;

		// Token: 0x0400097C RID: 2428
		private Color colorInt = Color.red;

		// Token: 0x06000F64 RID: 3940 RVA: 0x000831DC File Offset: 0x000815DC
		public Area_Allowed()
		{
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x000831F0 File Offset: 0x000815F0
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
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x000832AC File Offset: 0x000816AC
		public override string Label
		{
			get
			{
				return this.labelInt;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x000832C8 File Offset: 0x000816C8
		public override Color Color
		{
			get
			{
				return this.colorInt;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x000832E4 File Offset: 0x000816E4
		public override bool Mutable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x000832FC File Offset: 0x000816FC
		public override int ListPriority
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00083318 File Offset: 0x00081718
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.labelInt, "label", null, false);
			Scribe_Values.Look<Color>(ref this.colorInt, "color", default(Color), false);
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00083358 File Offset: 0x00081758
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0008336E File Offset: 0x0008176E
		public override void SetLabel(string label)
		{
			this.labelInt = label;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x00083378 File Offset: 0x00081778
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

		// Token: 0x06000F6E RID: 3950 RVA: 0x000833C0 File Offset: 0x000817C0
		public override string ToString()
		{
			return this.labelInt;
		}
	}
}
