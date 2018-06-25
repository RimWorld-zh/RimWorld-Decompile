using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008A1 RID: 2209
	[StaticConstructorOnStartup]
	public static class SkillUI
	{
		// Token: 0x04001B11 RID: 6929
		private static float levelLabelWidth = -1f;

		// Token: 0x04001B12 RID: 6930
		private const float SkillWidth = 240f;

		// Token: 0x04001B13 RID: 6931
		public const float SkillHeight = 24f;

		// Token: 0x04001B14 RID: 6932
		public const float SkillYSpacing = 3f;

		// Token: 0x04001B15 RID: 6933
		private const float LeftEdgeMargin = 6f;

		// Token: 0x04001B16 RID: 6934
		private const float IncButX = 205f;

		// Token: 0x04001B17 RID: 6935
		private const float IncButSpacing = 10f;

		// Token: 0x04001B18 RID: 6936
		private static readonly Color DisabledSkillColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04001B19 RID: 6937
		private static Texture2D PassionMinorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMinor", true);

		// Token: 0x04001B1A RID: 6938
		private static Texture2D PassionMajorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMajor", true);

		// Token: 0x04001B1B RID: 6939
		private static Texture2D SkillBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x04001B1C RID: 6940
		private static List<SkillDef> skillDefsInListOrderCached = null;

		// Token: 0x06003294 RID: 12948 RVA: 0x001B3C9C File Offset: 0x001B209C
		public static void DrawSkillsOf(Pawn p, Vector2 offset, SkillUI.SkillDrawMode mode)
		{
			Text.Font = GameFont.Small;
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				float x = Text.CalcSize(allDefsListForReading[i].skillLabel.CapitalizeFirst()).x;
				if (x > SkillUI.levelLabelWidth)
				{
					SkillUI.levelLabelWidth = x;
				}
			}
			if (SkillUI.skillDefsInListOrderCached == null)
			{
				SkillUI.skillDefsInListOrderCached = (from sd in DefDatabase<SkillDef>.AllDefs
				orderby sd.listOrder descending
				select sd).ToList<SkillDef>();
			}
			for (int j = 0; j < SkillUI.skillDefsInListOrderCached.Count; j++)
			{
				SkillDef skillDef = SkillUI.skillDefsInListOrderCached[j];
				float y = (float)j * 27f + offset.y;
				SkillUI.DrawSkill(p.skills.GetSkill(skillDef), new Vector2(offset.x, y), mode, "");
			}
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x001B3DA2 File Offset: 0x001B21A2
		public static void DrawSkill(SkillRecord skill, Vector2 topLeft, SkillUI.SkillDrawMode mode, string tooltipPrefix = "")
		{
			SkillUI.DrawSkill(skill, new Rect(topLeft.x, topLeft.y, 240f, 24f), mode, "");
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x001B3DD0 File Offset: 0x001B21D0
		public static void DrawSkill(SkillRecord skill, Rect holdingRect, SkillUI.SkillDrawMode mode, string tooltipPrefix = "")
		{
			if (Mouse.IsOver(holdingRect))
			{
				GUI.DrawTexture(holdingRect, TexUI.HighlightTex);
			}
			GUI.BeginGroup(holdingRect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect = new Rect(6f, 0f, SkillUI.levelLabelWidth + 6f, holdingRect.height);
			Widgets.Label(rect, skill.def.skillLabel.CapitalizeFirst());
			Rect position = new Rect(rect.xMax, 0f, 24f, 24f);
			if (skill.passion > Passion.None)
			{
				Texture2D image = (skill.passion != Passion.Major) ? SkillUI.PassionMinorIcon : SkillUI.PassionMajorIcon;
				GUI.DrawTexture(position, image);
			}
			if (!skill.TotallyDisabled)
			{
				Rect rect2 = new Rect(position.xMax, 0f, holdingRect.width - position.xMax, holdingRect.height);
				float fillPercent = Mathf.Max(0.01f, (float)skill.Level / 20f);
				Widgets.FillableBar(rect2, fillPercent, SkillUI.SkillBarFillTex, null, false);
			}
			Rect rect3 = new Rect(position.xMax + 4f, 0f, 999f, holdingRect.height);
			rect3.yMin += 3f;
			string label;
			if (skill.TotallyDisabled)
			{
				GUI.color = SkillUI.DisabledSkillColor;
				label = "-";
			}
			else
			{
				label = skill.Level.ToStringCached();
			}
			GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
			Widgets.Label(rect3, label);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
			GUI.EndGroup();
			string text = SkillUI.GetSkillDescription(skill);
			if (tooltipPrefix != "")
			{
				text = tooltipPrefix + "\n\n" + text;
			}
			TooltipHandler.TipRegion(holdingRect, new TipSignal(text, skill.def.GetHashCode() * 397945));
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x001B3FB8 File Offset: 0x001B23B8
		private static string GetSkillDescription(SkillRecord sk)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (sk.TotallyDisabled)
			{
				stringBuilder.Append("DisabledLower".Translate().CapitalizeFirst());
			}
			else
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"Level".Translate(),
					" ",
					sk.Level,
					": ",
					sk.LevelDescriptor
				}));
				if (Current.ProgramState == ProgramState.Playing)
				{
					string text = (sk.Level != 20) ? "ProgressToNextLevel".Translate() : "Experience".Translate();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						text,
						": ",
						sk.xpSinceLastLevel.ToString("F0"),
						" / ",
						sk.XpRequiredForLevelUp
					}));
				}
				stringBuilder.Append("Passion".Translate() + ": ");
				Passion passion = sk.passion;
				if (passion != Passion.None)
				{
					if (passion != Passion.Minor)
					{
						if (passion == Passion.Major)
						{
							stringBuilder.Append("PassionMajor".Translate(new object[]
							{
								1.5f.ToStringPercent("F0")
							}));
						}
					}
					else
					{
						stringBuilder.Append("PassionMinor".Translate(new object[]
						{
							1f.ToStringPercent("F0")
						}));
					}
				}
				else
				{
					stringBuilder.Append("PassionNone".Translate(new object[]
					{
						0.35f.ToStringPercent("F0")
					}));
				}
				if (sk.LearningSaturatedToday)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("LearnedMaxToday".Translate(new object[]
					{
						sk.xpSinceMidnight,
						4000,
						0.2f.ToStringPercent("F0")
					}));
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append(sk.def.description);
			return stringBuilder.ToString();
		}

		// Token: 0x020008A2 RID: 2210
		public enum SkillDrawMode : byte
		{
			// Token: 0x04001B1F RID: 6943
			Gameplay,
			// Token: 0x04001B20 RID: 6944
			Menu
		}
	}
}
