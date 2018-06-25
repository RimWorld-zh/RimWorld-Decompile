using System;
using Verse;

namespace RimWorld
{
	public abstract class Designator_Plants : Designator
	{
		protected DesignationDef designationDef;

		public Designator_Plants()
		{
		}

		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		protected override DesignationDef Designation
		{
			get
			{
				return this.designationDef;
			}
		}

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

		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetPlant(base.Map));
		}

		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.designationDef));
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
