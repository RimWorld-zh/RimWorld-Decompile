using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CE RID: 1998
	public class Designator_Haul : Designator
	{
		// Token: 0x06002C47 RID: 11335 RVA: 0x00175DD0 File Offset: 0x001741D0
		public Designator_Haul()
		{
			this.defaultLabel = "DesignatorHaulThings".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Haul", true);
			this.defaultDesc = "DesignatorHaulThingsDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Haul;
			this.hotKey = KeyBindingDefOf.Misc12;
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002C48 RID: 11336 RVA: 0x00175E48 File Offset: 0x00174248
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002C49 RID: 11337 RVA: 0x00175E60 File Offset: 0x00174260
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Haul;
			}
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x00175E7C File Offset: 0x0017427C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				result = false;
			}
			else
			{
				Thing firstHaulable = c.GetFirstHaulable(base.Map);
				if (firstHaulable == null)
				{
					result = "MessageMustDesignateHaulable".Translate();
				}
				else
				{
					AcceptanceReport acceptanceReport = this.CanDesignateThing(firstHaulable);
					if (!acceptanceReport.Accepted)
					{
						result = acceptanceReport;
					}
					else
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x00175F08 File Offset: 0x00174308
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetFirstHaulable(base.Map));
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x00175F20 File Offset: 0x00174320
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result;
			if (!t.def.designateHaulable)
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				result = false;
			}
			else if (t.IsInValidStorage())
			{
				result = "MessageAlreadyInStorage".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x00175F9F File Offset: 0x0017439F
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x00175FC3 File Offset: 0x001743C3
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
