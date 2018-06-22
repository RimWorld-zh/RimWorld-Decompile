using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000698 RID: 1688
	public class Building_Vent : Building_TempControl
	{
		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x00132D20 File Offset: 0x00131120
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x00132D40 File Offset: 0x00131140
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x00132D57 File Offset: 0x00131157
		public override void TickRare()
		{
			if (FlickUtility.WantsToBeOn(this))
			{
				GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			}
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x00132D74 File Offset: 0x00131174
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

		// Token: 0x040013FD RID: 5117
		private CompFlickable flickableComp;
	}
}
