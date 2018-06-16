using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9A RID: 3738
	[StaticConstructorOnStartup]
	public static class GenUI
	{
		// Token: 0x06005811 RID: 22545 RVA: 0x002D1A7A File Offset: 0x002CFE7A
		public static void SetLabelAlign(TextAnchor a)
		{
			Text.Anchor = a;
		}

		// Token: 0x06005812 RID: 22546 RVA: 0x002D1A83 File Offset: 0x002CFE83
		public static void ResetLabelAlign()
		{
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06005813 RID: 22547 RVA: 0x002D1A8C File Offset: 0x002CFE8C
		public static float BackgroundDarkAlphaForText()
		{
			float result;
			if (Find.CurrentMap == null)
			{
				result = 0f;
			}
			else
			{
				float num = GenCelestial.CurCelestialSunGlow(Find.CurrentMap);
				float num2 = (Find.CurrentMap.Biome != BiomeDefOf.IceSheet) ? Mathf.Clamp01(Find.CurrentMap.snowGrid.TotalDepth / 1000f) : 1f;
				result = num * num2 * 0.41f;
			}
			return result;
		}

		// Token: 0x06005814 RID: 22548 RVA: 0x002D1B04 File Offset: 0x002CFF04
		public static void DrawTextWinterShadow(Rect rect)
		{
			float num = GenUI.BackgroundDarkAlphaForText();
			if (num > 0.001f)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(rect, GenUI.UnderShadowTex);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06005815 RID: 22549 RVA: 0x002D1B54 File Offset: 0x002CFF54
		public static float IconDrawScale(ThingDef tDef)
		{
			float num = tDef.uiIconScale;
			if (tDef.uiIconPath.NullOrEmpty())
			{
				if (tDef.graphicData != null && tDef.graphicData.drawSize.x > (float)tDef.Size.x && tDef.graphicData.drawSize.y > (float)tDef.Size.z)
				{
					num *= Mathf.Min(tDef.graphicData.drawSize.x / (float)tDef.Size.x, tDef.graphicData.drawSize.y / (float)tDef.Size.z);
				}
			}
			return num;
		}

		// Token: 0x06005816 RID: 22550 RVA: 0x002D1C24 File Offset: 0x002D0024
		public static void ErrorDialog(string message)
		{
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox(message, null, null, null, null, null, false, null, null));
			}
		}

		// Token: 0x06005817 RID: 22551 RVA: 0x002D1C54 File Offset: 0x002D0054
		public static void DrawFlash(float centerX, float centerY, float size, float alpha, Color color)
		{
			Rect position = new Rect(centerX - size / 2f, centerY - size / 2f, size, size);
			Color color2 = color;
			color2.a = alpha;
			GUI.color = color2;
			GUI.DrawTexture(position, GenUI.UIFlash);
			GUI.color = Color.white;
		}

		// Token: 0x06005818 RID: 22552 RVA: 0x002D1CA4 File Offset: 0x002D00A4
		public static float GetWidthCached(this string s)
		{
			if (GenUI.labelWidthCache.Count > 2000 || (Time.frameCount % 40000 == 0 && GenUI.labelWidthCache.Count > 100))
			{
				GenUI.labelWidthCache.Clear();
			}
			float x;
			float result;
			if (GenUI.labelWidthCache.TryGetValue(s, out x))
			{
				result = x;
			}
			else
			{
				x = Text.CalcSize(s).x;
				GenUI.labelWidthCache.Add(s, x);
				result = x;
			}
			return result;
		}

		// Token: 0x06005819 RID: 22553 RVA: 0x002D1D30 File Offset: 0x002D0130
		public static Rect Rounded(this Rect r)
		{
			return new Rect((float)((int)r.x), (float)((int)r.y), (float)((int)r.width), (float)((int)r.height));
		}

		// Token: 0x0600581A RID: 22554 RVA: 0x002D1D70 File Offset: 0x002D0170
		public static Vector2 Rounded(this Vector2 v)
		{
			return new Vector2((float)((int)v.x), (float)((int)v.y));
		}

		// Token: 0x0600581B RID: 22555 RVA: 0x002D1D9C File Offset: 0x002D019C
		public static float DistFromRect(Rect r, Vector2 p)
		{
			float num = Mathf.Abs(p.x - r.center.x) - r.width / 2f;
			if (num < 0f)
			{
				num = 0f;
			}
			float num2 = Mathf.Abs(p.y - r.center.y) - r.height / 2f;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x0600581C RID: 22556 RVA: 0x002D1E38 File Offset: 0x002D0238
		public static void DrawMouseAttachment(Texture iconTex, string text = "", float angle = 0f, Vector2 offset = default(Vector2), Rect? customRect = null)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float num = mousePosition.y + 12f;
			if (iconTex != null)
			{
				Rect mouseRect;
				if (customRect != null)
				{
					mouseRect = customRect.Value;
				}
				else
				{
					mouseRect = new Rect(mousePosition.x + 8f, num + 8f, 32f, 32f);
				}
				Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
				{
					Rect rect = mouseRect.AtZero();
					rect.position += new Vector2(offset.x * rect.size.x, offset.y * rect.size.y);
					Widgets.DrawTextureRotated(rect, iconTex, angle);
				}, false, false, 0f);
				num += mouseRect.height;
			}
			if (text != "")
			{
				Rect textRect = new Rect(mousePosition.x + 12f, num, 200f, 9999f);
				Find.WindowStack.ImmediateWindow(34003429, textRect, WindowLayer.Super, delegate
				{
					Widgets.Label(textRect.AtZero(), text);
				}, false, false, 0f);
			}
		}

		// Token: 0x0600581D RID: 22557 RVA: 0x002D1F98 File Offset: 0x002D0398
		public static void DrawMouseAttachment(Texture2D icon)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect mouseRect = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
			{
				GUI.DrawTexture(mouseRect.AtZero(), icon);
			}, false, false, 0f);
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x002D2018 File Offset: 0x002D0418
		public static void RenderMouseoverBracket()
		{
			Vector3 position = UI.MouseCell().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenUI.MouseoverBracketMaterial, 0);
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x002D204C File Offset: 0x002D044C
		public static void DrawStatusLevel(Need status, Rect rect)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 2f, rect.width, 25f);
			Widgets.Label(rect2, status.LabelCap);
			Rect rect3 = new Rect(100f, 3f, GenUI.PieceBarSize.x, GenUI.PieceBarSize.y);
			Widgets.FillableBar(rect3, status.CurLevelPercentage);
			Widgets.FillableBarChangeArrows(rect3, status.GUIChangeArrow);
			GUI.EndGroup();
			TooltipHandler.TipRegion(rect, status.GetTipString());
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
		}

		// Token: 0x06005820 RID: 22560 RVA: 0x002D20FC File Offset: 0x002D04FC
		public static IEnumerable<LocalTargetInfo> TargetsAtMouse(TargetingParameters clickParams, bool thingsOnly = false)
		{
			return GenUI.TargetsAt(UI.MouseMapPosition(), clickParams, thingsOnly);
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x002D2120 File Offset: 0x002D0520
		public static IEnumerable<LocalTargetInfo> TargetsAt(Vector3 clickPos, TargetingParameters clickParams, bool thingsOnly = false)
		{
			List<Thing> clickableList = GenUI.ThingsUnderMouse(clickPos, 0.8f, clickParams);
			for (int i = 0; i < clickableList.Count; i++)
			{
				yield return clickableList[i];
			}
			if (!thingsOnly)
			{
				IntVec3 cellTarg = UI.MouseCell();
				if (cellTarg.InBounds(Find.CurrentMap) && clickParams.CanTarget(new TargetInfo(cellTarg, Find.CurrentMap, false)))
				{
					yield return cellTarg;
				}
			}
			yield break;
		}

		// Token: 0x06005822 RID: 22562 RVA: 0x002D2158 File Offset: 0x002D0558
		public static List<Thing> ThingsUnderMouse(Vector3 clickPos, float pawnWideClickRadius, TargetingParameters clickParams)
		{
			IntVec3 c = IntVec3.FromVector3(clickPos);
			List<Thing> list = new List<Thing>();
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((pawn.DrawPos - clickPos).MagnitudeHorizontal() < 0.4f && clickParams.CanTarget(pawn))
				{
					GenUI.clickedPawns.Add(pawn);
				}
			}
			List<Pawn> list2 = GenUI.clickedPawns;
			if (GenUI.<>f__mg$cache0 == null)
			{
				GenUI.<>f__mg$cache0 = new Comparison<Pawn>(GenUI.CompareThingsByDistanceToMousePointer);
			}
			list2.Sort(GenUI.<>f__mg$cache0);
			for (int j = 0; j < GenUI.clickedPawns.Count; j++)
			{
				list.Add(GenUI.clickedPawns[j]);
			}
			List<Thing> list3 = new List<Thing>();
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(c))
			{
				if (!list.Contains(thing) && clickParams.CanTarget(thing))
				{
					list3.Add(thing);
				}
			}
			List<Thing> list4 = list3;
			if (GenUI.<>f__mg$cache1 == null)
			{
				GenUI.<>f__mg$cache1 = new Comparison<Thing>(GenUI.CompareThingsByDrawAltitude);
			}
			list4.Sort(GenUI.<>f__mg$cache1);
			list.AddRange(list3);
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned2 = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int k = 0; k < allPawnsSpawned2.Count; k++)
			{
				Pawn pawn2 = allPawnsSpawned2[k];
				if ((pawn2.DrawPos - clickPos).MagnitudeHorizontal() < pawnWideClickRadius && clickParams.CanTarget(pawn2))
				{
					GenUI.clickedPawns.Add(pawn2);
				}
			}
			List<Pawn> list5 = GenUI.clickedPawns;
			if (GenUI.<>f__mg$cache2 == null)
			{
				GenUI.<>f__mg$cache2 = new Comparison<Pawn>(GenUI.CompareThingsByDistanceToMousePointer);
			}
			list5.Sort(GenUI.<>f__mg$cache2);
			for (int l = 0; l < GenUI.clickedPawns.Count; l++)
			{
				if (!list.Contains(GenUI.clickedPawns[l]))
				{
					list.Add(GenUI.clickedPawns[l]);
				}
			}
			list.RemoveAll((Thing t) => !t.Spawned);
			GenUI.clickedPawns.Clear();
			return list;
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x002D2420 File Offset: 0x002D0820
		private static int CompareThingsByDistanceToMousePointer(Thing a, Thing b)
		{
			Vector3 b2 = UI.MouseMapPosition();
			float num = (a.DrawPos - b2).MagnitudeHorizontalSquared();
			float num2 = (b.DrawPos - b2).MagnitudeHorizontalSquared();
			int result;
			if (num < num2)
			{
				result = -1;
			}
			else if (num == num2)
			{
				result = 0;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		// Token: 0x06005824 RID: 22564 RVA: 0x002D247C File Offset: 0x002D087C
		private static int CompareThingsByDrawAltitude(Thing A, Thing B)
		{
			int result;
			if (A.def.Altitude < B.def.Altitude)
			{
				result = 1;
			}
			else if (A.def.Altitude == B.def.Altitude)
			{
				result = 0;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x06005825 RID: 22565 RVA: 0x002D24D8 File Offset: 0x002D08D8
		public static int CurrentAdjustmentMultiplier()
		{
			int result;
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent && KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				result = 1000;
			}
			else if (KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				result = 100;
			}
			else if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent)
			{
				result = 10;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		// Token: 0x06005826 RID: 22566 RVA: 0x002D2548 File Offset: 0x002D0948
		public static Rect GetInnerRect(this Rect rect)
		{
			return rect.ContractedBy(17f);
		}

		// Token: 0x06005827 RID: 22567 RVA: 0x002D2568 File Offset: 0x002D0968
		public static Rect ExpandedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x - margin, rect.y - margin, rect.width + margin * 2f, rect.height + margin * 2f);
		}

		// Token: 0x06005828 RID: 22568 RVA: 0x002D25B4 File Offset: 0x002D09B4
		public static Rect ContractedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2f, rect.height - margin * 2f);
		}

		// Token: 0x06005829 RID: 22569 RVA: 0x002D2600 File Offset: 0x002D0A00
		public static Rect ScaledBy(this Rect rect, float scale)
		{
			rect.x -= rect.width * (scale - 1f) / 2f;
			rect.y -= rect.height * (scale - 1f) / 2f;
			rect.width *= scale;
			rect.height *= scale;
			return rect;
		}

		// Token: 0x0600582A RID: 22570 RVA: 0x002D267C File Offset: 0x002D0A7C
		public static Rect CenteredOnXIn(this Rect rect, Rect otherRect)
		{
			return new Rect(otherRect.x + (otherRect.width - rect.width) / 2f, rect.y, rect.width, rect.height);
		}

		// Token: 0x0600582B RID: 22571 RVA: 0x002D26C8 File Offset: 0x002D0AC8
		public static Rect CenteredOnYIn(this Rect rect, Rect otherRect)
		{
			return new Rect(rect.x, otherRect.y + (otherRect.height - rect.height) / 2f, rect.width, rect.height);
		}

		// Token: 0x0600582C RID: 22572 RVA: 0x002D2714 File Offset: 0x002D0B14
		public static Rect AtZero(this Rect rect)
		{
			return new Rect(0f, 0f, rect.width, rect.height);
		}

		// Token: 0x0600582D RID: 22573 RVA: 0x002D2746 File Offset: 0x002D0B46
		public static void AbsorbClicksInRect(Rect r)
		{
			if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
			}
		}

		// Token: 0x0600582E RID: 22574 RVA: 0x002D2778 File Offset: 0x002D0B78
		public static Rect LeftHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x0600582F RID: 22575 RVA: 0x002D27B4 File Offset: 0x002D0BB4
		public static Rect LeftPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x002D27EC File Offset: 0x002D0BEC
		public static Rect LeftPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x, rect.y, width, rect.height);
		}

		// Token: 0x06005831 RID: 22577 RVA: 0x002D281C File Offset: 0x002D0C1C
		public static Rect RightHalf(this Rect rect)
		{
			return new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06005832 RID: 22578 RVA: 0x002D2868 File Offset: 0x002D0C68
		public static Rect RightPart(this Rect rect, float pct)
		{
			return new Rect(rect.x + rect.width * (1f - pct), rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x002D28B0 File Offset: 0x002D0CB0
		public static Rect RightPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x002D28EC File Offset: 0x002D0CEC
		public static Rect TopHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height / 2f);
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x002D2928 File Offset: 0x002D0D28
		public static Rect TopPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height * pct);
		}

		// Token: 0x06005836 RID: 22582 RVA: 0x002D2960 File Offset: 0x002D0D60
		public static Rect TopPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y, rect.width, height);
		}

		// Token: 0x06005837 RID: 22583 RVA: 0x002D2990 File Offset: 0x002D0D90
		public static Rect BottomHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
		}

		// Token: 0x06005838 RID: 22584 RVA: 0x002D29DC File Offset: 0x002D0DDC
		public static Rect BottomPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y + rect.height * (1f - pct), rect.width, rect.height * pct);
		}

		// Token: 0x06005839 RID: 22585 RVA: 0x002D2A24 File Offset: 0x002D0E24
		public static Rect BottomPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
		}

		// Token: 0x0600583A RID: 22586 RVA: 0x002D2A60 File Offset: 0x002D0E60
		public static Color LerpColor(List<Pair<float, Color>> colors, float value)
		{
			Color result;
			if (colors.Count == 0)
			{
				result = Color.white;
			}
			else
			{
				int i = 0;
				while (i < colors.Count)
				{
					if (value < colors[i].First)
					{
						if (i == 0)
						{
							return colors[i].Second;
						}
						return Color.Lerp(colors[i - 1].Second, colors[i].Second, Mathf.InverseLerp(colors[i - 1].First, colors[i].First, value));
					}
					else
					{
						i++;
					}
				}
				result = colors.Last<Pair<float, Color>>().Second;
			}
			return result;
		}

		// Token: 0x0600583B RID: 22587 RVA: 0x002D2B3C File Offset: 0x002D0F3C
		public static Vector2 GetMouseAttachedWindowPos(float width, float height)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float y;
			if (mousePosition.y + 14f + height < (float)UI.screenHeight)
			{
				y = mousePosition.y + 14f;
			}
			else if (mousePosition.y - 5f - height >= 0f)
			{
				y = mousePosition.y - 5f - height;
			}
			else
			{
				y = 0f;
			}
			float x;
			if (mousePosition.x + 16f + width < (float)UI.screenWidth)
			{
				x = mousePosition.x + 16f;
			}
			else
			{
				x = mousePosition.x - 4f - width;
			}
			return new Vector2(x, y);
		}

		// Token: 0x0600583C RID: 22588 RVA: 0x002D2C10 File Offset: 0x002D1010
		public static float GetCenteredButtonPos(int buttonIndex, int buttonsCount, float totalWidth, float buttonWidth, float pad = 10f)
		{
			float num = (float)buttonsCount * buttonWidth + (float)(buttonsCount - 1) * pad;
			return Mathf.Floor((totalWidth - num) / 2f + (float)buttonIndex * (buttonWidth + pad));
		}

		// Token: 0x04003A44 RID: 14916
		public const float Pad = 10f;

		// Token: 0x04003A45 RID: 14917
		public const float GapTiny = 4f;

		// Token: 0x04003A46 RID: 14918
		public const float GapSmall = 10f;

		// Token: 0x04003A47 RID: 14919
		public const float Gap = 17f;

		// Token: 0x04003A48 RID: 14920
		public const float GapWide = 26f;

		// Token: 0x04003A49 RID: 14921
		public const float ListSpacing = 28f;

		// Token: 0x04003A4A RID: 14922
		public const float MouseAttachIconSize = 32f;

		// Token: 0x04003A4B RID: 14923
		public const float MouseAttachIconOffset = 8f;

		// Token: 0x04003A4C RID: 14924
		public const float ScrollBarWidth = 16f;

		// Token: 0x04003A4D RID: 14925
		public const float HorizontalSliderHeight = 16f;

		// Token: 0x04003A4E RID: 14926
		public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

		// Token: 0x04003A4F RID: 14927
		public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

		// Token: 0x04003A50 RID: 14928
		public static readonly Color SubtleMouseoverColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x04003A51 RID: 14929
		public static readonly Vector2 MaxWinSize = new Vector2(1010f, 754f);

		// Token: 0x04003A52 RID: 14930
		public const float SmallIconSize = 24f;

		// Token: 0x04003A53 RID: 14931
		public const int RootGUIDepth = 50;

		// Token: 0x04003A54 RID: 14932
		public const int CameraGUIDepth = 100;

		// Token: 0x04003A55 RID: 14933
		private const float MouseIconSize = 32f;

		// Token: 0x04003A56 RID: 14934
		private const float MouseIconOffset = 12f;

		// Token: 0x04003A57 RID: 14935
		private static readonly Material MouseoverBracketMaterial = MaterialPool.MatFrom("UI/Overlays/MouseoverBracketTex", ShaderDatabase.MetaOverlay);

		// Token: 0x04003A58 RID: 14936
		private static readonly Texture2D UnderShadowTex = ContentFinder<Texture2D>.Get("UI/Misc/ScreenCornerShadow", true);

		// Token: 0x04003A59 RID: 14937
		private static readonly Texture2D UIFlash = ContentFinder<Texture2D>.Get("UI/Misc/Flash", true);

		// Token: 0x04003A5A RID: 14938
		private static Dictionary<string, float> labelWidthCache = new Dictionary<string, float>();

		// Token: 0x04003A5B RID: 14939
		private static readonly Vector2 PieceBarSize = new Vector2(100f, 17f);

		// Token: 0x04003A5C RID: 14940
		public const float PawnDirectClickRadius = 0.4f;

		// Token: 0x04003A5D RID: 14941
		private static List<Pawn> clickedPawns = new List<Pawn>();

		// Token: 0x04003A5E RID: 14942
		[CompilerGenerated]
		private static Comparison<Pawn> <>f__mg$cache0;

		// Token: 0x04003A5F RID: 14943
		[CompilerGenerated]
		private static Comparison<Thing> <>f__mg$cache1;

		// Token: 0x04003A60 RID: 14944
		[CompilerGenerated]
		private static Comparison<Pawn> <>f__mg$cache2;
	}
}
