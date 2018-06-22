using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200072A RID: 1834
	[StaticConstructorOnStartup]
	internal class Gizmo_RefuelableFuelStatus : Gizmo
	{
		// Token: 0x06002883 RID: 10371 RVA: 0x00159E67 File Offset: 0x00158267
		public Gizmo_RefuelableFuelStatus()
		{
			this.order = -100f;
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x00159E7C File Offset: 0x0015827C
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x00159E98 File Offset: 0x00158298
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Rect overRect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Find.WindowStack.ImmediateWindow(1523289473, overRect, WindowLayer.GameUI, delegate
			{
				Rect rect = overRect.AtZero().ContractedBy(6f);
				Rect rect2 = rect;
				rect2.height = overRect.height / 2f;
				Text.Font = GameFont.Tiny;
				Widgets.Label(rect2, this.refuelable.Props.FuelGizmoLabel);
				Rect rect3 = rect;
				rect3.yMin = overRect.height / 2f;
				float fillPercent = this.refuelable.Fuel / this.refuelable.Props.fuelCapacity;
				Widgets.FillableBar(rect3, fillPercent, Gizmo_RefuelableFuelStatus.FullBarTex, Gizmo_RefuelableFuelStatus.EmptyBarTex, false);
				if (this.refuelable.Props.targetFuelLevelConfigurable)
				{
					float num = this.refuelable.TargetFuelLevel / this.refuelable.Props.fuelCapacity;
					float x = rect3.x + num * rect3.width - (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.width * 0.5f / 2f;
					float y = rect3.y - (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.height * 0.5f;
					GUI.DrawTexture(new Rect(x, y, (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.width * 0.5f, (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.height * 0.5f), Gizmo_RefuelableFuelStatus.TargetLevelArrow);
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, this.refuelable.Fuel.ToString("F0") + " / " + this.refuelable.Props.fuelCapacity.ToString("F0"));
				Text.Anchor = TextAnchor.UpperLeft;
			}, true, false, 1f);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x04001634 RID: 5684
		public CompRefuelable refuelable;

		// Token: 0x04001635 RID: 5685
		private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.35f, 0.35f, 0.2f));

		// Token: 0x04001636 RID: 5686
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x04001637 RID: 5687
		private static readonly Texture2D TargetLevelArrow = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarkerRotated", true);

		// Token: 0x04001638 RID: 5688
		private const float ArrowScale = 0.5f;
	}
}
