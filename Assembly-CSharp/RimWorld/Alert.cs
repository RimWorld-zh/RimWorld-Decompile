using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public abstract class Alert
	{
		public const float Width = 154f;

		private const float TextWidth = 148f;

		public const float Height = 28f;

		private const float ItemPeekWidth = 30f;

		public const float InfoRectWidth = 330f;

		protected AlertPriority defaultPriority;

		protected string defaultLabel;

		protected string defaultExplanation;

		protected float lastBellTime = -1000f;

		private AlertBounce alertBounce;

		private static readonly Texture2D AlertBGTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		private static readonly Texture2D AlertBGTexHighlight = TexUI.HighlightTex;

		public virtual AlertPriority Priority
		{
			get
			{
				return this.defaultPriority;
			}
		}

		protected virtual Color BGColor
		{
			get
			{
				return Color.clear;
			}
		}

		public virtual bool Active
		{
			get
			{
				AlertReport report = this.GetReport();
				return report.active;
			}
		}

		public abstract AlertReport GetReport();

		public virtual string GetExplanation()
		{
			return this.defaultExplanation;
		}

		public virtual string GetLabel()
		{
			return this.defaultLabel;
		}

		public void Notify_Started()
		{
			if ((int)this.Priority >= 1)
			{
				if (this.alertBounce == null)
				{
					this.alertBounce = new AlertBounce();
				}
				this.alertBounce.DoAlertStartEffect();
				if (Time.timeSinceLevelLoad > 1.0 && Time.realtimeSinceStartup > this.lastBellTime + 0.5)
				{
					SoundDefOf.TinyBell.PlayOneShotOnCamera(null);
					this.lastBellTime = Time.realtimeSinceStartup;
				}
			}
		}

		public virtual void AlertActiveUpdate()
		{
		}

		public virtual Rect DrawAt(float topY, bool minimized)
		{
			Text.Font = GameFont.Small;
			string label = this.GetLabel();
			float height = Text.CalcHeight(label, 148f);
			Rect rect = new Rect((float)((float)UI.screenWidth - 154.0), topY, 154f, height);
			if (this.alertBounce != null)
			{
				rect.x -= this.alertBounce.CalculateHorizontalOffset();
			}
			GUI.color = this.BGColor;
			GUI.DrawTexture(rect, Alert.AlertBGTex);
			GUI.color = Color.white;
			GUI.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(new Rect(0f, 0f, 148f, height), label);
			GUI.EndGroup();
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, Alert.AlertBGTexHighlight);
			}
			if (Widgets.ButtonInvisible(rect, false))
			{
				AlertReport report = this.GetReport();
				if (report.culprit.IsValid)
				{
					AlertReport report2 = this.GetReport();
					CameraJumper.TryJumpAndSelect(report2.culprit);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			return rect;
		}

		public void DrawInfoPane()
		{
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			string expString = this.GetExplanation();
			AlertReport report = this.GetReport();
			if (report.culprit.IsValid)
			{
				expString = expString + "\n\n(" + "ClickToJumpToProblem".Translate() + ")";
			}
			float num = Text.CalcHeight(expString, 310f);
			num = (float)(num + 20.0);
			double x = (float)UI.screenWidth - 154.0 - 330.0 - 8.0;
			Vector2 mousePosition = Event.current.mousePosition;
			Rect infoRect = new Rect((float)x, Mathf.Max(Mathf.Min(mousePosition.y, (float)UI.screenHeight - num), 0f), 330f, num);
			if (infoRect.yMax > (float)UI.screenHeight)
			{
				infoRect.y -= (float)UI.screenHeight - infoRect.yMax;
			}
			if (infoRect.y < 0.0)
			{
				infoRect.y = 0f;
			}
			Find.WindowStack.ImmediateWindow(138956, infoRect, WindowLayer.GameUI, (Action)delegate
			{
				Text.Font = GameFont.Small;
				Rect rect = infoRect.AtZero();
				Widgets.DrawWindowBackground(rect);
				Rect position = rect.ContractedBy(10f);
				GUI.BeginGroup(position);
				Widgets.Label(new Rect(0f, 0f, position.width, position.height), expString);
				GUI.EndGroup();
			}, false, false, 1f);
		}
	}
}
