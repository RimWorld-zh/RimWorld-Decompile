using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class TransferableUIUtility
	{
		private const float AmountAreaWidth = 90f;

		private const float AmountAreaHeight = 25f;

		private const float AdjustArrowWidth = 30f;

		public const float ResourceIconSize = 27f;

		public const float SortersHeight = 27f;

		private static List<TransferableCountToTransferStoppingPoint> stoppingPoints = new List<TransferableCountToTransferStoppingPoint>();

		public static readonly Color ZeroCountColor = new Color(0.5f, 0.5f, 0.5f);

		public static readonly Texture2D FlashTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0f, 0f, 0.4f));

		private static readonly Texture2D TradeArrow = ContentFinder<Texture2D>.Get("UI/Widgets/TradeArrow", true);

		public static void DoCountAdjustInterface(Rect rect, Transferable trad, int index, int min, int max, bool flash = false, List<TransferableCountToTransferStoppingPoint> extraStoppingPoints = null)
		{
			TransferableUIUtility.stoppingPoints.Clear();
			if (extraStoppingPoints != null)
			{
				TransferableUIUtility.stoppingPoints.AddRange(extraStoppingPoints);
			}
			for (int i = TransferableUIUtility.stoppingPoints.Count - 1; i >= 0; i--)
			{
				TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint = TransferableUIUtility.stoppingPoints[i];
				if (transferableCountToTransferStoppingPoint.threshold != 0)
				{
					TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint2 = TransferableUIUtility.stoppingPoints[i];
					if (transferableCountToTransferStoppingPoint2.threshold > min)
					{
						TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint3 = TransferableUIUtility.stoppingPoints[i];
						if (transferableCountToTransferStoppingPoint3.threshold >= max)
							goto IL_007c;
						continue;
					}
					goto IL_007c;
				}
				continue;
				IL_007c:
				TransferableUIUtility.stoppingPoints.RemoveAt(i);
			}
			bool flag = false;
			for (int j = 0; j < TransferableUIUtility.stoppingPoints.Count; j++)
			{
				TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint4 = TransferableUIUtility.stoppingPoints[j];
				if (transferableCountToTransferStoppingPoint4.threshold == 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TransferableUIUtility.stoppingPoints.Add(new TransferableCountToTransferStoppingPoint(0, "0", "0"));
			}
			TransferableUIUtility.DoCountAdjustInterfaceInternal(rect, trad, index, min, max, flash);
		}

		private static void DoCountAdjustInterfaceInternal(Rect rect, Transferable trad, int index, int min, int max, bool flash)
		{
			rect = rect.Rounded();
			Vector2 center = rect.center;
			double x = center.x - 45.0;
			Vector2 center2 = rect.center;
			Rect rect2 = new Rect((float)x, (float)(center2.y - 12.5), 90f, 25f).Rounded();
			if (flash)
			{
				GUI.DrawTexture(rect2, TransferableUIUtility.FlashTex);
			}
			TransferableOneWay transferableOneWay = trad as TransferableOneWay;
			bool flag = transferableOneWay != null && transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn && transferableOneWay.MaxCount == 1;
			if (!trad.Interactive)
			{
				GUI.color = ((trad.CountToTransfer != 0) ? Color.white : TransferableUIUtility.ZeroCountColor);
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect2, trad.CountToTransfer.ToStringCached());
			}
			else if (flag)
			{
				bool flag2;
				bool flag3 = flag2 = (trad.CountToTransfer != 0);
				Widgets.Checkbox(rect2.position, ref flag2, 24f, false);
				if (flag2 != flag3)
				{
					if (flag2)
					{
						trad.AdjustTo(trad.GetMaximum());
					}
					else
					{
						trad.AdjustTo(trad.GetMinimum());
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
				Widgets.TextFieldNumeric(rect3, ref countToTransfer, ref editBuffer, (float)min, (float)max);
				trad.AdjustTo(countToTransfer);
				trad.EditBuffer = editBuffer;
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			if (trad.Interactive && !flag)
			{
				TransferablePositiveCountDirection positiveCountDirection = trad.PositiveCountDirection;
				int num = (positiveCountDirection == TransferablePositiveCountDirection.Source) ? 1 : (-1);
				int num2 = GenUI.CurrentAdjustmentMultiplier();
				bool flag4 = trad.GetRange() == 1;
				if (trad.CanAdjustBy(num * num2).Accepted)
				{
					Rect rect4 = new Rect((float)(rect2.x - 30.0), rect.y, 30f, rect.height);
					if (flag4)
					{
						rect4.x -= rect4.width;
						rect4.width += rect4.width;
					}
					if (Widgets.ButtonText(rect4, "<", true, false, true))
					{
						trad.AdjustBy(num * num2);
						SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
					}
					if (!flag4)
					{
						string label = "<<";
						int? nullable = default(int?);
						int num3 = 0;
						for (int i = 0; i < TransferableUIUtility.stoppingPoints.Count; i++)
						{
							TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint = TransferableUIUtility.stoppingPoints[i];
							if (positiveCountDirection == TransferablePositiveCountDirection.Source)
							{
								if (trad.CountToTransfer < transferableCountToTransferStoppingPoint.threshold && (transferableCountToTransferStoppingPoint.threshold < num3 || !nullable.HasValue))
								{
									label = transferableCountToTransferStoppingPoint.leftLabel;
									nullable = new int?(transferableCountToTransferStoppingPoint.threshold);
								}
							}
							else if (trad.CountToTransfer > transferableCountToTransferStoppingPoint.threshold && (transferableCountToTransferStoppingPoint.threshold > num3 || !nullable.HasValue))
							{
								label = transferableCountToTransferStoppingPoint.leftLabel;
								nullable = new int?(transferableCountToTransferStoppingPoint.threshold);
							}
						}
						rect4.x -= rect4.width;
						if (Widgets.ButtonText(rect4, label, true, false, true))
						{
							if (nullable.HasValue)
							{
								trad.AdjustTo(nullable.Value);
							}
							else if (num == 1)
							{
								trad.AdjustTo(trad.GetMaximum());
							}
							else
							{
								trad.AdjustTo(trad.GetMinimum());
							}
							SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
						}
					}
				}
				if (trad.CanAdjustBy(-num * num2).Accepted)
				{
					Rect rect5 = new Rect(rect2.xMax, rect.y, 30f, rect.height);
					if (flag4)
					{
						rect5.width += rect5.width;
					}
					if (Widgets.ButtonText(rect5, ">", true, false, true))
					{
						trad.AdjustBy(-num * num2);
						SoundDefOf.TickLow.PlayOneShotOnCamera(null);
					}
					if (!flag4)
					{
						string label2 = ">>";
						int? nullable2 = default(int?);
						int num4 = 0;
						for (int j = 0; j < TransferableUIUtility.stoppingPoints.Count; j++)
						{
							TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint2 = TransferableUIUtility.stoppingPoints[j];
							if (positiveCountDirection == TransferablePositiveCountDirection.Destination)
							{
								if (trad.CountToTransfer < transferableCountToTransferStoppingPoint2.threshold && (transferableCountToTransferStoppingPoint2.threshold < num4 || !nullable2.HasValue))
								{
									label2 = transferableCountToTransferStoppingPoint2.rightLabel;
									nullable2 = new int?(transferableCountToTransferStoppingPoint2.threshold);
								}
							}
							else if (trad.CountToTransfer > transferableCountToTransferStoppingPoint2.threshold && (transferableCountToTransferStoppingPoint2.threshold > num4 || !nullable2.HasValue))
							{
								label2 = transferableCountToTransferStoppingPoint2.rightLabel;
								nullable2 = new int?(transferableCountToTransferStoppingPoint2.threshold);
							}
						}
						rect5.x += rect5.width;
						if (Widgets.ButtonText(rect5, label2, true, false, true))
						{
							if (nullable2.HasValue)
							{
								trad.AdjustTo(nullable2.Value);
							}
							else if (num == 1)
							{
								trad.AdjustTo(trad.GetMinimum());
							}
							else
							{
								trad.AdjustTo(trad.GetMaximum());
							}
							SoundDefOf.TickLow.PlayOneShotOnCamera(null);
						}
					}
				}
			}
			if (trad.CountToTransfer == 0)
				return;
			Rect position = new Rect((float)(rect2.x + rect2.width / 2.0 - (float)(TransferableUIUtility.TradeArrow.width / 2)), (float)(rect2.y + rect2.height / 2.0 - (float)(TransferableUIUtility.TradeArrow.height / 2)), (float)TransferableUIUtility.TradeArrow.width, (float)TransferableUIUtility.TradeArrow.height);
			TransferablePositiveCountDirection positiveCountDirection2 = trad.PositiveCountDirection;
			if (positiveCountDirection2 == TransferablePositiveCountDirection.Source && trad.CountToTransfer > 0)
			{
				goto IL_0646;
			}
			if (positiveCountDirection2 == TransferablePositiveCountDirection.Destination && trad.CountToTransfer < 0)
				goto IL_0646;
			goto IL_066e;
			IL_0646:
			position.x += position.width;
			position.width *= -1f;
			goto IL_066e;
			IL_066e:
			GUI.DrawTexture(position, TransferableUIUtility.TradeArrow);
		}

		public static void DrawMassInfo(Rect rect, float usedMass, float availableMass, string tip, float lastMassFlashTime = -9999f, bool alignRight = false)
		{
			if (usedMass > availableMass)
			{
				GUI.color = Color.red;
			}
			else
			{
				GUI.color = Color.gray;
			}
			string text = "MassUsageInfo".Translate(usedMass.ToString("0.##"), availableMass.ToString("0.##"));
			Vector2 vector = Text.CalcSize(text);
			Rect rect2 = (!alignRight) ? new Rect(rect.x, rect.y, vector.x, vector.y) : new Rect(rect.xMax - vector.x, rect.y, vector.x, vector.y);
			if (Time.time - lastMassFlashTime < 1.0)
			{
				GUI.DrawTexture(rect2, TransferableUIUtility.FlashTex);
			}
			Widgets.Label(rect2, text);
			TooltipHandler.TipRegion(rect2, tip);
			GUI.color = Color.white;
		}

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
				Rect rect2 = new Rect(80f, 0f, (float)(idRect.width - 80.0), idRect.height);
				Text.WordWrap = false;
				GUI.color = labelColor;
				Widgets.Label(rect2, trad.Label);
				GUI.color = Color.white;
				Text.WordWrap = true;
				TooltipHandler.TipRegion(idRect, new TipSignal((Func<string>)delegate()
				{
					if (!trad.HasAnyThing)
					{
						return string.Empty;
					}
					return trad.Label + ": " + trad.TipDescription;
				}, trad.GetHashCode()));
			}
		}

		public static float DefaultListOrderPriority(Transferable transferable)
		{
			if (!transferable.HasAnyThing)
			{
				return 0f;
			}
			return TransferableUIUtility.DefaultListOrderPriority(transferable.ThingDef);
		}

		public static float DefaultListOrderPriority(ThingDef def)
		{
			if (def == ThingDefOf.Silver)
			{
				return 100f;
			}
			if (def == ThingDefOf.Gold)
			{
				return 99f;
			}
			if (def.Minifiable)
			{
				return 90f;
			}
			if (def.IsApparel)
			{
				return 80f;
			}
			if (def.IsRangedWeapon)
			{
				return 70f;
			}
			if (def.IsMeleeWeapon)
			{
				return 60f;
			}
			if (def.isBodyPartOrImplant)
			{
				return 50f;
			}
			if (def.CountAsResource)
			{
				return -10f;
			}
			return 20f;
		}

		public static void DoTransferableSorters(TransferableSorterDef sorter1, TransferableSorterDef sorter2, Action<TransferableSorterDef> sorter1Setter, Action<TransferableSorterDef> sorter2Setter)
		{
			Rect position = new Rect(0f, 0f, 350f, 27f);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Tiny;
			Rect rect = new Rect(0f, 0f, 60f, 27f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, "SortBy".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect((float)(rect.xMax + 10.0), 0f, 130f, 27f);
			if (Widgets.ButtonText(rect2, sorter1.LabelCap, true, false, true))
			{
				TransferableUIUtility.OpenSorterChangeFloatMenu(sorter1Setter);
			}
			Rect rect3 = new Rect((float)(rect2.xMax + 10.0), 0f, 130f, 27f);
			if (Widgets.ButtonText(rect3, sorter2.LabelCap, true, false, true))
			{
				TransferableUIUtility.OpenSorterChangeFloatMenu(sorter2Setter);
			}
			GUI.EndGroup();
		}

		private static void OpenSorterChangeFloatMenu(Action<TransferableSorterDef> sorterSetter)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<TransferableSorterDef> allDefsListForReading = DefDatabase<TransferableSorterDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				TransferableSorterDef def = allDefsListForReading[i];
				list.Add(new FloatMenuOption(def.LabelCap, (Action)delegate()
				{
					sorterSetter(def);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}
	}
}
