using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DF RID: 2015
	public class Designator_Uninstall : Designator
	{
		// Token: 0x06002CB6 RID: 11446 RVA: 0x00178910 File Offset: 0x00176D10
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

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002CB7 RID: 11447 RVA: 0x00178988 File Offset: 0x00176D88
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002CB8 RID: 11448 RVA: 0x001789A0 File Offset: 0x00176DA0
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x001789BC File Offset: 0x00176DBC
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

		// Token: 0x06002CBA RID: 11450 RVA: 0x00178A35 File Offset: 0x00176E35
		public override void DesignateSingleCell(IntVec3 loc)
		{
			this.DesignateThing(this.TopUninstallableInCell(loc));
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x00178A48 File Offset: 0x00176E48
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

		// Token: 0x06002CBC RID: 11452 RVA: 0x00178AF0 File Offset: 0x00176EF0
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

		// Token: 0x06002CBD RID: 11453 RVA: 0x00178B7C File Offset: 0x00176F7C
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

		// Token: 0x06002CBE RID: 11454 RVA: 0x00178C89 File Offset: 0x00177089
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
