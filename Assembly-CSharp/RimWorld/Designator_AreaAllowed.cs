using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class Designator_AreaAllowed : Designator_Area
	{
		private static Area selectedArea;

		public Designator_AreaAllowed(DesignateMode mode)
		{
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		public static Area SelectedArea
		{
			get
			{
				return Designator_AreaAllowed.selectedArea;
			}
		}

		public static void ClearSelectedArea()
		{
			Designator_AreaAllowed.selectedArea = null;
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			if (Designator_AreaAllowed.selectedArea != null && Find.WindowStack.FloatMenu == null)
			{
				Designator_AreaAllowed.selectedArea.MarkForDraw();
			}
		}

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

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private void <ProcessInput>__BaseCallProxy0(Event ev)
		{
			base.ProcessInput(ev);
		}

		[CompilerGenerated]
		private sealed class <ProcessInput>c__AnonStorey0
		{
			internal Event ev;

			internal Designator_AreaAllowed $this;

			public <ProcessInput>c__AnonStorey0()
			{
			}

			internal void <>m__0(Area a)
			{
				Designator_AreaAllowed.selectedArea = a;
				this.$this.<ProcessInput>__BaseCallProxy0(this.ev);
			}
		}
	}
}
