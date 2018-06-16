using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000854 RID: 2132
	public static class ITab_Pawn_Log_Utility
	{
		// Token: 0x06003048 RID: 12360 RVA: 0x001A457C File Offset: 0x001A297C
		public static IEnumerable<ITab_Pawn_Log_Utility.LogLineDisplayable> GenerateLogLinesFor(Pawn pawn, bool showAll, bool showCombat, bool showSocial)
		{
			LogEntry[] nonCombatLines = (!showSocial) ? new LogEntry[0] : (from e in Find.PlayLog.AllEntries
			where e.Concerns(pawn)
			select e).ToArray<LogEntry>();
			int nonCombatIndex = 0;
			Battle currentBattle = null;
			if (showCombat)
			{
				bool atTop = true;
				foreach (Battle battle in Find.BattleLog.Battles)
				{
					if (battle.Concerns(pawn))
					{
						foreach (LogEntry entry in battle.Entries)
						{
							if (entry.Concerns(pawn) && (showAll || entry.ShowInCompactView()))
							{
								while (nonCombatIndex < nonCombatLines.Length && nonCombatLines[nonCombatIndex].Age < entry.Age)
								{
									if (currentBattle != null && currentBattle != battle)
									{
										yield return new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
										currentBattle = null;
									}
									LogEntry[] array = nonCombatLines;
									int num;
									nonCombatIndex = (num = nonCombatIndex) + 1;
									yield return new ITab_Pawn_Log_Utility.LogLineDisplayableLog(array[num], pawn);
									atTop = false;
								}
								if (currentBattle != battle)
								{
									if (!atTop)
									{
										yield return new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
									}
									yield return new ITab_Pawn_Log_Utility.LogLineDisplayableHeader(battle.GetName());
									currentBattle = battle;
									atTop = false;
								}
								yield return new ITab_Pawn_Log_Utility.LogLineDisplayableLog(entry, pawn);
							}
						}
					}
				}
			}
			while (nonCombatIndex < nonCombatLines.Length)
			{
				if (currentBattle != null)
				{
					yield return new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
					currentBattle = null;
				}
				LogEntry[] array2 = nonCombatLines;
				int num;
				nonCombatIndex = (num = nonCombatIndex) + 1;
				yield return new ITab_Pawn_Log_Utility.LogLineDisplayableLog(array2[num], pawn);
			}
			yield break;
		}

		// Token: 0x04001A2F RID: 6703
		[TweakValue("Interface", 0f, 1f)]
		private static float AlternateAlpha = 0.03f;

		// Token: 0x04001A30 RID: 6704
		[TweakValue("Interface", 0f, 1f)]
		private static float HighlightAlpha = 0.2f;

		// Token: 0x04001A31 RID: 6705
		[TweakValue("Interface", 0f, 10f)]
		private static float HighlightDuration = 4f;

		// Token: 0x04001A32 RID: 6706
		[TweakValue("Interface", 0f, 30f)]
		private static float BattleBottomPadding = 20f;

		// Token: 0x02000855 RID: 2133
		public class LogDrawData
		{
			// Token: 0x0600304B RID: 12363 RVA: 0x001A4606 File Offset: 0x001A2A06
			public void StartNewDraw()
			{
				this.alternatingBackground = false;
			}

			// Token: 0x04001A33 RID: 6707
			public bool alternatingBackground = false;

			// Token: 0x04001A34 RID: 6708
			public LogEntry highlightEntry = null;

			// Token: 0x04001A35 RID: 6709
			public float highlightIntensity = 0f;
		}

		// Token: 0x02000856 RID: 2134
		public abstract class LogLineDisplayable
		{
			// Token: 0x0600304D RID: 12365 RVA: 0x001A4624 File Offset: 0x001A2A24
			public float GetHeight(float width)
			{
				if (this.cachedHeight == -1f)
				{
					this.cachedHeight = this.GetHeight_Worker(width);
				}
				return this.cachedHeight;
			}

			// Token: 0x0600304E RID: 12366
			public abstract float GetHeight_Worker(float width);

			// Token: 0x0600304F RID: 12367
			public abstract void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data);

			// Token: 0x06003050 RID: 12368
			public abstract void AppendTo(StringBuilder sb);

			// Token: 0x06003051 RID: 12369 RVA: 0x001A465C File Offset: 0x001A2A5C
			public virtual bool Matches(LogEntry log)
			{
				return false;
			}

			// Token: 0x04001A36 RID: 6710
			private float cachedHeight = -1f;
		}

		// Token: 0x02000857 RID: 2135
		public class LogLineDisplayableHeader : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x06003052 RID: 12370 RVA: 0x001A4672 File Offset: 0x001A2A72
			public LogLineDisplayableHeader(string text)
			{
				this.text = text;
			}

			// Token: 0x06003053 RID: 12371 RVA: 0x001A4684 File Offset: 0x001A2A84
			public override float GetHeight_Worker(float width)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = Text.CalcHeight(this.text, width);
				Text.Font = font;
				return result;
			}

			// Token: 0x06003054 RID: 12372 RVA: 0x001A46B9 File Offset: 0x001A2AB9
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, position, width, base.GetHeight(width)), this.text);
				Text.Font = GameFont.Small;
			}

			// Token: 0x06003055 RID: 12373 RVA: 0x001A46E6 File Offset: 0x001A2AE6
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine("--    " + this.text);
			}

			// Token: 0x04001A37 RID: 6711
			private string text;
		}

		// Token: 0x02000858 RID: 2136
		public class LogLineDisplayableLog : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x06003056 RID: 12374 RVA: 0x001A4700 File Offset: 0x001A2B00
			public LogLineDisplayableLog(LogEntry log, Pawn pawn)
			{
				this.log = log;
				this.pawn = pawn;
			}

			// Token: 0x06003057 RID: 12375 RVA: 0x001A4718 File Offset: 0x001A2B18
			public override float GetHeight_Worker(float width)
			{
				float width2 = width - 29f;
				return Mathf.Max(26f, this.log.GetTextHeight(this.pawn, width2));
			}

			// Token: 0x06003058 RID: 12376 RVA: 0x001A4754 File Offset: 0x001A2B54
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				float height = base.GetHeight(width);
				float width2 = width - 29f;
				Rect rect = new Rect(0f, position, width, height);
				if (this.log == data.highlightEntry)
				{
					Widgets.DrawRectFast(rect, new Color(1f, 1f, 1f, ITab_Pawn_Log_Utility.HighlightAlpha * data.highlightIntensity), null);
					data.highlightIntensity = Mathf.Max(0f, data.highlightIntensity - Time.deltaTime / ITab_Pawn_Log_Utility.HighlightDuration);
				}
				else if (data.alternatingBackground)
				{
					Widgets.DrawRectFast(rect, new Color(1f, 1f, 1f, ITab_Pawn_Log_Utility.AlternateAlpha), null);
				}
				data.alternatingBackground = !data.alternatingBackground;
				Widgets.Label(new Rect(29f, position, width2, height), this.log.ToGameStringFromPOV(this.pawn, false));
				Texture2D texture2D = this.log.IconFromPOV(this.pawn);
				if (texture2D != null)
				{
					Rect position2 = new Rect(0f, position + (height - 26f) / 2f, 26f, 26f);
					GUI.DrawTexture(position2, texture2D);
				}
				Widgets.DrawHighlightIfMouseover(rect);
				TooltipHandler.TipRegion(rect, () => this.log.GetTipString(), 613261 + this.log.LogID * 2063);
				if (Widgets.ButtonInvisible(rect, false))
				{
					this.log.ClickedFromPOV(this.pawn);
				}
				if (DebugViewSettings.logCombatLogMouseover && Mouse.IsOver(rect))
				{
					this.log.ToGameStringFromPOV(this.pawn, true);
				}
			}

			// Token: 0x06003059 RID: 12377 RVA: 0x001A4901 File Offset: 0x001A2D01
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine(this.log.ToGameStringFromPOV(this.pawn, false));
			}

			// Token: 0x0600305A RID: 12378 RVA: 0x001A4920 File Offset: 0x001A2D20
			public override bool Matches(LogEntry log)
			{
				return log == this.log;
			}

			// Token: 0x04001A38 RID: 6712
			private LogEntry log;

			// Token: 0x04001A39 RID: 6713
			private Pawn pawn;
		}

		// Token: 0x02000859 RID: 2137
		public class LogLineDisplayableGap : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0600305C RID: 12380 RVA: 0x001A495F File Offset: 0x001A2D5F
			public LogLineDisplayableGap(float height)
			{
				this.height = height;
			}

			// Token: 0x0600305D RID: 12381 RVA: 0x001A4970 File Offset: 0x001A2D70
			public override float GetHeight_Worker(float width)
			{
				return this.height;
			}

			// Token: 0x0600305E RID: 12382 RVA: 0x001A498B File Offset: 0x001A2D8B
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
			}

			// Token: 0x0600305F RID: 12383 RVA: 0x001A498E File Offset: 0x001A2D8E
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine();
			}

			// Token: 0x04001A3A RID: 6714
			private float height;
		}
	}
}
