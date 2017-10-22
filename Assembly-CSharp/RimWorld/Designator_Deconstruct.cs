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
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!DebugSettings.godMode && c.Fogged(base.Map))
			{
				return false;
			}
			Thing thing = this.TopDeconstructibleInCell(c);
			if (thing == null)
			{
				return false;
			}
			return true;
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
			if (building == null)
			{
				return false;
			}
			if (building.def.category != ThingCategory.Building)
			{
				return false;
			}
			if (!DebugSettings.godMode)
			{
				if (!building.def.building.IsDeconstructible)
				{
					return false;
				}
				if (building.Faction != Faction.OfPlayer && !building.ClaimableBy(Faction.OfPlayer) && !building.def.building.alwaysDeconstructible)
				{
					return false;
				}
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
			{
				return false;
			}
			return true;
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
