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
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (this.CancelableDesignationsAt(c).Count() > 0)
			{
				return true;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					return true;
				}
			}
			return false;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Designation>.Enumerator enumerator = this.CancelableDesignationsAt(c).ToList().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Designation current = enumerator.Current;
					if (current.def.designateCancelable)
					{
						base.Map.designationManager.RemoveDesignation(current);
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
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
			if (t.def.mineable && base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) != null)
			{
				return true;
			}
			return t.Faction == Faction.OfPlayer && (t is Frame || t is Blueprint);
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
