using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081B RID: 2075
	public static class NeedsCardUtility
	{
		// Token: 0x06002E53 RID: 11859 RVA: 0x0018A108 File Offset: 0x00188508
		public static Vector2 GetSize(Pawn pawn)
		{
			NeedsCardUtility.UpdateDisplayNeeds(pawn);
			Vector2 result;
			if (pawn.needs.mood != null)
			{
				result = NeedsCardUtility.FullSize;
			}
			else
			{
				result = new Vector2(225f, (float)NeedsCardUtility.displayNeeds.Count * Mathf.Min(70f, NeedsCardUtility.FullSize.y / (float)NeedsCardUtility.displayNeeds.Count));
			}
			return result;
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x0018A178 File Offset: 0x00188578
		public static void DoNeedsMoodAndThoughts(Rect rect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			Rect rect2 = new Rect(rect.x, rect.y, 225f, rect.height);
			NeedsCardUtility.DoNeeds(rect2, pawn);
			if (pawn.needs.mood != null)
			{
				Rect rect3 = new Rect(rect2.xMax, rect.y, rect.width - rect2.width, rect.height);
				NeedsCardUtility.DoMoodAndThoughts(rect3, pawn, ref thoughtScrollPosition);
			}
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x0018A1F4 File Offset: 0x001885F4
		public static void DoNeeds(Rect rect, Pawn pawn)
		{
			NeedsCardUtility.UpdateDisplayNeeds(pawn);
			float num = 0f;
			for (int i = 0; i < NeedsCardUtility.displayNeeds.Count; i++)
			{
				Need need = NeedsCardUtility.displayNeeds[i];
				Rect rect2 = new Rect(rect.x, rect.y + num, rect.width, Mathf.Min(70f, rect.height / (float)NeedsCardUtility.displayNeeds.Count));
				if (!need.def.major)
				{
					if (i > 0 && NeedsCardUtility.displayNeeds[i - 1].def.major)
					{
						rect2.y += 10f;
					}
					rect2.width *= 0.73f;
					rect2.height = Mathf.Max(rect2.height * 0.666f, 30f);
				}
				need.DrawOnGUI(rect2, int.MaxValue, -1f, true, true);
				num = rect2.yMax;
			}
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x0018A308 File Offset: 0x00188708
		private static void DoMoodAndThoughts(Rect rect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.width * 0.8f, 70f);
			pawn.needs.mood.DrawOnGUI(rect2, int.MaxValue, -1f, true, true);
			Rect rect3 = new Rect(0f, 80f, rect.width, rect.height - 70f - 10f);
			rect3 = rect3.ContractedBy(10f);
			NeedsCardUtility.DrawThoughtListing(rect3, pawn, ref thoughtScrollPosition);
			GUI.EndGroup();
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x0018A3A4 File Offset: 0x001887A4
		private static void UpdateDisplayNeeds(Pawn pawn)
		{
			NeedsCardUtility.displayNeeds.Clear();
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (allNeeds[i].ShowOnNeedList)
				{
					NeedsCardUtility.displayNeeds.Add(allNeeds[i]);
				}
			}
			PawnNeedsUIUtility.SortInDisplayOrder(NeedsCardUtility.displayNeeds);
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x0018A414 File Offset: 0x00188814
		private static void DrawThoughtListing(Rect listingRect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			if (Event.current.type != EventType.Layout)
			{
				Text.Font = GameFont.Small;
				PawnNeedsUIUtility.GetThoughtGroupsInDisplayOrder(pawn.needs.mood, NeedsCardUtility.thoughtGroupsPresent);
				float height = (float)NeedsCardUtility.thoughtGroupsPresent.Count * 24f;
				Widgets.BeginScrollView(listingRect, ref thoughtScrollPosition, new Rect(0f, 0f, listingRect.width - 16f, height), true);
				Text.Anchor = TextAnchor.MiddleLeft;
				float num = 0f;
				for (int i = 0; i < NeedsCardUtility.thoughtGroupsPresent.Count; i++)
				{
					Rect rect = new Rect(0f, num, listingRect.width - 16f, 20f);
					if (NeedsCardUtility.DrawThoughtGroup(rect, NeedsCardUtility.thoughtGroupsPresent[i], pawn))
					{
						num += 24f;
					}
				}
				Widgets.EndScrollView();
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x0018A500 File Offset: 0x00188900
		private static bool DrawThoughtGroup(Rect rect, Thought group, Pawn pawn)
		{
			try
			{
				pawn.needs.mood.thoughts.GetMoodThoughts(group, NeedsCardUtility.thoughtGroup);
				Thought leadingThoughtInGroup = PawnNeedsUIUtility.GetLeadingThoughtInGroup(NeedsCardUtility.thoughtGroup);
				if (!leadingThoughtInGroup.VisibleInNeedsTab)
				{
					NeedsCardUtility.thoughtGroup.Clear();
					return false;
				}
				if (leadingThoughtInGroup != NeedsCardUtility.thoughtGroup[0])
				{
					NeedsCardUtility.thoughtGroup.Remove(leadingThoughtInGroup);
					NeedsCardUtility.thoughtGroup.Insert(0, leadingThoughtInGroup);
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(leadingThoughtInGroup.Description);
				if (group.def.DurationTicks > 5)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					Thought_Memory thought_Memory = leadingThoughtInGroup as Thought_Memory;
					if (thought_Memory != null)
					{
						if (NeedsCardUtility.thoughtGroup.Count == 1)
						{
							stringBuilder.Append("ThoughtExpiresIn".Translate(new object[]
							{
								(group.def.DurationTicks - thought_Memory.age).ToStringTicksToPeriod()
							}));
						}
						else
						{
							Thought_Memory thought_Memory2 = (Thought_Memory)NeedsCardUtility.thoughtGroup[NeedsCardUtility.thoughtGroup.Count - 1];
							stringBuilder.Append("ThoughtStartsExpiringIn".Translate(new object[]
							{
								(group.def.DurationTicks - thought_Memory.age).ToStringTicksToPeriod()
							}));
							stringBuilder.AppendLine();
							stringBuilder.Append("ThoughtFinishesExpiringIn".Translate(new object[]
							{
								(group.def.DurationTicks - thought_Memory2.age).ToStringTicksToPeriod()
							}));
						}
					}
				}
				if (NeedsCardUtility.thoughtGroup.Count > 1)
				{
					bool flag = false;
					for (int i = 1; i < NeedsCardUtility.thoughtGroup.Count; i++)
					{
						bool flag2 = false;
						for (int j = 0; j < i; j++)
						{
							if (NeedsCardUtility.thoughtGroup[i].LabelCap == NeedsCardUtility.thoughtGroup[j].LabelCap)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							if (!flag)
							{
								stringBuilder.AppendLine();
								stringBuilder.AppendLine();
								flag = true;
							}
							stringBuilder.AppendLine("+ " + NeedsCardUtility.thoughtGroup[i].LabelCap);
						}
					}
				}
				TooltipHandler.TipRegion(rect, new TipSignal(stringBuilder.ToString(), 7291));
				Text.WordWrap = false;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect2 = new Rect(rect.x + 10f, rect.y, 225f, rect.height);
				rect2.yMin -= 3f;
				rect2.yMax += 3f;
				string text = leadingThoughtInGroup.LabelCap;
				if (NeedsCardUtility.thoughtGroup.Count > 1)
				{
					text = text + " x" + NeedsCardUtility.thoughtGroup.Count;
				}
				Widgets.Label(rect2, text);
				Text.Anchor = TextAnchor.MiddleCenter;
				float num = pawn.needs.mood.thoughts.MoodOffsetOfGroup(group);
				if (num == 0f)
				{
					GUI.color = NeedsCardUtility.NoEffectColor;
				}
				else if (num > 0f)
				{
					GUI.color = NeedsCardUtility.MoodColor;
				}
				else
				{
					GUI.color = NeedsCardUtility.MoodColorNegative;
				}
				Rect rect3 = new Rect(rect.x + 235f, rect.y, 32f, rect.height);
				Widgets.Label(rect3, num.ToString("##0"));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.WordWrap = true;
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Exception in DrawThoughtGroup for ",
					group.def,
					" on ",
					pawn,
					": ",
					ex.ToString()
				}), 3452698, false);
			}
			return true;
		}

		// Token: 0x040018BF RID: 6335
		private static List<Need> displayNeeds = new List<Need>();

		// Token: 0x040018C0 RID: 6336
		private static readonly Color MoodColor = new Color(0.1f, 1f, 0.1f);

		// Token: 0x040018C1 RID: 6337
		private static readonly Color MoodColorNegative = new Color(0.8f, 0.4f, 0.4f);

		// Token: 0x040018C2 RID: 6338
		private static readonly Color NoEffectColor = new Color(0.5f, 0.5f, 0.5f, 0.75f);

		// Token: 0x040018C3 RID: 6339
		private const float ThoughtHeight = 20f;

		// Token: 0x040018C4 RID: 6340
		private const float ThoughtSpacing = 4f;

		// Token: 0x040018C5 RID: 6341
		private const float ThoughtIntervalY = 24f;

		// Token: 0x040018C6 RID: 6342
		private const float MoodX = 235f;

		// Token: 0x040018C7 RID: 6343
		private const float MoodNumberWidth = 32f;

		// Token: 0x040018C8 RID: 6344
		private const float NeedsColumnWidth = 225f;

		// Token: 0x040018C9 RID: 6345
		public static readonly Vector2 FullSize = new Vector2(580f, 520f);

		// Token: 0x040018CA RID: 6346
		private static List<Thought> thoughtGroupsPresent = new List<Thought>();

		// Token: 0x040018CB RID: 6347
		private static List<Thought> thoughtGroup = new List<Thought>();
	}
}
