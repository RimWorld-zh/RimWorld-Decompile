using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F2 RID: 2034
	public abstract class Designator_Plants : Designator
	{
		// Token: 0x06002D18 RID: 11544 RVA: 0x0017AD5F File Offset: 0x0017915F
		public Designator_Plants()
		{
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002D19 RID: 11545 RVA: 0x0017AD68 File Offset: 0x00179168
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002D1A RID: 11546 RVA: 0x0017AD80 File Offset: 0x00179180
		protected override DesignationDef Designation
		{
			get
			{
				return this.designationDef;
			}
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x0017AD9C File Offset: 0x0017919C
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

		// Token: 0x06002D1C RID: 11548 RVA: 0x0017ADFC File Offset: 0x001791FC
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

		// Token: 0x06002D1D RID: 11549 RVA: 0x0017AE88 File Offset: 0x00179288
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetPlant(base.Map));
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x0017AE9D File Offset: 0x0017929D
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.designationDef));
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x0017AED3 File Offset: 0x001792D3
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x040017B9 RID: 6073
		protected DesignationDef designationDef;
	}
}
