using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C8 RID: 1992
	public class Designator_Deconstruct : Designator
	{
		// Token: 0x06002C1F RID: 11295 RVA: 0x00175394 File Offset: 0x00173794
		public Designator_Deconstruct()
		{
			this.defaultLabel = "DesignatorDeconstruct".Translate();
			this.defaultDesc = "DesignatorDeconstructDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Deconstruct", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Designator_Deconstruct;
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002C20 RID: 11296 RVA: 0x0017540C File Offset: 0x0017380C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002C21 RID: 11297 RVA: 0x00175424 File Offset: 0x00173824
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x00175440 File Offset: 0x00173840
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
			else if (this.TopDeconstructibleInCell(c) == null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x001754B9 File Offset: 0x001738B9
		public override void DesignateSingleCell(IntVec3 loc)
		{
			this.DesignateThing(this.TopDeconstructibleInCell(loc));
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x001754CC File Offset: 0x001738CC
		private Thing TopDeconstructibleInCell(IntVec3 loc)
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

		// Token: 0x06002C25 RID: 11301 RVA: 0x00175574 File Offset: 0x00173974
		public override void DesignateThing(Thing t)
		{
			Thing innerIfMinified = t.GetInnerIfMinified();
			if (DebugSettings.godMode || innerIfMinified.GetStatValue(StatDefOf.WorkToBuild, true) == 0f || t.def.IsFrame)
			{
				t.Destroy(DestroyMode.Deconstruct);
			}
			else
			{
				base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			}
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x001755EC File Offset: 0x001739EC
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
			else if (!building.DeconstructibleBy(Faction.OfPlayer))
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x001756AD File Offset: 0x00173AAD
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
