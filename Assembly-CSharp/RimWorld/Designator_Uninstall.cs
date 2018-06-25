using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Uninstall : Designator
	{
		[CompilerGenerated]
		private static Func<Thing, AltitudeLayer> <>f__am$cache0;

		public Designator_Uninstall()
		{
			this.defaultLabel = "DesignatorUninstall".Translate();
			this.defaultDesc = "DesignatorUninstallDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Uninstall", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Misc12;
		}

		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
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
			else if (this.TopUninstallableInCell(c) == null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public override void DesignateSingleCell(IntVec3 loc)
		{
			this.DesignateThing(this.TopUninstallableInCell(loc));
		}

		private Thing TopUninstallableInCell(IntVec3 loc)
		{
			foreach (Thing thing in from t in base.Map.thingGrid.ThingsAt(loc)
			orderby t.def.altitudeLayer descending
			select t)
			{
				if (this.CanDesignateThing(thing).Accepted)
				{
					return thing;
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
			if (DebugSettings.godMode || t.GetStatValue(StatDefOf.WorkToBuild, true) == 0f || t.def.IsFrame)
			{
				t.Uninstall();
			}
			else
			{
				base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
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
				if (!DebugSettings.godMode)
				{
					if (building.Faction != Faction.OfPlayer)
					{
						if (building.Faction != null)
						{
							return false;
						}
						if (!building.ClaimableBy(Faction.OfPlayer))
						{
							return false;
						}
					}
				}
				if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
				{
					result = false;
				}
				else if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		[CompilerGenerated]
		private static AltitudeLayer <TopUninstallableInCell>m__0(Thing t)
		{
			return t.def.altitudeLayer;
		}
	}
}
