using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_RearmTrap : Designator
	{
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_RearmTrap()
		{
			base.defaultLabel = "DesignatorRearmTrap".Translate();
			base.defaultDesc = "DesignatorRearmTrapDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/RearmTrap", true);
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateClaim;
			base.hotKey = KeyBindingDefOf.Misc7;
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.RearmablesInCell(c).Any())
			{
				return false;
			}
			return true;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing item in this.RearmablesInCell(c))
			{
				this.DesignateThing(item);
			}
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building_TrapRearmable building_TrapRearmable = t as Building_TrapRearmable;
			return building_TrapRearmable != null && !building_TrapRearmable.Armed && base.Map.designationManager.DesignationOn(building_TrapRearmable, DesignationDefOf.RearmTrap) == null;
		}

		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.RearmTrap));
		}

		private IEnumerable<Thing> RearmablesInCell(IntVec3 c)
		{
			if (!c.Fogged(base.Map))
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				int i = 0;
				while (true)
				{
					if (i < thingList.Count)
					{
						if (!this.CanDesignateThing(thingList[i]).Accepted)
						{
							i++;
							continue;
						}
						break;
					}
					yield break;
				}
				yield return thingList[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
