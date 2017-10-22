using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Haul : Designator
	{
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_Haul()
		{
			base.defaultLabel = "DesignatorHaulThings".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/Haul", true);
			base.defaultDesc = "DesignatorHaulThingsDesc".Translate();
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateHaul;
			base.hotKey = KeyBindingDefOf.Misc12;
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (c.InBounds(base.Map) && !c.Fogged(base.Map))
			{
				Thing firstHaulable = c.GetFirstHaulable(base.Map);
				if (firstHaulable == null)
				{
					return "MessageMustDesignateHaulable".Translate();
				}
				AcceptanceReport result = this.CanDesignateThing(firstHaulable);
				if (!result.Accepted)
				{
					return result;
				}
				return true;
			}
			return false;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetFirstHaulable(base.Map));
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.designateHaulable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Haul) != null)
			{
				return false;
			}
			if (t.IsInValidStorage())
			{
				return "MessageAlreadyInStorage".Translate();
			}
			return true;
		}

		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Haul));
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
