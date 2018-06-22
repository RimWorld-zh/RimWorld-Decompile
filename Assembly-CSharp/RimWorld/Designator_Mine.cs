using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CF RID: 1999
	public class Designator_Mine : Designator
	{
		// Token: 0x06002C56 RID: 11350 RVA: 0x00176410 File Offset: 0x00174810
		public Designator_Mine()
		{
			this.defaultLabel = "DesignatorMine".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine", true);
			this.defaultDesc = "DesignatorMineDesc".Translate();
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_Mine;
			this.hotKey = KeyBindingDefOf.Misc10;
			this.tutorTag = "Mine";
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002C57 RID: 11351 RVA: 0x00176494 File Offset: 0x00174894
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002C58 RID: 11352 RVA: 0x001764AC File Offset: 0x001748AC
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002C59 RID: 11353 RVA: 0x001764C4 File Offset: 0x001748C4
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Mine;
			}
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x001764E0 File Offset: 0x001748E0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationAt(c, this.Designation) != null)
			{
				result = AcceptanceReport.WasRejected;
			}
			else if (c.Fogged(base.Map))
			{
				result = true;
			}
			else
			{
				Mineable firstMineable = c.GetFirstMineable(base.Map);
				if (firstMineable == null)
				{
					result = "MessageMustDesignateMineable".Translate();
				}
				else
				{
					AcceptanceReport acceptanceReport = this.CanDesignateThing(firstMineable);
					if (!acceptanceReport.Accepted)
					{
						result = acceptanceReport;
					}
					else
					{
						result = AcceptanceReport.WasAccepted;
					}
				}
			}
			return result;
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x001765A0 File Offset: 0x001749A0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result;
			if (!t.def.mineable)
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationAt(t.Position, this.Designation) != null)
			{
				result = AcceptanceReport.WasRejected;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x00176604 File Offset: 0x00174A04
		public override void DesignateSingleCell(IntVec3 loc)
		{
			base.Map.designationManager.AddDesignation(new Designation(loc, this.Designation));
			base.Map.designationManager.TryRemoveDesignation(loc, DesignationDefOf.SmoothWall);
			if (DebugSettings.godMode)
			{
				Mineable firstMineable = loc.GetFirstMineable(base.Map);
				if (firstMineable != null)
				{
					firstMineable.DestroyMined(null);
				}
			}
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x0017666F File Offset: 0x00174A6F
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x0017667E File Offset: 0x00174A7E
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Mining, KnowledgeAmount.SpecificInteraction);
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x00176692 File Offset: 0x00174A92
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x0017669A File Offset: 0x00174A9A
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
