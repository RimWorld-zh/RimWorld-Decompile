using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Cancel : Designator
	{
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_Cancel()
		{
			base.defaultLabel = "DesignatorCancel".Translate();
			base.defaultDesc = "DesignatorCancelDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
			base.useMouseIcon = true;
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.soundSucceeded = SoundDefOf.DesignateCancel;
			base.hotKey = KeyBindingDefOf.DesignatorCancel;
			base.tutorTag = "Cancel";
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (this.CancelableDesignationsAt(c).Count() > 0)
			{
				result = true;
			}
			else
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (this.CanDesignateThing(thingList[i]).Accepted)
						goto IL_006b;
				}
				result = false;
			}
			goto IL_0094;
			IL_0094:
			return result;
			IL_006b:
			result = true;
			goto IL_0094;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Designation item in this.CancelableDesignationsAt(c).ToList())
			{
				if (item.def.designateCancelable)
				{
					base.Map.designationManager.RemoveDesignation(item);
				}
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int num = thingList.Count - 1; num >= 0; num--)
			{
				if (this.CanDesignateThing(thingList[num]).Accepted)
				{
					this.DesignateThing(thingList[num]);
				}
			}
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (base.Map.designationManager.DesignationOn(t) != null)
			{
				foreach (Designation item in base.Map.designationManager.AllDesignationsOn(t))
				{
					if (item.def.designateCancelable)
					{
						return true;
					}
				}
			}
			return (!t.def.mineable || base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) == null) ? (t.Faction == Faction.OfPlayer && (t is Frame || t is Blueprint)) : true;
		}

		public override void DesignateThing(Thing t)
		{
			if (t is Frame || t is Blueprint)
			{
				t.Destroy(DestroyMode.Cancel);
			}
			else
			{
				base.Map.designationManager.RemoveAllDesignationsOn(t, true);
				if (t.def.mineable)
				{
					Designation designation = base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine);
					if (designation != null)
					{
						base.Map.designationManager.RemoveDesignation(designation);
					}
				}
			}
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		private IEnumerable<Designation> CancelableDesignationsAt(IntVec3 c)
		{
			return from x in base.Map.designationManager.AllDesignationsAt(c)
			where x.def != DesignationDefOf.Plan
			select x;
		}
	}
}
