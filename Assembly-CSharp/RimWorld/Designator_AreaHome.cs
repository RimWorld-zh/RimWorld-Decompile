using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C2 RID: 1986
	public abstract class Designator_AreaHome : Designator_Area
	{
		// Token: 0x06002BF3 RID: 11251 RVA: 0x001741BA File Offset: 0x001725BA
		public Designator_AreaHome(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x001741F4 File Offset: 0x001725F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002BF5 RID: 11253 RVA: 0x0017420C File Offset: 0x0017260C
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x00174224 File Offset: 0x00172624
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

		// Token: 0x06002BF7 RID: 11255 RVA: 0x00174290 File Offset: 0x00172690
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

		// Token: 0x06002BF8 RID: 11256 RVA: 0x001742DC File Offset: 0x001726DC
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HomeArea, KnowledgeAmount.Total);
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x001742F0 File Offset: 0x001726F0
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.Home.MarkForDraw();
		}

		// Token: 0x04001799 RID: 6041
		private DesignateMode mode;
	}
}
