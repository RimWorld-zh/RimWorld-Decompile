using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F0 RID: 2032
	public abstract class Designator_Plants : Designator
	{
		// Token: 0x040017BB RID: 6075
		protected DesignationDef designationDef;

		// Token: 0x06002D14 RID: 11540 RVA: 0x0017B2EB File Offset: 0x001796EB
		public Designator_Plants()
		{
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002D15 RID: 11541 RVA: 0x0017B2F4 File Offset: 0x001796F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x0017B30C File Offset: 0x0017970C
		protected override DesignationDef Designation
		{
			get
			{
				return this.designationDef;
			}
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x0017B328 File Offset: 0x00179728
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

		// Token: 0x06002D18 RID: 11544 RVA: 0x0017B388 File Offset: 0x00179788
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

		// Token: 0x06002D19 RID: 11545 RVA: 0x0017B414 File Offset: 0x00179814
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetPlant(base.Map));
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x0017B429 File Offset: 0x00179829
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.designationDef));
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x0017B45F File Offset: 0x0017985F
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
