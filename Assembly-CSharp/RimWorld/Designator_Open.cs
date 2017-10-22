using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Open : Designator
	{
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_Open()
		{
			base.defaultLabel = "DesignatorOpen".Translate();
			base.defaultDesc = "DesignatorOpenDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/Open", true);
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateClaim;
		}

		protected override void FinalizeDesignationFailed()
		{
			base.FinalizeDesignationFailed();
			Messages.Message("MessageMustDesignateOpenable".Translate(), MessageSound.RejectInput);
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.OpenablesInCell(c).Any())
			{
				return false;
			}
			return true;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing item in this.OpenablesInCell(c))
			{
				this.DesignateThing(item);
			}
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			IOpenable openable = t as IOpenable;
			if (openable != null && openable.CanOpen && base.Map.designationManager.DesignationOn(t, DesignationDefOf.Open) == null)
			{
				return true;
			}
			return false;
		}

		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Open));
		}

		private IEnumerable<Thing> OpenablesInCell(IntVec3 c)
		{
			if (!c.Fogged(base.Map))
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (this.CanDesignateThing(thingList[i]).Accepted)
					{
						yield return thingList[i];
					}
				}
			}
		}
	}
}
