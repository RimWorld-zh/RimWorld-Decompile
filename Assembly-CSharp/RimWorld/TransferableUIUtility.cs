using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200082B RID: 2091
	[StaticConstructorOnStartup]
	public static class TransferableUIUtility
	{
		// Token: 0x06002F01 RID: 12033 RVA: 0x00190F34 File Offset: 0x0018F334
		public static void DoCountAdjustInterface(Rect rect, Transferable trad, int index, int min, int max, bool flash = false, List<TransferableCountToTransferStoppingPoint> extraStoppingPoints = null, bool readOnly = false)
		{
			TransferableUIUtility.stoppingPoints.Clear();
			if (extraStoppingPoints != null)
			{
				TransferableUIUtility.stoppingPoints.AddRange(extraStoppingPoints);
			}
			for (int i = TransferableUIUtility.stoppingPoints.Count - 1; i >= 0; i--)
			{
				if (TransferableUIUtility.stoppingPoints[i].threshold != 0 && (TransferableUIUtility.stoppingPoints[i].threshold <= min || TransferableUIUtility.stoppingPoints[i].threshold >= max))
				{
					TransferableUIUtility.stoppingPoints.RemoveAt(i);
				}
			}
			bool flag = false;
			for (int j = 0; j < TransferableUIUtility.stoppingPoints.Count; j++)
			{
				if (TransferableUIUtility.stoppingPoints[j].threshold == 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TransferableUIUtility.stoppingPoints.Add(new TransferableCountToTransferStoppingPoint(0, "0", "0"));
			}
			TransferableUIUtility.DoCountAdjustInterfaceInternal(rect, trad, index, min, max, flash, readOnly);
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x0019104C File Offset: 0x0018F44C
		private static void DoCountAdjustInterfaceInternal(Rect rect, Transferable trad, int index, int min, int max, bool flash, bool readOnly)
		{
			rect = rect.Rounded();
			Rect rect2 = new Rect(rect.center.x - 45f, rect.center.y - 12.5f, 90f, 25f).Rounded();
			if (flash)
			{
				GUI.DrawTexture(rect2, TransferableUIUtility.FlashTex);
			}
			TransferableOneWay transferableOneWay = trad as TransferableOneWay;
			bool flag = transferableOneWay != null && transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn && transferableOneWay.MaxCount == 1;
			if (!trad.Interactive || readOnly)
			{
				if (flag)
				{
					bool flag2 = trad.CountToTransfer != 0;
					Widgets.Checkbox(rect2.position, ref flag2, 24f, true, false, null, null);
				}
				else
				{
					GUI.color = ((trad.CountToTransfer != 0) ? Color.white : TransferableUIUtility.ZeroCountColor);
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect2, trad.CountToTransfer.ToStringCached());
				}
			}
			else if (flag)
			{
				bool flag3 = trad.CountToTransfer != 0;
				bool flag4 = flag3;
				Widgets.Checkbox(rect2.position, ref flag4, 24f, false, true, null, null);
				if (flag4 != flag3)
				{
					if (flag4)
					{
						trad.AdjustTo(trad.GetMaximumToTransfer());
					}
					else
					{
						trad.AdjustTo(trad.GetMinimumToTransfer());
					}
				}
			}
			else
			{
				Rect rect3 = rect2.ContractedBy(2f);
				rect3.xMax -= 15f;
				rect3.xMin += 16f;
				int countToTransfer = trad.CountToTransfer;
				string editBuffer = trad.EditBuffer;
				Widgets.TextFieldNumeric<int>(rect3, ref countToTransfer, ref editBuffer, (float)min, (float)max);
				trad.AdjustTo(countToTransfer);
				trad.EditBuffer = editBuffer;
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			if (trad.Interactive && !flag)
			{
				TransferablePositiveCountDirection positiveCountDirection = trad.PositiveCountDirection;
				int num = (positiveCountDirection != TransferablePositiveCountDirection.Source) ? -1 : 1;
				int num2 = GenUI.CurrentAdjustmentMultiplier();
				bool flag5 = trad.GetRange() == 1;
				if (trad.CanAdjustBy(num * num2).Accepted)
				{
					Rect rect4 = new Rect(rect2.x - 30f, rect.y, 30f, rect.height);
					if (flag5)
					{
						rect4.x -= rect4.width;
						rect4.width += rect4.width;
					}
					if (Widgets.ButtonText(rect4, "<", true, false, true))
					{
						trad.AdjustBy(num * num2);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					if (!flag5)
					{
						string label = "<<";
						int? num3 = null;
						int num4 = 0;
						for (int i = 0; i < TransferableUIUtility.stoppingPoints.Count; i++)
						{
							TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint = TransferableUIUtility.stoppingPoints[i];
							if (positiveCountDirection == TransferablePositiveCountDirection.Source)
							{
								if (trad.CountToTransfer < transferableCountToTransferStoppingPoint.threshold && (transferableCountToTransferStoppingPoint.threshold < num4 || num3 == null))
								{
									label = transferableCountToTransferStoppingPoint.leftLabel;
									num3 = new int?(transferableCountToTransferStoppingPoint.threshold);
								}
							}
							else if (trad.CountToTransfer > transferableCountToTransferStoppingPoint.threshold && (transferableCountToTransferStoppingPoint.threshold > num4 || num3 == null))
							{
								label = transferableCountToTransferStoppingPoint.leftLabel;
								num3 = new int?(transferableCountToTransferStoppingPoint.threshold);
							}
						}
						rect4.x -= rect4.width;
						if (Widgets.ButtonText(rect4, label, true, false, true))
						{
							if (num3 != null)
							{
								trad.AdjustTo(num3.Value);
							}
							else if (num == 1)
							{
								trad.AdjustTo(trad.GetMaximumToTransfer());
							}
							else
							{
								trad.AdjustTo(trad.GetMinimumToTransfer());
							}
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						}
					}
				}
				if (trad.CanAdjustBy(-num * num2).Accepted)
				{
					Rect rect5 = new Rect(rect2.xMax, rect.y, 30f, rect.height);
					if (flag5)
					{
						rect5.width += rect5.width;
					}
					if (Widgets.ButtonText(rect5, ">", true, false, true))
					{
						trad.AdjustBy(-num * num2);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
					if (!flag5)
					{
						string label2 = ">>";
						int? num5 = null;
						int num6 = 0;
						for (int j = 0; j < TransferableUIUtility.stoppingPoints.Count; j++)
						{
							TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint2 = TransferableUIUtility.stoppingPoints[j];
							if (positiveCountDirection == TransferablePositiveCountDirection.Destination)
							{
								if (trad.CountToTransfer < transferableCountToTransferStoppingPoint2.threshold && (transferableCountToTransferStoppingPoint2.threshold < num6 || num5 == null))
								{
									label2 = transferableCountToTransferStoppingPoint2.rightLabel;
									num5 = new int?(transferableCountToTransferStoppingPoint2.threshold);
								}
							}
							else if (trad.CountToTransfer > transferableCountToTransferStoppingPoint2.threshold && (transferableCountToTransferStoppingPoint2.threshold > num6 || num5 == null))
							{
								label2 = transferableCountToTransferStoppingPoint2.rightLabel;
								num5 = new int?(transferableCountToTransferStoppingPoint2.threshold);
							}
						}
						rect5.x += rect5.width;
						if (Widgets.ButtonText(rect5, label2, true, false, true))
						{
							if (num5 != null)
							{
								trad.AdjustTo(num5.Value);
							}
							else if (num == 1)
							{
								trad.AdjustTo(trad.GetMinimumToTransfer());
							}
							else
							{
								trad.AdjustTo(trad.GetMaximumToTransfer());
							}
							SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						}
					}
				}
			}
			if (trad.CountToTransfer != 0)
			{
				Rect position = new Rect(rect2.x + rect2.width / 2f - (float)(TransferableUIUtility.TradeArrow.width / 2), rect2.y + rect2.height / 2f - (float)(TransferableUIUtility.TradeArrow.height / 2), (float)TransferableUIUtility.TradeArrow.width, (float)TransferableUIUtility.TradeArrow.height);
				TransferablePositiveCountDirection positiveCountDirection2 = trad.PositiveCountDirection;
				if ((positiveCountDirection2 == TransferablePositiveCountDirection.Source && trad.CountToTransfer > 0) || (positiveCountDirection2 == TransferablePositiveCountDirection.Destination && trad.CountToTransfer < 0))
				{
					position.x += position.width;
					position.width *= -1f;
				}
				GUI.DrawTexture(position, TransferableUIUtility.TradeArrow);
			}
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x00191750 File Offset: 0x0018FB50
		public static void DrawTransferableInfo(Transferable trad, Rect idRect, Color labelColor)
		{
			if (trad.HasAnyThing)
			{
				if (Mouse.IsOver(idRect))
				{
					Widgets.DrawHighlight(idRect);
				}
				Rect rect = new Rect(0f, 0f, 27f, 27f);
				Widgets.ThingIcon(rect, trad.AnyThing, 1f);
				Widgets.InfoCardButton(40f, 0f, trad.AnyThing);
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect2 = new Rect(80f, 0f, idRect.width - 80f, idRect.height);
				Text.WordWrap = false;
				GUI.color = labelColor;
				Widgets.Label(rect2, trad.LabelCap);
				GUI.color = Color.white;
				Text.WordWrap = true;
				Transferable localTrad = trad;
				TooltipHandler.TipRegion(idRect, new TipSignal(delegate()
				{
					string result;
					if (!localTrad.HasAnyThing)
					{
						result = "";
					}
					else
					{
						string text = localTrad.LabelCap;
						string tipDescription = localTrad.TipDescription;
						if (!tipDescription.NullOrEmpty())
						{
							text = text + ": " + tipDescription;
						}
						result = text;
					}
					return result;
				}, localTrad.GetHashCode()));
			}
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x00191844 File Offset: 0x0018FC44
		public static float DefaultListOrderPriority(Transferable transferable)
		{
			float result;
			if (!transferable.HasAnyThing)
			{
				result = 0f;
			}
			else
			{
				result = TransferableUIUtility.DefaultListOrderPriority(transferable.ThingDef);
			}
			return result;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x0019187C File Offset: 0x0018FC7C
		public static float DefaultListOrderPriority(ThingDef def)
		{
			float result;
			if (def == ThingDefOf.Silver)
			{
				result = 100f;
			}
			else if (def == ThingDefOf.Gold)
			{
				result = 99f;
			}
			else if (def.Minifiable)
			{
				result = 90f;
			}
			else if (def.IsApparel)
			{
				result = 80f;
			}
			else if (def.IsRangedWeapon)
			{
				result = 70f;
			}
			else if (def.IsMeleeWeapon)
			{
				result = 60f;
			}
			else if (def.isTechHediff)
			{
				result = 50f;
			}
			else if (def.CountAsResource)
			{
				result = -10f;
			}
			else
			{
				result = 20f;
			}
			return result;
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x00191948 File Offset: 0x0018FD48
		public static void DoTransferableSorters(TransferableSorterDef sorter1, TransferableSorterDef sorter2, Action<TransferableSorterDef> sorter1Setter, Action<TransferableSorterDef> sorter2Setter)
		{
			Rect position = new Rect(0f, 0f, 350f, 27f);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Tiny;
			Rect rect = new Rect(0f, 0f, 60f, 27f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, "SortBy".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(rect.xMax + 10f, 0f, 130f, 27f);
			if (Widgets.ButtonText(rect2, sorter1.LabelCap, true, false, true))
			{
				TransferableUIUtility.OpenSorterChangeFloatMenu(sorter1Setter);
			}
			Rect rect3 = new Rect(rect2.xMax + 10f, 0f, 130f, 27f);
			if (Widgets.ButtonText(rect3, sorter2.LabelCap, true, false, true))
			{
				TransferableUIUtility.OpenSorterChangeFloatMenu(sorter2Setter);
			}
			GUI.EndGroup();
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x00191A38 File Offset: 0x0018FE38
		private static void OpenSorterChangeFloatMenu(Action<TransferableSorterDef> sorterSetter)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<TransferableSorterDef> allDefsListForReading = DefDatabase<TransferableSorterDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				TransferableSorterDef def = allDefsListForReading[i];
				list.Add(new FloatMenuOption(def.LabelCap, delegate()
				{
					sorterSetter(def);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x00191AD4 File Offset: 0x0018FED4
		public static void DrawExtraInfo(List<TransferableUIUtility.ExtraInfo> info, Rect rect)
		{
			if (rect.width > (float)info.Count * 230f)
			{
				rect.x += Mathf.Floor((rect.width - (float)info.Count * 230f) / 2f);
				rect.width = (float)info.Count * 230f;
			}
			GUI.BeginGroup(rect);
			float num = Mathf.Floor(rect.width / (float)info.Count);
			float num2 = 0f;
			for (int i = 0; i < info.Count; i++)
			{
				float num3 = (i != info.Count - 1) ? num : (rect.width - num2);
				Rect rect2 = new Rect(num2, 0f, num3, rect.height);
				Rect rect3 = new Rect(num2, 0f, num3, rect.height / 2f);
				Rect rect4 = new Rect(num2, rect.height / 2f, num3, rect.height / 2f);
				bool flag = Time.time - info[i].lastFlashTime < 1f;
				if (flag)
				{
					GUI.DrawTexture(rect2, TransferableUIUtility.FlashTex);
				}
				Text.Anchor = TextAnchor.LowerCenter;
				Text.Font = GameFont.Tiny;
				GUI.color = Color.gray;
				Widgets.Label(new Rect(rect3.x, rect3.y - 2f, rect3.width, rect3.height - -3f), info[i].key);
				Rect rect5 = new Rect(rect4.x, rect4.y + -3f + 2f, rect4.width, rect4.height - -3f);
				Text.Font = GameFont.Small;
				if (info[i].secondValue.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperCenter;
					GUI.color = info[i].color;
					Widgets.Label(rect5, info[i].value);
				}
				else
				{
					Rect rect6 = rect5;
					rect6.width = Mathf.Floor(rect5.width / 2f - 15f);
					Text.Anchor = TextAnchor.UpperRight;
					GUI.color = info[i].color;
					Widgets.Label(rect6, info[i].value);
					Rect rect7 = rect5;
					rect7.xMin += Mathf.Ceil(rect5.width / 2f + 15f);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = info[i].secondColor;
					Widgets.Label(rect7, info[i].secondValue);
					Rect position = rect5;
					position.x = Mathf.Floor(rect5.x + rect5.width / 2f - 7.5f);
					position.y += 3f;
					position.width = 15f;
					position.height = 15f;
					GUI.color = Color.white;
					GUI.DrawTexture(position, TransferableUIUtility.DividerTex);
				}
				GUI.color = Color.white;
				Widgets.DrawHighlightIfMouseover(rect2);
				TooltipHandler.TipRegion(rect2, info[i].tip);
				num2 += num3;
			}
			GUI.EndGroup();
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04001945 RID: 6469
		private static List<TransferableCountToTransferStoppingPoint> stoppingPoints = new List<TransferableCountToTransferStoppingPoint>();

		// Token: 0x04001946 RID: 6470
		private const float AmountAreaWidth = 90f;

		// Token: 0x04001947 RID: 6471
		private const float AmountAreaHeight = 25f;

		// Token: 0x04001948 RID: 6472
		private const float AdjustArrowWidth = 30f;

		// Token: 0x04001949 RID: 6473
		public const float ResourceIconSize = 27f;

		// Token: 0x0400194A RID: 6474
		public const float SortersHeight = 27f;

		// Token: 0x0400194B RID: 6475
		public const float ExtraInfoHeight = 40f;

		// Token: 0x0400194C RID: 6476
		public const float ExtraInfoMargin = 12f;

		// Token: 0x0400194D RID: 6477
		public static readonly Color ZeroCountColor = new Color(0.5f, 0.5f, 0.5f);

		// Token: 0x0400194E RID: 6478
		public static readonly Texture2D FlashTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0f, 0f, 0.4f));

		// Token: 0x0400194F RID: 6479
		private static readonly Texture2D TradeArrow = ContentFinder<Texture2D>.Get("UI/Widgets/TradeArrow", true);

		// Token: 0x04001950 RID: 6480
		private static readonly Texture2D DividerTex = ContentFinder<Texture2D>.Get("UI/Widgets/Divider", true);

		// Token: 0x0200082C RID: 2092
		public struct ExtraInfo
		{
			// Token: 0x06002F0A RID: 12042 RVA: 0x00191ED4 File Offset: 0x001902D4
			public ExtraInfo(string key, string value, Color color, string tip, float lastFlashTime = -9999f)
			{
				this.key = key;
				this.value = value;
				this.color = color;
				this.tip = tip;
				this.lastFlashTime = lastFlashTime;
				this.secondValue = null;
				this.secondColor = default(Color);
			}

			// Token: 0x06002F0B RID: 12043 RVA: 0x00191F1D File Offset: 0x0019031D
			public ExtraInfo(string key, string value, Color color, string tip, string secondValue, Color secondColor, float lastFlashTime = -9999f)
			{
				this.key = key;
				this.value = value;
				this.color = color;
				this.tip = tip;
				this.lastFlashTime = lastFlashTime;
				this.secondValue = secondValue;
				this.secondColor = secondColor;
			}

			// Token: 0x04001951 RID: 6481
			public string key;

			// Token: 0x04001952 RID: 6482
			public string value;

			// Token: 0x04001953 RID: 6483
			public string secondValue;

			// Token: 0x04001954 RID: 6484
			public string tip;

			// Token: 0x04001955 RID: 6485
			public float lastFlashTime;

			// Token: 0x04001956 RID: 6486
			public Color color;

			// Token: 0x04001957 RID: 6487
			public Color secondColor;
		}
	}
}
