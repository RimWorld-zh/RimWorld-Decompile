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
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.Mine) != null)
			{
				result = AcceptanceReport.WasRejected;
			}
			else if (c.Fogged(base.Map))
			{
				result = true;
			}
			else
			{
				Mineable firstMineable = c.GetFirstMineable(base.Map);
				if (firstMineable == null)
				{
					result = "MessageMustDesignateMineable".Translate();
				}
				else
				{
					AcceptanceReport acceptanceReport = this.CanDesignateThing(firstMineable);
					result = (acceptanceReport.Accepted ? AcceptanceReport.WasAccepted : acceptanceReport);
				}
			}
			return result;
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			return t.def.mineable ? ((base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) == null) ? true : AcceptanceReport.WasRejected) : false;
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
