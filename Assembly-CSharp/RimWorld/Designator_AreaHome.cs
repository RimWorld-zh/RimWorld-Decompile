using Verse;

namespace RimWorld
{
	public abstract class Designator_AreaHome : Designator
	{
		private DesignateMode mode;

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

		public Designator_AreaHome(DesignateMode mode)
		{
			this.mode = mode;
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.hotKey = KeyBindingDefOf.Misc7;
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else
			{
				bool flag = ((Area)base.Map.areaManager.Home)[c];
				result = ((this.mode != 0) ? flag : (!flag));
			}
			return result;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				((Area)base.Map.areaManager.Home)[c] = true;
			}
			else
			{
				((Area)base.Map.areaManager.Home)[c] = false;
			}
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HomeArea, KnowledgeAmount.Total);
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.Home.MarkForDraw();
		}
	}
}
