using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007EE RID: 2030
	public abstract class Designator_Plants : Designator
	{
		// Token: 0x040017B7 RID: 6071
		protected DesignationDef designationDef;

		// Token: 0x06002D11 RID: 11537 RVA: 0x0017AF37 File Offset: 0x00179337
		public Designator_Plants()
		{
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002D12 RID: 11538 RVA: 0x0017AF40 File Offset: 0x00179340
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002D13 RID: 11539 RVA: 0x0017AF58 File Offset: 0x00179358
		protected override DesignationDef Designation
		{
			get
			{
				return this.designationDef;
			}
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x0017AF74 File Offset: 0x00179374
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result;
			if (t.def.plant == null)
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationOn(t, this.designationDef) != null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x0017AFD4 File Offset: 0x001793D4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				result = false;
			}
			else
			{
				Plant plant = c.GetPlant(base.Map);
				if (plant == null)
				{
					result = "MessageMustDesignatePlants".Translate();
				}
				else
				{
					AcceptanceReport acceptanceReport = this.CanDesignateThing(plant);
					if (!acceptanceReport.Accepted)
					{
						result = acceptanceReport;
					}
					else
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x0017B060 File Offset: 0x00179460
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetPlant(base.Map));
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x0017B075 File Offset: 0x00179475
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.designationDef));
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x0017B0AB File Offset: 0x001794AB
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
