using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BE RID: 1982
	public abstract class Designator_AreaHome : Designator_Area
	{
		// Token: 0x06002BEE RID: 11246 RVA: 0x00174426 File Offset: 0x00172826
		public Designator_AreaHome(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002BEF RID: 11247 RVA: 0x00174460 File Offset: 0x00172860
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002BF0 RID: 11248 RVA: 0x00174478 File Offset: 0x00172878
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x00174490 File Offset: 0x00172890
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else
			{
				bool flag = base.Map.areaManager.Home[c];
				if (this.mode == DesignateMode.Add)
				{
					result = !flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x001744FC File Offset: 0x001728FC
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.Home[c] = true;
			}
			else
			{
				base.Map.areaManager.Home[c] = false;
			}
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x00174548 File Offset: 0x00172948
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HomeArea, KnowledgeAmount.Total);
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x0017455C File Offset: 0x0017295C
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.Home.MarkForDraw();
		}

		// Token: 0x04001797 RID: 6039
		private DesignateMode mode;
	}
}
