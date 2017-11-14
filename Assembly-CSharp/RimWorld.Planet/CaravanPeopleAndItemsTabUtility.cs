using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public static class CaravanPeopleAndItemsTabUtility
	{
		private const float PawnRowHeight = 50f;

		private const float NonPawnRowHeight = 30f;

		private const float PawnLabelHeight = 18f;

		private const float PawnLabelColumnWidth = 100f;

		private const float NonPawnLabelColumnWidth = 300f;

		public const float ItemMassColumnWidth = 60f;

		private const float SpaceAroundIcon = 4f;

		private const float NeedWidth = 100f;

		private const float NeedMargin = 10f;

		public const float SpecificTabButtonSize = 24f;

		public const float AbandonButtonSize = 24f;

		private const float AbandonSpecificCountButtonSize = 24f;

		public static readonly Texture2D AbandonButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/Abandon", true);

		public static readonly Texture2D AbandonSpecificCountButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/AbandonSpecificCount", true);

		public static readonly Texture2D SpecificTabButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/OpenSpecificTab", true);

		public static readonly Color OpenedSpecificTabButtonColor = new Color(0f, 0.8f, 0f);

		public static readonly Color OpenedSpecificTabButtonMouseoverColor = new Color(0f, 0.5f, 0f);

		private static List<Need> tmpNeeds = new List<Need>();

		private static List<Thought> thoughtGroupsPresent = new List<Thought>();

		private static List<Thought> thoughtGroup = new List<Thought>();

		public static void DoRows(Vector2 size, List<Thing> things, Caravan caravan, ref Vector2 scrollPosition, ref float scrollViewHeight, bool alwaysShowItemsSection, ref Pawn specificNeedsTabForPawn, bool doNeeds = true)
		{
			if (specificNeedsTabForPawn != null && (!things.Contains(specificNeedsTabForPawn) || specificNeedsTabForPawn.Dead))
			{
				specificNeedsTabForPawn = null;
			}
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, size.x, size.y).ContractedBy(10f);
			Rect viewRect = new Rect(0f, 0f, (float)(rect.width - 16.0), scrollViewHeight);
			bool listingUsesAbandonSpecificCountButtons = CaravanPeopleAndItemsTabUtility.AnyItemOrEmpty(things);
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			float num = 0f;
			bool flag = false;
			for (int i = 0; i < things.Count; i++)
			{
				Pawn pawn = things[i] as Pawn;
				if (pawn != null && pawn.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref num, viewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					CaravanPeopleAndItemsTabUtility.DoRow(ref num, viewRect, rect, scrollPosition, (Thing)pawn, caravan, ref specificNeedsTabForPawn, doNeeds, listingUsesAbandonSpecificCountButtons);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < things.Count; j++)
			{
				Pawn pawn2 = things[j] as Pawn;
				if (pawn2 != null && !pawn2.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref num, viewRect.width, "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					CaravanPeopleAndItemsTabUtility.DoRow(ref num, viewRect, rect, scrollPosition, (Thing)pawn2, caravan, ref specificNeedsTabForPawn, doNeeds, listingUsesAbandonSpecificCountButtons);
				}
			}
			bool flag3 = false;
			if (alwaysShowItemsSection)
			{
				Widgets.ListSeparator(ref num, viewRect.width, "CaravanItems".Translate());
			}
			for (int k = 0; k < things.Count; k++)
			{
				if (!(things[k] is Pawn))
				{
					if (!flag3)
					{
						if (!alwaysShowItemsSection)
						{
							Widgets.ListSeparator(ref num, viewRect.width, "CaravanItems".Translate());
						}
						flag3 = true;
					}
					CaravanPeopleAndItemsTabUtility.DoRow(ref num, viewRect, rect, scrollPosition, things[k], caravan, ref specificNeedsTabForPawn, doNeeds, listingUsesAbandonSpecificCountButtons);
				}
			}
			if (alwaysShowItemsSection && !flag3)
			{
				GUI.color = Color.gray;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(new Rect(0f, num, viewRect.width, 25f), "NoneBrackets".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				num = (float)(num + 25.0);
				GUI.color = Color.white;
			}
			if (Event.current.type == EventType.Layout)
			{
				scrollViewHeight = (float)(num + 30.0);
			}
			Widgets.EndScrollView();
		}

		public static Vector2 GetSize(List<Thing> things, float paneTopY, bool doNeeds = true)
		{
			float a = 0f;
			if (things.Any((Thing x) => x is Pawn))
			{
				a = 100f;
				if (doNeeds)
				{
					a = (float)(a + (float)CaravanPeopleAndItemsTabUtility.MaxNeedsCount(things) * 100.0);
				}
				a = (float)(a + 24.0);
			}
			float b = 0f;
			if (CaravanPeopleAndItemsTabUtility.AnyItemOrEmpty(things))
			{
				b = 300f;
				b = (float)(b + 24.0);
				b = (float)(b + 60.0);
			}
			Vector2 result = default(Vector2);
			result.x = (float)(103.0 + Mathf.Max(a, b) + 16.0);
			result.y = Mathf.Min(550f, (float)(paneTopY - 30.0));
			return result;
		}

		private static bool AnyItemOrEmpty(List<Thing> things)
		{
			return things.Any((Thing x) => !(x is Pawn)) || !things.Any();
		}

		public static void DoAbandonButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect((float)(rowRect.width - 24.0), (float)((rowRect.height - 24.0) / 2.0), 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanPeopleAndItemsTabUtility.AbandonButtonTex))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, caravan, false), Gen.HashCombineInt(t.GetHashCode(), 1383004931));
		}

		private static void DoAbandonSpecificCountButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect((float)(rowRect.width - 24.0), (float)((rowRect.height - 24.0) / 2.0), 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanPeopleAndItemsTabUtility.AbandonSpecificCountButtonTex))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, caravan, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
		}

		public static void DoOpenSpecificTabButton(Rect rowRect, Pawn p, ref Pawn specificTabForPawn)
		{
			Color baseColor = (p != specificTabForPawn) ? Color.white : CaravanPeopleAndItemsTabUtility.OpenedSpecificTabButtonColor;
			Color mouseoverColor = (p != specificTabForPawn) ? GenUI.MouseoverColor : CaravanPeopleAndItemsTabUtility.OpenedSpecificTabButtonMouseoverColor;
			Rect rect = new Rect((float)(rowRect.width - 24.0), (float)((rowRect.height - 24.0) / 2.0), 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanPeopleAndItemsTabUtility.SpecificTabButtonTex, baseColor, mouseoverColor))
			{
				if (p == specificTabForPawn)
				{
					specificTabForPawn = null;
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
				else
				{
					specificTabForPawn = p;
					SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				}
			}
			TooltipHandler.TipRegion(rect, "OpenSpecificTabButtonTip".Translate());
			GUI.color = Color.white;
		}

		private static int MaxNeedsCount(List<Thing> things)
		{
			int num = 0;
			for (int i = 0; i < things.Count; i++)
			{
				Pawn pawn = things[i] as Pawn;
				if (pawn != null)
				{
					CaravanPeopleAndItemsTabUtility.GetNeedsToDisplay(pawn, CaravanPeopleAndItemsTabUtility.tmpNeeds);
					num = Mathf.Max(num, CaravanPeopleAndItemsTabUtility.tmpNeeds.Count);
				}
			}
			return num;
		}

		private static void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Vector2 scrollPosition, Thing thing, Caravan caravan, ref Pawn specificNeedsTabForPawn, bool doNeeds, bool listingUsesAbandonSpecificCountButtons)
		{
			float num = (float)((!(thing is Pawn)) ? 30.0 : 50.0);
			float num2 = scrollPosition.y - num;
			float num3 = scrollPosition.y + scrollOutRect.height;
			if (curY > num2 && curY < num3)
			{
				CaravanPeopleAndItemsTabUtility.DoRow(new Rect(0f, curY, viewRect.width, num), thing, caravan, ref specificNeedsTabForPawn, doNeeds, listingUsesAbandonSpecificCountButtons);
			}
			curY += num;
		}

		private static void DoRow(Rect rect, Thing thing, Caravan caravan, ref Pawn specificNeedsTabForPawn, bool doNeeds, bool listingUsesAbandonSpecificCountButtons)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			Pawn pawn = thing as Pawn;
			if (listingUsesAbandonSpecificCountButtons)
			{
				if (thing.stackCount != 1)
				{
					CaravanPeopleAndItemsTabUtility.DoAbandonSpecificCountButton(rect2, thing, caravan);
				}
				rect2.width -= 24f;
			}
			CaravanPeopleAndItemsTabUtility.DoAbandonButton(rect2, thing, caravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton((float)(rect2.width - 24.0), (float)((rect.height - 24.0) / 2.0), thing);
			rect2.width -= 24f;
			if (pawn != null && !pawn.Dead)
			{
				CaravanPeopleAndItemsTabUtility.DoOpenSpecificTabButton(rect2, pawn, ref specificNeedsTabForPawn);
				rect2.width -= 24f;
			}
			if (pawn == null)
			{
				Rect rect3 = rect2;
				rect3.xMin = (float)(rect3.xMax - 60.0);
				CaravanPeopleAndItemsTabUtility.TryDrawMass(thing, rect3);
				rect2.width -= 60f;
			}
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect4 = new Rect(4f, (float)((rect.height - 27.0) / 2.0), 27f, 27f);
			Widgets.ThingIcon(rect4, thing, 1f);
			if (pawn != null)
			{
				Rect bgRect = new Rect((float)(rect4.xMax + 4.0), 16f, 100f, 18f);
				GenMapUI.DrawPawnLabel(pawn, bgRect, 1f, 100f, null, GameFont.Small, false, false);
				if (doNeeds)
				{
					CaravanPeopleAndItemsTabUtility.GetNeedsToDisplay(pawn, CaravanPeopleAndItemsTabUtility.tmpNeeds);
					float xMax = bgRect.xMax;
					for (int i = 0; i < CaravanPeopleAndItemsTabUtility.tmpNeeds.Count; i++)
					{
						Need need = CaravanPeopleAndItemsTabUtility.tmpNeeds[i];
						int maxThresholdMarkers = 0;
						bool doTooltip = true;
						Rect rect5 = new Rect(xMax, 0f, 100f, 50f);
						Need_Mood mood = need as Need_Mood;
						if (mood != null)
						{
							maxThresholdMarkers = 1;
							doTooltip = false;
							TooltipHandler.TipRegion(rect5, new TipSignal(() => CaravanPeopleAndItemsTabUtility.CustomMoodNeedTooltip(mood), rect5.GetHashCode()));
						}
						need.DrawOnGUI(rect5, maxThresholdMarkers, 10f, false, doTooltip);
						xMax = rect5.xMax;
					}
				}
				if (pawn.Downed)
				{
					GUI.color = new Color(1f, 0f, 0f, 0.5f);
					Widgets.DrawLineHorizontal(0f, (float)(rect.height / 2.0), rect.width);
					GUI.color = Color.white;
				}
			}
			else
			{
				Rect rect6 = new Rect((float)(rect4.xMax + 4.0), 0f, 300f, 30f);
				Text.Anchor = TextAnchor.MiddleLeft;
				Text.WordWrap = false;
				Widgets.Label(rect6, thing.LabelCap.Truncate(rect6.width, null));
				Text.Anchor = TextAnchor.UpperLeft;
				Text.WordWrap = true;
			}
			GUI.EndGroup();
		}

		public static void TryDrawMass(Thing thing, Rect rect)
		{
			float mass = thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount;
			GUI.color = TransferableOneWayWidget.ItemMassColor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect, mass.ToStringMass());
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		private static void GetNeedsToDisplay(Pawn p, List<Need> outNeeds)
		{
			outNeeds.Clear();
			List<Need> allNeeds = p.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				Need need = allNeeds[i];
				if (need.def.showForCaravanMembers)
				{
					outNeeds.Add(need);
				}
			}
			PawnNeedsUIUtility.SortInDisplayOrder(outNeeds);
		}

		private static string CustomMoodNeedTooltip(Need_Mood mood)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(mood.GetTipString());
			PawnNeedsUIUtility.GetThoughtGroupsInDisplayOrder(mood, CaravanPeopleAndItemsTabUtility.thoughtGroupsPresent);
			bool flag = false;
			for (int i = 0; i < CaravanPeopleAndItemsTabUtility.thoughtGroupsPresent.Count; i++)
			{
				Thought group = CaravanPeopleAndItemsTabUtility.thoughtGroupsPresent[i];
				mood.thoughts.GetMoodThoughts(group, CaravanPeopleAndItemsTabUtility.thoughtGroup);
				Thought leadingThoughtInGroup = PawnNeedsUIUtility.GetLeadingThoughtInGroup(CaravanPeopleAndItemsTabUtility.thoughtGroup);
				if (leadingThoughtInGroup.VisibleInNeedsTab)
				{
					if (!flag)
					{
						flag = true;
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(leadingThoughtInGroup.LabelCap);
					if (CaravanPeopleAndItemsTabUtility.thoughtGroup.Count > 1)
					{
						stringBuilder.Append(" x");
						stringBuilder.Append(CaravanPeopleAndItemsTabUtility.thoughtGroup.Count);
					}
					stringBuilder.Append(": ");
					stringBuilder.AppendLine(mood.thoughts.MoodOffsetOfGroup(group).ToString("##0"));
				}
			}
			return stringBuilder.ToString();
		}
	}
}
