using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C2 RID: 1986
	public abstract class Designator_AreaHome : Designator_Area
	{
		// Token: 0x06002BF5 RID: 11253 RVA: 0x0017424E File Offset: 0x0017264E
		public Designator_AreaHome(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002BF6 RID: 11254 RVA: 0x00174288 File Offset: 0x00172688
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x001742A0 File Offset: 0x001726A0
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x001742B8 File Offset: 0x001726B8
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

		// Token: 0x06002BF9 RID: 11257 RVA: 0x00174324 File Offset: 0x00172724
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

		// Token: 0x06002BFA RID: 11258 RVA: 0x00174370 File Offset: 0x00172770
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HomeArea, KnowledgeAmount.Total);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x00174384 File Offset: 0x00172784
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.Home.MarkForDraw();
		}

		// Token: 0x04001799 RID: 6041
		private DesignateMode mode;
	}
}
