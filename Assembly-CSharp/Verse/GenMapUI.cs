using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE1 RID: 3297
	[StaticConstructorOnStartup]
	public static class GenMapUI
	{
		// Token: 0x04003124 RID: 12580
		public static readonly Texture2D OverlayHealthTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0f, 0f, 0.25f));

		// Token: 0x04003125 RID: 12581
		public const float NameBGHeight_Tiny = 12f;

		// Token: 0x04003126 RID: 12582
		public const float NameBGExtraWidth_Tiny = 4f;

		// Token: 0x04003127 RID: 12583
		public const float NameBGHeight_Small = 16f;

		// Token: 0x04003128 RID: 12584
		public const float NameBGExtraWidth_Small = 6f;

		// Token: 0x04003129 RID: 12585
		public const float LabelOffsetYStandard = -0.4f;

		// Token: 0x0400312A RID: 12586
		public static readonly Color DefaultThingLabelColor = new Color(1f, 1f, 1f, 0.75f);

		// Token: 0x060048A1 RID: 18593 RVA: 0x002623F4 File Offset: 0x002607F4
		public static Vector2 LabelDrawPosFor(Thing thing, float worldOffsetZ)
		{
			Vector3 drawPos = thing.DrawPos;
			drawPos.z += worldOffsetZ;
			Vector2 result = Find.Camera.WorldToScreenPoint(drawPos) / Prefs.UIScale;
			result.y = (float)UI.screenHeight - result.y;
			return result;
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x00262450 File Offset: 0x00260850
		public static Vector2 LabelDrawPosFor(IntVec3 center)
		{
			Vector3 position = center.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Vector2 result = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
			result.y = (float)UI.screenHeight - result.y;
			result.y -= 1f;
			return result;
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x002624B3 File Offset: 0x002608B3
		public static void DrawThingLabel(Thing thing, string text)
		{
			GenMapUI.DrawThingLabel(thing, text, GenMapUI.DefaultThingLabelColor);
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x002624C2 File Offset: 0x002608C2
		public static void DrawThingLabel(Thing thing, string text, Color textColor)
		{
			GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(thing, -0.4f), text, textColor);
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x002624D8 File Offset: 0x002608D8
		public static void DrawThingLabel(Vector2 screenPos, string text, Color textColor)
		{
			Text.Font = GameFont.Tiny;
			float x = Text.CalcSize(text).x;
			Rect position = new Rect(screenPos.x - x / 2f - 4f, screenPos.y, x + 8f, 12f);
			GUI.DrawTexture(position, TexUI.GrayTextBG);
			GUI.color = textColor;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect = new Rect(screenPos.x - x / 2f, screenPos.y - 3f, x, 999f);
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x00262588 File Offset: 0x00260988
		public static void DrawPawnLabel(Pawn pawn, Vector2 pos, float alpha = 1f, float truncateToWidth = 9999f, Dictionary<string, string> truncatedLabelsCache = null, GameFont font = GameFont.Tiny, bool alwaysDrawBg = true, bool alignCenter = true)
		{
			float pawnLabelNameWidth = GenMapUI.GetPawnLabelNameWidth(pawn, truncateToWidth, truncatedLabelsCache, font);
			Rect bgRect = new Rect(pos.x - pawnLabelNameWidth / 2f - 4f, pos.y, pawnLabelNameWidth + 8f, 12f);
			if (!pawn.RaceProps.Humanlike)
			{
				bgRect.y -= 4f;
			}
			GenMapUI.DrawPawnLabel(pawn, bgRect, alpha, truncateToWidth, truncatedLabelsCache, font, alwaysDrawBg, alignCenter);
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x00262608 File Offset: 0x00260A08
		public static void DrawPawnLabel(Pawn pawn, Rect bgRect, float alpha = 1f, float truncateToWidth = 9999f, Dictionary<string, string> truncatedLabelsCache = null, GameFont font = GameFont.Tiny, bool alwaysDrawBg = true, bool alignCenter = true)
		{
			GUI.color = new Color(1f, 1f, 1f, alpha);
			Text.Font = font;
			string pawnLabel = GenMapUI.GetPawnLabel(pawn, truncateToWidth, truncatedLabelsCache, font);
			float pawnLabelNameWidth = GenMapUI.GetPawnLabelNameWidth(pawn, truncateToWidth, truncatedLabelsCache, font);
			float summaryHealthPercent = pawn.health.summaryHealth.SummaryHealthPercent;
			if (alwaysDrawBg || summaryHealthPercent < 0.999f)
			{
				GUI.DrawTexture(bgRect, TexUI.GrayTextBG);
			}
			if (summaryHealthPercent < 0.999f)
			{
				Rect rect = bgRect.ContractedBy(1f);
				Widgets.FillableBar(rect, summaryHealthPercent, GenMapUI.OverlayHealthTex, BaseContent.ClearTex, false);
			}
			Color color = PawnNameColorUtility.PawnNameColorOf(pawn);
			color.a = alpha;
			GUI.color = color;
			Rect rect2;
			if (alignCenter)
			{
				Text.Anchor = TextAnchor.UpperCenter;
				rect2 = new Rect(bgRect.center.x - pawnLabelNameWidth / 2f, bgRect.y - 2f, pawnLabelNameWidth, 100f);
			}
			else
			{
				Text.Anchor = TextAnchor.UpperLeft;
				rect2 = new Rect(bgRect.x + 2f, bgRect.center.y - Text.CalcSize(pawnLabel).y / 2f, pawnLabelNameWidth, 100f);
			}
			Widgets.Label(rect2, pawnLabel);
			if (pawn.Drafted)
			{
				Widgets.DrawLineHorizontal(bgRect.center.x - pawnLabelNameWidth / 2f, bgRect.y + 11f, pawnLabelNameWidth);
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x0026279C File Offset: 0x00260B9C
		public static void DrawText(Vector2 worldPos, string text, Color textColor)
		{
			Vector3 position = new Vector3(worldPos.x, 0f, worldPos.y);
			Vector2 vector = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			Text.Font = GameFont.Tiny;
			GUI.color = textColor;
			Text.Anchor = TextAnchor.UpperCenter;
			float x = Text.CalcSize(text).x;
			Widgets.Label(new Rect(vector.x - x / 2f, vector.y - 2f, x, 999f), text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x00262854 File Offset: 0x00260C54
		private static float GetPawnLabelNameWidth(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			string pawnLabel = GenMapUI.GetPawnLabel(pawn, truncateToWidth, truncatedLabelsCache, font);
			float num;
			if (font == GameFont.Tiny)
			{
				num = pawnLabel.GetWidthCached();
			}
			else
			{
				num = Text.CalcSize(pawnLabel).x;
			}
			if (num < 20f)
			{
				num = 20f;
			}
			Text.Font = font2;
			return num;
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x002628BC File Offset: 0x00260CBC
		private static string GetPawnLabel(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			string result = pawn.LabelShort.CapitalizeFirst().Truncate(truncateToWidth, truncatedLabelsCache);
			Text.Font = font2;
			return result;
		}
	}
}
