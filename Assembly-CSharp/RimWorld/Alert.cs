using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000785 RID: 1925
	[StaticConstructorOnStartup]
	public abstract class Alert
	{
		// Token: 0x04001702 RID: 5890
		protected AlertPriority defaultPriority = AlertPriority.Medium;

		// Token: 0x04001703 RID: 5891
		protected string defaultLabel;

		// Token: 0x04001704 RID: 5892
		protected string defaultExplanation;

		// Token: 0x04001705 RID: 5893
		protected float lastBellTime = -1000f;

		// Token: 0x04001706 RID: 5894
		private int jumpToTargetCycleIndex;

		// Token: 0x04001707 RID: 5895
		private AlertBounce alertBounce = null;

		// Token: 0x04001708 RID: 5896
		public const float Width = 154f;

		// Token: 0x04001709 RID: 5897
		private const float TextWidth = 148f;

		// Token: 0x0400170A RID: 5898
		public const float Height = 28f;

		// Token: 0x0400170B RID: 5899
		private const float ItemPeekWidth = 30f;

		// Token: 0x0400170C RID: 5900
		public const float InfoRectWidth = 330f;

		// Token: 0x0400170D RID: 5901
		private static readonly Texture2D AlertBGTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x0400170E RID: 5902
		private static readonly Texture2D AlertBGTexHighlight = TexUI.HighlightTex;

		// Token: 0x0400170F RID: 5903
		private static List<GlobalTargetInfo> tmpTargets = new List<GlobalTargetInfo>();

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06002AB2 RID: 10930 RVA: 0x00169870 File Offset: 0x00167C70
		public virtual AlertPriority Priority
		{
			get
			{
				return this.defaultPriority;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x0016988C File Offset: 0x00167C8C
		protected virtual Color BGColor
		{
			get
			{
				return Color.clear;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002AB4 RID: 10932 RVA: 0x001698A8 File Offset: 0x00167CA8
		public virtual bool Active
		{
			get
			{
				return this.GetReport().active;
			}
		}

		// Token: 0x06002AB5 RID: 10933
		public abstract AlertReport GetReport();

		// Token: 0x06002AB6 RID: 10934 RVA: 0x001698CC File Offset: 0x00167CCC
		public virtual string GetExplanation()
		{
			return this.defaultExplanation;
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x001698E8 File Offset: 0x00167CE8
		public virtual string GetLabel()
		{
			return this.defaultLabel;
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x00169904 File Offset: 0x00167D04
		public void Notify_Started()
		{
			if (this.Priority >= AlertPriority.High)
			{
				if (this.alertBounce == null)
				{
					this.alertBounce = new AlertBounce();
				}
				this.alertBounce.DoAlertStartEffect();
				if (Time.timeSinceLevelLoad > 1f && Time.realtimeSinceStartup > this.lastBellTime + 0.5f)
				{
					SoundDefOf.TinyBell.PlayOneShotOnCamera(null);
					this.lastBellTime = Time.realtimeSinceStartup;
				}
			}
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x0016997E File Offset: 0x00167D7E
		public virtual void AlertActiveUpdate()
		{
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x00169984 File Offset: 0x00167D84
		public virtual Rect DrawAt(float topY, bool minimized)
		{
			Text.Font = GameFont.Small;
			string label = this.GetLabel();
			float height = Text.CalcHeight(label, 148f);
			Rect rect = new Rect((float)UI.screenWidth - 154f, topY, 154f, height);
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
				IEnumerable<GlobalTargetInfo> culprits = this.GetReport().culprits;
				if (culprits != null)
				{
					Alert.tmpTargets.Clear();
					foreach (GlobalTargetInfo item in culprits)
					{
						if (item.IsValid)
						{
							Alert.tmpTargets.Add(item);
						}
					}
					if (Alert.tmpTargets.Any<GlobalTargetInfo>())
					{
						if (Event.current.button == 1)
						{
							this.jumpToTargetCycleIndex--;
						}
						else
						{
							this.jumpToTargetCycleIndex++;
						}
						GlobalTargetInfo target = Alert.tmpTargets[GenMath.PositiveMod(this.jumpToTargetCycleIndex, Alert.tmpTargets.Count)];
						CameraJumper.TryJumpAndSelect(target);
						Alert.tmpTargets.Clear();
					}
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			return rect;
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x00169B64 File Offset: 0x00167F64
		public void DrawInfoPane()
		{
			Alert.<DrawInfoPane>c__AnonStorey0 <DrawInfoPane>c__AnonStorey = new Alert.<DrawInfoPane>c__AnonStorey0();
			if (Event.current.type == EventType.Repaint)
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
				<DrawInfoPane>c__AnonStorey.expString = this.GetExplanation();
				if (this.GetReport().AnyCulpritValid)
				{
					<DrawInfoPane>c__AnonStorey.expString = <DrawInfoPane>c__AnonStorey.expString + "\n\n(" + "ClickToJumpToProblem".Translate() + ")";
				}
				float num = Text.CalcHeight(<DrawInfoPane>c__AnonStorey.expString, 310f);
				num += 20f;
				<DrawInfoPane>c__AnonStorey.infoRect = new Rect((float)UI.screenWidth - 154f - 330f - 8f, Mathf.Max(Mathf.Min(Event.current.mousePosition.y, (float)UI.screenHeight - num), 0f), 330f, num);
				if (<DrawInfoPane>c__AnonStorey.infoRect.yMax > (float)UI.screenHeight)
				{
					Alert.<DrawInfoPane>c__AnonStorey0 <DrawInfoPane>c__AnonStorey2 = <DrawInfoPane>c__AnonStorey;
					<DrawInfoPane>c__AnonStorey2.infoRect.y = <DrawInfoPane>c__AnonStorey2.infoRect.y - ((float)UI.screenHeight - <DrawInfoPane>c__AnonStorey.infoRect.yMax);
				}
				if (<DrawInfoPane>c__AnonStorey.infoRect.y < 0f)
				{
					<DrawInfoPane>c__AnonStorey.infoRect.y = 0f;
				}
				Find.WindowStack.ImmediateWindow(138956, <DrawInfoPane>c__AnonStorey.infoRect, WindowLayer.GameUI, delegate
				{
					Text.Font = GameFont.Small;
					Rect rect = <DrawInfoPane>c__AnonStorey.infoRect.AtZero();
					Widgets.DrawWindowBackground(rect);
					Rect position = rect.ContractedBy(10f);
					GUI.BeginGroup(position);
					Widgets.Label(new Rect(0f, 0f, position.width, position.height), <DrawInfoPane>c__AnonStorey.expString);
					GUI.EndGroup();
				}, false, false, 1f);
			}
		}
	}
}
