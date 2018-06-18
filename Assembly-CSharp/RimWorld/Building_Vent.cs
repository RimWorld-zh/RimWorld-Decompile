using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200069C RID: 1692
	public class Building_Vent : Building_TempControl
	{
		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x00132BD8 File Offset: 0x00130FD8
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x00132BF8 File Offset: 0x00130FF8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x00132C0F File Offset: 0x0013100F
		public override void TickRare()
		{
			if (FlickUtility.WantsToBeOn(this))
			{
				GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			}
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x00132C2C File Offset: 0x0013102C
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
