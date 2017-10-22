using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Mine : Designator
	{
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

		public Designator_Mine()
		{
			base.defaultLabel = "DesignatorMine".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine", true);
			base.defaultDesc = "DesignatorMineDesc".Translate();
			base.useMouseIcon = true;
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.soundSucceeded = SoundDefOf.DesignateMine;
			base.hotKey = KeyBindingDefOf.Misc10;
			base.tutorTag = "Mine";
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.Mine) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			if (c.Fogged(base.Map))
			{
				return true;
			}
			Thing thing = MineUtility.MineableInCell(c, base.Map);
			if (thing == null)
			{
				return "MessageMustDesignateMineable".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(thing);
			if (!result.Accepted)
			{
				return result;
			}
			return AcceptanceReport.WasAccepted;
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.mineable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			return true;
		}

		public override void DesignateSingleCell(IntVec3 loc)
		{
			base.Map.designationManager.AddDesignation(new Designation(loc, DesignationDefOf.Mine));
		}

		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Mining, KnowledgeAmount.SpecificInteraction);
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
