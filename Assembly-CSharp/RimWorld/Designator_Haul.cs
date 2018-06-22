using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CC RID: 1996
	public class Designator_Haul : Designator
	{
		// Token: 0x06002C43 RID: 11331 RVA: 0x00175C80 File Offset: 0x00174080
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
		// (get) Token: 0x06002C44 RID: 11332 RVA: 0x00175CF8 File Offset: 0x001740F8
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002C45 RID: 11333 RVA: 0x00175D10 File Offset: 0x00174110
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Haul;
			}
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x00175D2C File Offset: 0x0017412C
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

		// Token: 0x06002C47 RID: 11335 RVA: 0x00175DB8 File Offset: 0x001741B8
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetFirstHaulable(base.Map));
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x00175DD0 File Offset: 0x001741D0
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

		// Token: 0x06002C49 RID: 11337 RVA: 0x00175E4F File Offset: 0x0017424F
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x00175E73 File Offset: 0x00174273
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
