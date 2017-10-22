using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Uninstall : Designator
	{
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_Uninstall()
		{
			base.defaultLabel = "DesignatorUninstall".Translate();
			base.defaultDesc = "DesignatorUninstallDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/Uninstall", true);
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateDeconstruct;
			base.hotKey = KeyBindingDefOf.Misc12;
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
				Thing thing = this.TopUninstallableInCell(c);
				result = ((thing != null) ? true : false);
			}
			return result;
		}

		public override void DesignateSingleCell(IntVec3 loc)
		{
			this.DesignateThing(this.TopUninstallableInCell(loc));
		}

		private Thing TopUninstallableInCell(IntVec3 loc)
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
			if (t.Faction != Faction.OfPlayer)
			{
				t.SetFaction(Faction.OfPlayer, null);
			}
			if (DebugSettings.godMode || t.GetStatValue(StatDefOf.WorkToBuild, true) == 0.0 || t.def.IsFrame)
			{
				t.Uninstall();
			}
			else
			{
				base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Uninstall));
			}
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t as Building;
			AcceptanceReport result;
			if (building == null)
			{
				result = false;
			}
			else if (building.def.category != ThingCategory.Building)
			{
				result = false;
			}
			else if (!building.def.Minifiable)
			{
				result = false;
			}
			else
			{
				if (!DebugSettings.godMode && building.Faction != Faction.OfPlayer)
				{
					if (building.Faction != null)
					{
						result = false;
						goto IL_00fe;
					}
					if (!building.ClaimableBy(Faction.OfPlayer))
					{
						result = false;
						goto IL_00fe;
					}
				}
				result = ((base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) == null) ? ((base.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) == null) ? true : false) : false);
			}
			goto IL_00fe;
			IL_00fe:
			return result;
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
