using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Strip : Designator
	{
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_Strip()
		{
			base.defaultLabel = "DesignatorStrip".Translate();
			base.defaultDesc = "DesignatorStripDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/Strip", true);
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateClaim;
			base.hotKey = KeyBindingDefOf.Misc11;
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) ? (this.StrippablesInCell(c).Any() ? true : "MessageMustDesignateStrippable".Translate()) : false;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing item in this.StrippablesInCell(c))
			{
				this.DesignateThing(item);
			}
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			return (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Strip) == null) ? StrippableUtility.CanBeStrippedByColony(t) : false;
		}

		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Strip));
		}

		private IEnumerable<Thing> StrippablesInCell(IntVec3 c)
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
