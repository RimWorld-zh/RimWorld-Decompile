using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C7 RID: 1991
	public class Designator_Claim : Designator
	{
		// Token: 0x06002C18 RID: 11288 RVA: 0x0017514C File Offset: 0x0017354C
		public Designator_Claim()
		{
			this.defaultLabel = "DesignatorClaim".Translate();
			this.defaultDesc = "DesignatorClaimDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Claim", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc4;
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002C19 RID: 11289 RVA: 0x001751C4 File Offset: 0x001735C4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x001751DC File Offset: 0x001735DC
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (c.Fogged(base.Map))
			{
				result = false;
			}
			else if (!(from t in c.GetThingList(base.Map)
			where this.CanDesignateThing(t).Accepted
			select t).Any<Thing>())
			{
				result = "MessageMustDesignateClaimable".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x00175270 File Offset: 0x00173670
		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x001752CC File Offset: 0x001736CC
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t as Building;
			return building != null && building.Faction != Faction.OfPlayer && building.ClaimableBy(Faction.OfPlayer);
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x00175314 File Offset: 0x00173714
		public override void DesignateThing(Thing t)
		{
			t.SetFaction(Faction.OfPlayer, null);
			CellRect.CellRectIterator iterator = t.OccupiedRect().GetIterator();
			while (!iterator.Done())
			{
				MoteMaker.ThrowMetaPuffs(new TargetInfo(iterator.Current, base.Map, false));
				iterator.MoveNext();
			}
		}
	}
}
