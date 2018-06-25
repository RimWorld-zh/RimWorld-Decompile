using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C0 RID: 1984
	public abstract class Designator_AreaHome : Designator_Area
	{
		// Token: 0x0400179B RID: 6043
		private DesignateMode mode;

		// Token: 0x06002BF1 RID: 11249 RVA: 0x001747DA File Offset: 0x00172BDA
		public Designator_AreaHome(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002BF2 RID: 11250 RVA: 0x00174814 File Offset: 0x00172C14
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x0017482C File Offset: 0x00172C2C
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x00174844 File Offset: 0x00172C44
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

		// Token: 0x06002BF5 RID: 11253 RVA: 0x001748B0 File Offset: 0x00172CB0
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

		// Token: 0x06002BF6 RID: 11254 RVA: 0x001748FC File Offset: 0x00172CFC
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HomeArea, KnowledgeAmount.Total);
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x00174910 File Offset: 0x00172D10
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.Home.MarkForDraw();
		}
	}
}
