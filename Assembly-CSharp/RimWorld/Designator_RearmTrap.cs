using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D8 RID: 2008
	public class Designator_RearmTrap : Designator
	{
		// Token: 0x06002C7B RID: 11387 RVA: 0x00176B34 File Offset: 0x00174F34
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

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002C7C RID: 11388 RVA: 0x00176BC4 File Offset: 0x00174FC4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002C7D RID: 11389 RVA: 0x00176BDC File Offset: 0x00174FDC
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.RearmTrap;
			}
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x00176BF8 File Offset: 0x00174FF8
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

		// Token: 0x06002C7F RID: 11391 RVA: 0x00176C50 File Offset: 0x00175050
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.RearmablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x00176CB0 File Offset: 0x001750B0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building_TrapRearmable building_TrapRearmable = t as Building_TrapRearmable;
			return building_TrapRearmable != null && !building_TrapRearmable.Armed && base.Map.designationManager.DesignationOn(building_TrapRearmable, this.Designation) == null;
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x00176CFF File Offset: 0x001750FF
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x00176D24 File Offset: 0x00175124
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
