using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DE RID: 2014
	public class Designator_Unforbid : Designator
	{
		// Token: 0x06002CAF RID: 11439 RVA: 0x00178710 File Offset: 0x00176B10
		public Designator_Unforbid()
		{
			this.defaultLabel = "DesignatorUnforbid".Translate();
			this.defaultDesc = "DesignatorUnforbidDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Unforbid", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc6;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "UnforbidAllItems".Translate();
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x001787A0 File Offset: 0x00176BA0
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x001787B8 File Offset: 0x00176BB8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				result = false;
			}
			else if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				result = "MessageMustDesignateUnforbiddable".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x00178838 File Offset: 0x00176C38
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

		// Token: 0x06002CB3 RID: 11443 RVA: 0x00178894 File Offset: 0x00176C94
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result;
			if (t.def.category != ThingCategory.Item)
			{
				result = false;
			}
			else
			{
				CompForbiddable compForbiddable = t.TryGetComp<CompForbiddable>();
				result = (compForbiddable != null && compForbiddable.Forbidden);
			}
			return result;
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x001788E1 File Offset: 0x00176CE1
		public override void DesignateThing(Thing t)
		{
			t.SetForbidden(false, false);
		}
	}
}
