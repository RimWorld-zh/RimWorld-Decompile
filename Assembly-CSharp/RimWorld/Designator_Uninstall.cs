using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E3 RID: 2019
	public class Designator_Uninstall : Designator
	{
		// Token: 0x06002CBD RID: 11453 RVA: 0x00178738 File Offset: 0x00176B38
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

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002CBE RID: 11454 RVA: 0x001787B0 File Offset: 0x00176BB0
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002CBF RID: 11455 RVA: 0x001787C8 File Offset: 0x00176BC8
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x001787E4 File Offset: 0x00176BE4
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

		// Token: 0x06002CC1 RID: 11457 RVA: 0x0017885D File Offset: 0x00176C5D
		public override void DesignateSingleCell(IntVec3 loc)
		{
			this.DesignateThing(this.TopUninstallableInCell(loc));
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x00178870 File Offset: 0x00176C70
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

		// Token: 0x06002CC3 RID: 11459 RVA: 0x00178918 File Offset: 0x00176D18
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

		// Token: 0x06002CC4 RID: 11460 RVA: 0x001789A4 File Offset: 0x00176DA4
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

		// Token: 0x06002CC5 RID: 11461 RVA: 0x00178AB1 File Offset: 0x00176EB1
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
