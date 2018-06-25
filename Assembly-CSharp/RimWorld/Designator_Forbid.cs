using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Forbid : Designator
	{
		public Designator_Forbid()
		{
			this.defaultLabel = "DesignatorForbid".Translate();
			this.defaultDesc = "DesignatorForbidDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Commands/Halt", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Command_ItemForbid;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "ForbidAllItems".Translate();
		}

		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				result = false;
			}
			else if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				result = "MessageMustDesignateForbiddable".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

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
				result = (compForbiddable != null && !compForbiddable.Forbidden);
			}
			return result;
		}

		public override void DesignateThing(Thing t)
		{
			t.SetForbidden(true, false);
		}

		[CompilerGenerated]
		private bool <CanDesignateCell>m__0(Thing t)
		{
			return this.CanDesignateThing(t).Accepted;
		}
	}
}
