using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200069A RID: 1690
	public class Building_Vent : Building_TempControl
	{
		// Token: 0x040013FD RID: 5117
		private CompFlickable flickableComp;

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x00132E70 File Offset: 0x00131270
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x00132E90 File Offset: 0x00131290
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x00132EA7 File Offset: 0x001312A7
		public override void TickRare()
		{
			if (FlickUtility.WantsToBeOn(this))
			{
				GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			}
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x00132EC4 File Offset: 0x001312C4
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
