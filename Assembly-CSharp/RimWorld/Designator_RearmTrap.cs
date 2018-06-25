using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D6 RID: 2006
	public class Designator_RearmTrap : Designator
	{
		// Token: 0x06002C77 RID: 11383 RVA: 0x001770C0 File Offset: 0x001754C0
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
		// (get) Token: 0x06002C78 RID: 11384 RVA: 0x00177150 File Offset: 0x00175550
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002C79 RID: 11385 RVA: 0x00177168 File Offset: 0x00175568
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.RearmTrap;
			}
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x00177184 File Offset: 0x00175584
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

		// Token: 0x06002C7B RID: 11387 RVA: 0x001771DC File Offset: 0x001755DC
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.RearmablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x0017723C File Offset: 0x0017563C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building_TrapRearmable building_TrapRearmable = t as Building_TrapRearmable;
			return building_TrapRearmable != null && !building_TrapRearmable.Armed && base.Map.designationManager.DesignationOn(building_TrapRearmable, this.Designation) == null;
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x0017728B File Offset: 0x0017568B
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x001772B0 File Offset: 0x001756B0
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
