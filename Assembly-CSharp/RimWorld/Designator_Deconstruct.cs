using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Deconstruct : Designator
	{
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_Deconstruct()
		{
			base.defaultLabel = "DesignatorDeconstruct".Translate();
			base.defaultDesc = "DesignatorDeconstructDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/Deconstruct", true);
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateDeconstruct;
			base.hotKey = KeyBindingDefOf.DesignatorDeconstruct;
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (!DebugSettings.godMode && c.Fogged(base.Map))
			{
				result = false;
			}
			else
			{
				Thing thing = this.TopDeconstructibleInCell(c);
				result = ((thing != null) ? true : false);
			}
			return result;
		}

		public override void DesignateSingleCell(IntVec3 loc)
		{
			this.DesignateThing(this.TopDeconstructibleInCell(loc));
		}

		private Thing TopDeconstructibleInCell(IntVec3 loc)
		{
			foreach (Thing item in from t in base.Map.thingGrid.ThingsAt(loc)
			orderby t.def.altitudeLayer descending
			select t)
			{
				if (this.CanDesignateThing(item).Accepted)
				{
					return item;
				}
			}
			return null;
		}

		public override void DesignateThing(Thing t)
		{
			if (t.def.Claimable && t.Faction != Faction.OfPlayer)
			{
				t.SetFaction(Faction.OfPlayer, null);
			}
			Thing innerIfMinified = t.GetInnerIfMinified();
			if (DebugSettings.godMode || innerIfMinified.GetStatValue(StatDefOf.WorkToBuild, true) == 0.0 || t.def.IsFrame)
			{
				t.Destroy(DestroyMode.Deconstruct);
			}
			else
			{
				base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Deconstruct));
			}
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t.GetInnerIfMinified() as Building;
			AcceptanceReport result;
			if (building == null)
			{
				result = false;
			}
			else if (building.def.category != ThingCategory.Building)
			{
				result = false;
			}
			else
			{
				if (!DebugSettings.godMode)
				{
					if (!building.def.building.IsDeconstructible)
					{
						result = false;
						goto IL_0104;
					}
					if (building.Faction != Faction.OfPlayer && !building.ClaimableBy(Faction.OfPlayer) && !building.def.building.alwaysDeconstructible)
					{
						result = false;
						goto IL_0104;
					}
				}
				result = ((base.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) == null) ? ((base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) == null) ? true : false) : false);
			}
			goto IL_0104;
			IL_0104:
			return result;
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
