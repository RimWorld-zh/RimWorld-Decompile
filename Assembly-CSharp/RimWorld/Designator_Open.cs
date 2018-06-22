using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D0 RID: 2000
	public class Designator_Open : Designator
	{
		// Token: 0x06002C61 RID: 11361 RVA: 0x001766A4 File Offset: 0x00174AA4
		public Designator_Open()
		{
			this.defaultLabel = "DesignatorOpen".Translate();
			this.defaultDesc = "DesignatorOpenDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Open", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002C62 RID: 11362 RVA: 0x00176714 File Offset: 0x00174B14
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002C63 RID: 11363 RVA: 0x0017672C File Offset: 0x00174B2C
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Open;
			}
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x00176746 File Offset: 0x00174B46
		protected override void FinalizeDesignationFailed()
		{
			base.FinalizeDesignationFailed();
			Messages.Message("MessageMustDesignateOpenable".Translate(), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x00176764 File Offset: 0x00174B64
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (!this.OpenablesInCell(c).Any<Thing>())
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x001767BC File Offset: 0x00174BBC
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.OpenablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x0017681C File Offset: 0x00174C1C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			IOpenable openable = t as IOpenable;
			AcceptanceReport result;
			if (openable == null || !openable.CanOpen || base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x00176877 File Offset: 0x00174C77
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x0017689C File Offset: 0x00174C9C
		private IEnumerable<Thing> OpenablesInCell(IntVec3 c)
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
