using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D4 RID: 2004
	public class Designator_RearmTrap : Designator
	{
		// Token: 0x06002C74 RID: 11380 RVA: 0x00176D0C File Offset: 0x0017510C
		public Designator_RearmTrap()
		{
			this.defaultLabel = "DesignatorRearmTrap".Translate();
			this.defaultDesc = "DesignatorRearmTrapDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RearmTrap", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "RearmAllTraps".Translate();
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002C75 RID: 11381 RVA: 0x00176D9C File Offset: 0x0017519C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002C76 RID: 11382 RVA: 0x00176DB4 File Offset: 0x001751B4
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.RearmTrap;
			}
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x00176DD0 File Offset: 0x001751D0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (!this.RearmablesInCell(c).Any<Thing>())
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x00176E28 File Offset: 0x00175228
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.RearmablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x00176E88 File Offset: 0x00175288
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building_TrapRearmable building_TrapRearmable = t as Building_TrapRearmable;
			return building_TrapRearmable != null && !building_TrapRearmable.Armed && base.Map.designationManager.DesignationOn(building_TrapRearmable, this.Designation) == null;
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x00176ED7 File Offset: 0x001752D7
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x00176EFC File Offset: 0x001752FC
		private IEnumerable<Thing> RearmablesInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return thingList[i];
				}
			}
			yield break;
		}
	}
}
