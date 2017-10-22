using Verse;

namespace RimWorld
{
	public abstract class Designator_Plants : Designator
	{
		protected DesignationDef designationDef;

		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_Plants()
		{
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			return (t.def.plant != null) ? ((base.Map.designationManager.DesignationOn(t, this.designationDef) == null) ? true : false) : false;
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
					result = (acceptanceReport.Accepted ? true : acceptanceReport);
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
