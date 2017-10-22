using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public abstract class Designator_Place : Designator
	{
		private const float RotButSize = 64f;

		private const float RotButSpacing = 10f;

		protected Rot4 placingRot = Rot4.North;

		protected static float middleMouseDownTime;

		public abstract BuildableDef PlacingDef
		{
			get;
		}

		public Designator_Place()
		{
			base.soundDragSustain = SoundDefOf.DesignateDragBuilding;
			base.soundDragChanged = SoundDefOf.DesignateDragBuildingChanged;
			base.soundSucceeded = SoundDefOf.DesignatePlaceBuilding;
		}

		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.rotatable)
			{
				Rect winRect = new Rect(leftX, (float)(bottomY - 90.0), 200f, 90f);
				Find.WindowStack.ImmediateWindow(73095, winRect, WindowLayer.GameUI, (Action)delegate
				{
					RotationDirection rotationDirection = RotationDirection.None;
					Text.Anchor = TextAnchor.MiddleCenter;
					Text.Font = GameFont.Medium;
					Rect rect = new Rect((float)(winRect.width / 2.0 - 64.0 - 5.0), 15f, 64f, 64f);
					if (Widgets.ButtonImage(rect, TexUI.RotLeftTex))
					{
						SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
						rotationDirection = RotationDirection.Counterclockwise;
						Event.current.Use();
					}
					Widgets.Label(rect, KeyBindingDefOf.DesignatorRotateLeft.MainKeyLabel);
					Rect rect2 = new Rect((float)(winRect.width / 2.0 + 5.0), 15f, 64f, 64f);
					if (Widgets.ButtonImage(rect2, TexUI.RotRightTex))
					{
						SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
						rotationDirection = RotationDirection.Clockwise;
						Event.current.Use();
					}
					Widgets.Label(rect2, KeyBindingDefOf.DesignatorRotateRight.MainKeyLabel);
					if (rotationDirection != 0)
					{
						this.placingRot.Rotate(rotationDirection);
					}
					Text.Anchor = TextAnchor.UpperLeft;
					Text.Font = GameFont.Small;
				}, true, false, 1f);
			}
		}

		public override void SelectedProcessInput(Event ev)
		{
			base.SelectedProcessInput(ev);
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.rotatable)
			{
				this.HandleRotationShortcuts();
			}
		}

		public override void SelectedUpdate()
		{
			GenDraw.DrawNoBuildEdgeLines();
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted))
			{
				IntVec3 intVec = UI.MouseCell();
				if (this.PlacingDef is TerrainDef)
				{
					GenUI.RenderMouseoverBracket();
				}
				else
				{
					Color ghostCol = (!this.CanDesignateCell(intVec).Accepted) ? new Color(1f, 0f, 0f, 0.4f) : new Color(0.5f, 1f, 0.6f, 0.4f);
					this.DrawGhost(ghostCol);
					if (this.CanDesignateCell(intVec).Accepted && this.PlacingDef.specialDisplayRadius > 0.0099999997764825821)
					{
						GenDraw.DrawRadiusRing(UI.MouseCell(), this.PlacingDef.specialDisplayRadius);
					}
					GenDraw.DrawInteractionCell((ThingDef)this.PlacingDef, intVec, this.placingRot);
				}
			}
		}

		protected virtual void DrawGhost(Color ghostCol)
		{
			GhostDrawer.DrawGhostThing(UI.MouseCell(), this.placingRot, (ThingDef)this.PlacingDef, null, ghostCol, AltitudeLayer.Blueprint);
		}

		private void HandleRotationShortcuts()
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
					Designator_Place.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.MouseUp && Time.realtimeSinceStartup - Designator_Place.middleMouseDownTime < 0.15000000596046448)
				{
					rotationDirection = RotationDirection.Clockwise;
				}
			}
			if (KeyBindingDefOf.DesignatorRotateRight.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Clockwise;
			}
			if (KeyBindingDefOf.DesignatorRotateLeft.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Counterclockwise;
			}
			if (rotationDirection == RotationDirection.Clockwise)
			{
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
				this.placingRot.Rotate(RotationDirection.Clockwise);
			}
			if (rotationDirection == RotationDirection.Counterclockwise)
			{
				SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
				this.placingRot.Rotate(RotationDirection.Counterclockwise);
			}
		}

		public override void Selected()
		{
			this.placingRot = this.PlacingDef.defaultPlacingRot;
		}
	}
}
