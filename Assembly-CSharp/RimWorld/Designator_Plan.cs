using Verse;

namespace RimWorld
{
	public abstract class Designator_Plan : Designator
	{
		private DesignateMode mode;

		private DesignationDef desDef = DesignationDefOf.Plan;

		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		public Designator_Plan(DesignateMode mode)
		{
			this.mode = mode;
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.hotKey = KeyBindingDefOf.Misc9;
			this.desDef = DesignationDefOf.Plan;
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (c.InNoBuildEdgeArea(base.Map))
			{
				result = "TooCloseToMapEdge".Translate();
			}
			else
			{
				if (this.mode == DesignateMode.Add)
				{
					if (base.Map.designationManager.DesignationAt(c, this.desDef) != null)
					{
						result = false;
						goto IL_00c0;
					}
				}
				else if (this.mode == DesignateMode.Remove && base.Map.designationManager.DesignationAt(c, this.desDef) == null)
				{
					result = false;
					goto IL_00c0;
				}
				result = true;
			}
			goto IL_00c0;
			IL_00c0:
			return result;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.designationManager.AddDesignation(new Designation(c, this.desDef));
			}
			else if (this.mode == DesignateMode.Remove)
			{
				base.Map.designationManager.DesignationAt(c, this.desDef).Delete();
			}
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoBuildEdgeLines();
		}
	}
}
