using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F2 RID: 2034
	public abstract class Designator_Plants : Designator
	{
		// Token: 0x06002D16 RID: 11542 RVA: 0x0017ACCB File Offset: 0x001790CB
		public Designator_Plants()
		{
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002D17 RID: 11543 RVA: 0x0017ACD4 File Offset: 0x001790D4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x0017ACEC File Offset: 0x001790EC
		protected override DesignationDef Designation
		{
			get
			{
				return this.designationDef;
			}
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x0017AD08 File Offset: 0x00179108
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

		// Token: 0x06002D1A RID: 11546 RVA: 0x0017AD68 File Offset: 0x00179168
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

		// Token: 0x06002D1B RID: 11547 RVA: 0x0017ADF4 File Offset: 0x001791F4
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetPlant(base.Map));
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x0017AE09 File Offset: 0x00179209
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.designationDef));
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x0017AE3F File Offset: 0x0017923F
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x040017B9 RID: 6073
		protected DesignationDef designationDef;
	}
}
