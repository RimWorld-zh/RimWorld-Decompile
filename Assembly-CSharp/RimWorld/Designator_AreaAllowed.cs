using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B9 RID: 1977
	public abstract class Designator_AreaAllowed : Designator_Area
	{
		// Token: 0x04001793 RID: 6035
		private static Area selectedArea;

		// Token: 0x06002BD8 RID: 11224 RVA: 0x00173EED File Offset: 0x001722ED
		public Designator_AreaAllowed(DesignateMode mode)
		{
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002BD9 RID: 11225 RVA: 0x00173F14 File Offset: 0x00172314
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002BDA RID: 11226 RVA: 0x00173F2C File Offset: 0x0017232C
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002BDB RID: 11227 RVA: 0x00173F44 File Offset: 0x00172344
		public static Area SelectedArea
		{
			get
			{
				return Designator_AreaAllowed.selectedArea;
			}
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x00173F5E File Offset: 0x0017235E
		public static void ClearSelectedArea()
		{
			Designator_AreaAllowed.selectedArea = null;
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x00173F67 File Offset: 0x00172367
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			if (Designator_AreaAllowed.selectedArea != null && Find.WindowStack.FloatMenu == null)
			{
				Designator_AreaAllowed.selectedArea.MarkForDraw();
			}
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x00173F94 File Offset: 0x00172394
		public override void ProcessInput(Event ev)
		{
			if (base.CheckCanInteract())
			{
				if (Designator_AreaAllowed.selectedArea != null)
				{
					base.ProcessInput(ev);
				}
				AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
				{
					Designator_AreaAllowed.selectedArea = a;
					this.<ProcessInput>__BaseCallProxy0(ev);
				}, false, true, base.Map);
			}
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x00173FF5 File Offset: 0x001723F5
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}
	}
}
