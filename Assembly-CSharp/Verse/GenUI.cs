using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9B RID: 3739
	[StaticConstructorOnStartup]
	public static class GenUI
	{
		// Token: 0x04003A5A RID: 14938
		public const float Pad = 10f;

		// Token: 0x04003A5B RID: 14939
		public const float GapTiny = 4f;

		// Token: 0x04003A5C RID: 14940
		public const float GapSmall = 10f;

		// Token: 0x04003A5D RID: 14941
		public const float Gap = 17f;

		// Token: 0x04003A5E RID: 14942
		public const float GapWide = 26f;

		// Token: 0x04003A5F RID: 14943
		public const float ListSpacing = 28f;

		// Token: 0x04003A60 RID: 14944
		public const float MouseAttachIconSize = 32f;

		// Token: 0x04003A61 RID: 14945
		public const float MouseAttachIconOffset = 8f;

		// Token: 0x04003A62 RID: 14946
		public const float ScrollBarWidth = 16f;

		// Token: 0x04003A63 RID: 14947
		public const float HorizontalSliderHeight = 16f;

		// Token: 0x04003A64 RID: 14948
		public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

		// Token: 0x04003A65 RID: 14949
		public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

		// Token: 0x04003A66 RID: 14950
		public static readonly Color SubtleMouseoverColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x04003A67 RID: 14951
		public static readonly Vector2 MaxWinSize = new Vector2(1010f, 754f);

		// Token: 0x04003A68 RID: 14952
		public const float SmallIconSize = 24f;

		// Token: 0x04003A69 RID: 14953
		public const int RootGUIDepth = 50;

		// Token: 0x04003A6A RID: 14954
		public const int CameraGUIDepth = 100;

		// Token: 0x04003A6B RID: 14955
		private const float MouseIconSize = 32f;

		// Token: 0x04003A6C RID: 14956
		private const float MouseIconOffset = 12f;

		// Token: 0x04003A6D RID: 14957
		private static readonly Material MouseoverBracketMaterial = MaterialPool.MatFrom("UI/Overlays/MouseoverBracketTex", ShaderDatabase.MetaOverlay);

		// Token: 0x04003A6E RID: 14958
		private static readonly Texture2D UnderShadowTex = ContentFinder<Texture2D>.Get("UI/Misc/ScreenCornerShadow", true);

		// Token: 0x04003A6F RID: 14959
		private static readonly Texture2D UIFlash = ContentFinder<Texture2D>.Get("UI/Misc/Flash", true);

		// Token: 0x04003A70 RID: 14960
		private static Dictionary<string, float> labelWidthCache = new Dictionary<string, float>();

		// Token: 0x04003A71 RID: 14961
		private static readonly Vector2 PieceBarSize = new Vector2(100f, 17f);

		// Token: 0x04003A72 RID: 14962
		public const float PawnDirectClickRadius = 0.4f;

		// Token: 0x04003A73 RID: 14963
		private static List<Pawn> clickedPawns = new List<Pawn>();

		// Token: 0x04003A74 RID: 14964
		[CompilerGenerated]
		private static Comparison<Pawn> <>f__mg$cache0;

		// Token: 0x04003A75 RID: 14965
		[CompilerGenerated]
		private static Comparison<Thing> <>f__mg$cache1;

		// Token: 0x04003A76 RID: 14966
		[CompilerGenerated]
		private static Comparison<Pawn> <>f__mg$cache2;

		// Token: 0x06005833 RID: 22579 RVA: 0x002D39A2 File Offset: 0x002D1DA2
		public static void SetLabelAlign(TextAnchor a)
		{
			Text.Anchor = a;
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x002D39AB File Offset: 0x002D1DAB
		public static void ResetLabelAlign()
		{
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x002D39B4 File Offset: 0x002D1DB4
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

		// Token: 0x06005836 RID: 22582 RVA: 0x002D3A2C File Offset: 0x002D1E2C
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

		// Token: 0x06005837 RID: 22583 RVA: 0x002D3A7C File Offset: 0x002D1E7C
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

		// Token: 0x06005838 RID: 22584 RVA: 0x002D3B4C File Offset: 0x002D1F4C
		public static void ErrorDialog(string message)
		{
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox(message, null, null, null, null, null, false, null, null));
			}
		}

		// Token: 0x06005839 RID: 22585 RVA: 0x002D3B7C File Offset: 0x002D1F7C
		public static void DrawFlash(float centerX, float centerY, float size, float alpha, Color color)
		{
			Rect position = new Rect(centerX - size / 2f, centerY - size / 2f, size, size);
			Color color2 = color;
			color2.a = alpha;
			GUI.color = color2;
			GUI.DrawTexture(position, GenUI.UIFlash);
			GUI.color = Color.white;
		}

		// Token: 0x0600583A RID: 22586 RVA: 0x002D3BCC File Offset: 0x002D1FCC
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

		// Token: 0x0600583B RID: 22587 RVA: 0x002D3C58 File Offset: 0x002D2058
		public static Rect Rounded(this Rect r)
		{
			return new Rect((float)((int)r.x), (float)((int)r.y), (float)((int)r.width), (float)((int)r.height));
		}

		// Token: 0x0600583C RID: 22588 RVA: 0x002D3C98 File Offset: 0x002D2098
		public static Vector2 Rounded(this Vector2 v)
		{
			return new Vector2((float)((int)v.x), (float)((int)v.y));
		}

		// Token: 0x0600583D RID: 22589 RVA: 0x002D3CC4 File Offset: 0x002D20C4
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

		// Token: 0x0600583E RID: 22590 RVA: 0x002D3D60 File Offset: 0x002D2160
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

		// Token: 0x0600583F RID: 22591 RVA: 0x002D3EC0 File Offset: 0x002D22C0
		public static void DrawMouseAttachment(Texture2D icon)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect mouseRect = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
			{
				GUI.DrawTexture(mouseRect.AtZero(), icon);
			}, false, false, 0f);
		}

		// Token: 0x06005840 RID: 22592 RVA: 0x002D3F40 File Offset: 0x002D2340
		public static void RenderMouseoverBracket()
		{
			Vector3 position = UI.MouseCell().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenUI.MouseoverBracketMaterial, 0);
		}

		// Token: 0x06005841 RID: 22593 RVA: 0x002D3F74 File Offset: 0x002D2374
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

		// Token: 0x06005842 RID: 22594 RVA: 0x002D4024 File Offset: 0x002D2424
		public static IEnumerable<LocalTargetInfo> TargetsAtMouse(TargetingParameters clickParams, bool thingsOnly = false)
		{
			return GenUI.TargetsAt(UI.MouseMapPosition(), clickParams, thingsOnly);
		}

		// Token: 0x06005843 RID: 22595 RVA: 0x002D4048 File Offset: 0x002D2448
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

		// Token: 0x06005844 RID: 22596 RVA: 0x002D4080 File Offset: 0x002D2480
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

		// Token: 0x06005845 RID: 22597 RVA: 0x002D4348 File Offset: 0x002D2748
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

		// Token: 0x06005846 RID: 22598 RVA: 0x002D43A4 File Offset: 0x002D27A4
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

		// Token: 0x06005847 RID: 22599 RVA: 0x002D4400 File Offset: 0x002D2800
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

		// Token: 0x06005848 RID: 22600 RVA: 0x002D4470 File Offset: 0x002D2870
		public static Rect GetInnerRect(this Rect rect)
		{
			return rect.ContractedBy(17f);
		}

		// Token: 0x06005849 RID: 22601 RVA: 0x002D4490 File Offset: 0x002D2890
		public static Rect ExpandedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x - margin, rect.y - margin, rect.width + margin * 2f, rect.height + margin * 2f);
		}

		// Token: 0x0600584A RID: 22602 RVA: 0x002D44DC File Offset: 0x002D28DC
		public static Rect ContractedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2f, rect.height - margin * 2f);
		}

		// Token: 0x0600584B RID: 22603 RVA: 0x002D4528 File Offset: 0x002D2928
		public static Rect ScaledBy(this Rect rect, float scale)
		{
			rect.x -= rect.width * (scale - 1f) / 2f;
			rect.y -= rect.height * (scale - 1f) / 2f;
			rect.width *= scale;
			rect.height *= scale;
			return rect;
		}

		// Token: 0x0600584C RID: 22604 RVA: 0x002D45A4 File Offset: 0x002D29A4
		public static Rect CenteredOnXIn(this Rect rect, Rect otherRect)
		{
			return new Rect(otherRect.x + (otherRect.width - rect.width) / 2f, rect.y, rect.width, rect.height);
		}

		// Token: 0x0600584D RID: 22605 RVA: 0x002D45F0 File Offset: 0x002D29F0
		public static Rect CenteredOnYIn(this Rect rect, Rect otherRect)
		{
			return new Rect(rect.x, otherRect.y + (otherRect.height - rect.height) / 2f, rect.width, rect.height);
		}

		// Token: 0x0600584E RID: 22606 RVA: 0x002D463C File Offset: 0x002D2A3C
		public static Rect AtZero(this Rect rect)
		{
			return new Rect(0f, 0f, rect.width, rect.height);
		}

		// Token: 0x0600584F RID: 22607 RVA: 0x002D466E File Offset: 0x002D2A6E
		public static void AbsorbClicksInRect(Rect r)
		{
			if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06005850 RID: 22608 RVA: 0x002D46A0 File Offset: 0x002D2AA0
		public static Rect LeftHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06005851 RID: 22609 RVA: 0x002D46DC File Offset: 0x002D2ADC
		public static Rect LeftPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06005852 RID: 22610 RVA: 0x002D4714 File Offset: 0x002D2B14
		public static Rect LeftPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x, rect.y, width, rect.height);
		}

		// Token: 0x06005853 RID: 22611 RVA: 0x002D4744 File Offset: 0x002D2B44
		public static Rect RightHalf(this Rect rect)
		{
			return new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06005854 RID: 22612 RVA: 0x002D4790 File Offset: 0x002D2B90
		public static Rect RightPart(this Rect rect, float pct)
		{
			return new Rect(rect.x + rect.width * (1f - pct), rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06005855 RID: 22613 RVA: 0x002D47D8 File Offset: 0x002D2BD8
		public static Rect RightPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
		}

		// Token: 0x06005856 RID: 22614 RVA: 0x002D4814 File Offset: 0x002D2C14
		public static Rect TopHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height / 2f);
		}

		// Token: 0x06005857 RID: 22615 RVA: 0x002D4850 File Offset: 0x002D2C50
		public static Rect TopPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height * pct);
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x002D4888 File Offset: 0x002D2C88
		public static Rect TopPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y, rect.width, height);
		}

		// Token: 0x06005859 RID: 22617 RVA: 0x002D48B8 File Offset: 0x002D2CB8
		public static Rect BottomHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x002D4904 File Offset: 0x002D2D04
		public static Rect BottomPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y + rect.height * (1f - pct), rect.width, rect.height * pct);
		}

		// Token: 0x0600585B RID: 22619 RVA: 0x002D494C File Offset: 0x002D2D4C
		public static Rect BottomPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
		}

		// Token: 0x0600585C RID: 22620 RVA: 0x002D4988 File Offset: 0x002D2D88
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

		// Token: 0x0600585D RID: 22621 RVA: 0x002D4A64 File Offset: 0x002D2E64
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

		// Token: 0x0600585E RID: 22622 RVA: 0x002D4B38 File Offset: 0x002D2F38
		public static float GetCenteredButtonPos(int buttonIndex, int buttonsCount, float totalWidth, float buttonWidth, float pad = 10f)
		{
			float num = (float)buttonsCount * buttonWidth + (float)(buttonsCount - 1) * pad;
			return Mathf.Floor((totalWidth - num) / 2f + (float)buttonIndex * (buttonWidth + pad));
		}
	}
}
