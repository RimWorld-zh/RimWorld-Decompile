using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200069C RID: 1692
	public class Building_Vent : Building_TempControl
	{
		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060023CE RID: 9166 RVA: 0x00132B60 File Offset: 0x00130F60
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x00132B80 File Offset: 0x00130F80
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x00132B97 File Offset: 0x00130F97
		public override void TickRare()
		{
			if (FlickUtility.WantsToBeOn(this))
			{
				GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			}
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x00132BB4 File Offset: 0x00130FB4
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

		// Token: 0x040013FF RID: 5119
		private CompFlickable flickableComp;
	}
}
