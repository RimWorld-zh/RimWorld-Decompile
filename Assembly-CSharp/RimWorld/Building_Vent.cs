using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200069A RID: 1690
	public class Building_Vent : Building_TempControl
	{
		// Token: 0x04001401 RID: 5121
		private CompFlickable flickableComp;

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060023CB RID: 9163 RVA: 0x001330D8 File Offset: 0x001314D8
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x001330F8 File Offset: 0x001314F8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x0013310F File Offset: 0x0013150F
		public override void TickRare()
		{
			if (FlickUtility.WantsToBeOn(this))
			{
				GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			}
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x0013312C File Offset: 0x0013152C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (!FlickUtility.WantsToBeOn(this))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("VentClosed".Translate());
			}
			return stringBuilder.ToString();
		}
	}
}
