using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ITab_Pawn_Log_Utility
	{
		[TweakValue("Interface", 0f, 1f)]
		private static float AlternateAlpha = 0.03f;

		[TweakValue("Interface", 0f, 1f)]
		private static float HighlightAlpha = 0.2f;

		[TweakValue("Interface", 0f, 10f)]
		private static float HighlightDuration = 4f;

		[TweakValue("Interface", 0f, 30f)]
		private static float BattleBottomPadding = 20f;

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

		// Note: this type is marked as 'beforefieldinit'.
		static ITab_Pawn_Log_Utility()
		{
		}

		public class LogDrawData
		{
			public bool alternatingBackground = false;

			public LogEntry highlightEntry = null;

			public float highlightIntensity = 0f;

			public LogDrawData()
			{
			}

			public void StartNewDraw()
			{
				this.alternatingBackground = false;
			}
		}

		public abstract class LogLineDisplayable
		{
			private float cachedHeight = -1f;

			protected LogLineDisplayable()
			{
			}

			public float GetHeight(float width)
			{
				if (this.cachedHeight == -1f)
				{
					this.cachedHeight = this.GetHeight_Worker(width);
				}
				return this.cachedHeight;
			}

			public abstract float GetHeight_Worker(float width);

			public abstract void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data);

			public abstract void AppendTo(StringBuilder sb);

			public virtual bool Matches(LogEntry log)
			{
				return false;
			}
		}

		public class LogLineDisplayableHeader : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			private string text;

			public LogLineDisplayableHeader(string text)
			{
				this.text = text;
			}

			public override float GetHeight_Worker(float width)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = Text.CalcHeight(this.text, width);
				Text.Font = font;
				return result;
			}

			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, position, width, base.GetHeight(width)), this.text);
				Text.Font = GameFont.Small;
			}

			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine("--    " + this.text);
			}
		}

		public class LogLineDisplayableLog : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			private LogEntry log;

			private Pawn pawn;

			public LogLineDisplayableLog(LogEntry log, Pawn pawn)
			{
				this.log = log;
				this.pawn = pawn;
			}

			public override float GetHeight_Worker(float width)
			{
				float width2 = width - 29f;
				return Mathf.Max(26f, this.log.GetTextHeight(this.pawn, width2));
			}

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

			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine(this.log.ToGameStringFromPOV(this.pawn, false));
			}

			public override bool Matches(LogEntry log)
			{
				return log == this.log;
			}

			[CompilerGenerated]
			private string <Draw>m__0()
			{
				return this.log.GetTipString();
			}
		}

		public class LogLineDisplayableGap : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			private float height;

			public LogLineDisplayableGap(float height)
			{
				this.height = height;
			}

			public override float GetHeight_Worker(float width)
			{
				return this.height;
			}

			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
			}

			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine();
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateLogLinesFor>c__Iterator0 : IEnumerable, IEnumerable<ITab_Pawn_Log_Utility.LogLineDisplayable>, IEnumerator, IDisposable, IEnumerator<ITab_Pawn_Log_Utility.LogLineDisplayable>
		{
			internal bool showSocial;

			internal Pawn pawn;

			internal LogEntry[] <nonCombatLines>__0;

			internal int <nonCombatIndex>__0;

			internal Battle <currentBattle>__0;

			internal bool showCombat;

			internal bool <atTop>__1;

			internal List<Battle>.Enumerator $locvar0;

			internal Battle <battle>__2;

			internal List<LogEntry>.Enumerator $locvar1;

			internal LogEntry <entry>__3;

			internal bool showAll;

			internal ITab_Pawn_Log_Utility.LogLineDisplayable $current;

			internal bool $disposing;

			internal int $PC;

			private ITab_Pawn_Log_Utility.<GenerateLogLinesFor>c__Iterator0.<GenerateLogLinesFor>c__AnonStorey1 $locvar2;

			[DebuggerHidden]
			public <GenerateLogLinesFor>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					nonCombatLines = ((!showSocial) ? new LogEntry[0] : (from e in Find.PlayLog.AllEntries
					where e.Concerns(pawn)
					select e).ToArray<LogEntry>());
					nonCombatIndex = 0;
					currentBattle = null;
					if (!showCombat)
					{
						goto IL_39A;
					}
					atTop = true;
					enumerator = Find.BattleLog.Battles.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
				case 2u:
				case 3u:
				case 4u:
				case 5u:
					break;
				case 6u:
					currentBattle = null;
					goto IL_3D8;
				case 7u:
					goto IL_41B;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
					case 2u:
					case 3u:
					case 4u:
					case 5u:
						Block_11:
						try
						{
							switch (num)
							{
							case 1u:
								currentBattle = null;
								goto IL_20B;
							case 2u:
								atTop = false;
								goto IL_257;
							case 3u:
								IL_2CF:
								this.$current = new ITab_Pawn_Log_Utility.LogLineDisplayableHeader(battle.GetName());
								if (!this.$disposing)
								{
									this.$PC = 4;
								}
								flag = true;
								return true;
							case 4u:
								currentBattle = battle;
								atTop = false;
								goto IL_30F;
							}
							while (enumerator2.MoveNext())
							{
								entry = enumerator2.Current;
								if (entry.Concerns(<GenerateLogLinesFor>c__AnonStorey.pawn) && (showAll || entry.ShowInCompactView()))
								{
									goto IL_257;
								}
							}
							break;
							IL_20B:
							this.$current = new ITab_Pawn_Log_Utility.LogLineDisplayableLog(nonCombatLines[nonCombatIndex++], <GenerateLogLinesFor>c__AnonStorey.pawn);
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
							IL_257:
							if (nonCombatIndex >= nonCombatLines.Length || nonCombatLines[nonCombatIndex].Age >= entry.Age)
							{
								if (currentBattle != battle)
								{
									if (!atTop)
									{
										this.$current = new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
										if (!this.$disposing)
										{
											this.$PC = 3;
										}
										flag = true;
										return true;
									}
									goto IL_2CF;
								}
							}
							else
							{
								if (currentBattle != null && currentBattle != battle)
								{
									this.$current = new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
									if (!this.$disposing)
									{
										this.$PC = 1;
									}
									flag = true;
									return true;
								}
								goto IL_20B;
							}
							IL_30F:
							this.$current = new ITab_Pawn_Log_Utility.LogLineDisplayableLog(entry, <GenerateLogLinesFor>c__AnonStorey.pawn);
							if (!this.$disposing)
							{
								this.$PC = 5;
							}
							flag = true;
							return true;
						}
						finally
						{
							if (!flag)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						break;
					}
					while (enumerator.MoveNext())
					{
						battle = enumerator.Current;
						if (battle.Concerns(<GenerateLogLinesFor>c__AnonStorey.pawn))
						{
							enumerator2 = battle.Entries.GetEnumerator();
							num = 4294967293u;
							goto Block_11;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				IL_39A:
				goto IL_41B;
				IL_3D8:
				this.$current = new ITab_Pawn_Log_Utility.LogLineDisplayableLog(nonCombatLines[nonCombatIndex++], <GenerateLogLinesFor>c__AnonStorey.pawn);
				if (!this.$disposing)
				{
					this.$PC = 7;
				}
				return true;
				IL_41B:
				if (nonCombatIndex >= nonCombatLines.Length)
				{
					this.$PC = -1;
				}
				else
				{
					if (currentBattle != null)
					{
						this.$current = new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
						if (!this.$disposing)
						{
							this.$PC = 6;
						}
						return true;
					}
					goto IL_3D8;
				}
				return false;
			}

			ITab_Pawn_Log_Utility.LogLineDisplayable IEnumerator<ITab_Pawn_Log_Utility.LogLineDisplayable>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
				case 2u:
				case 3u:
				case 4u:
				case 5u:
					try
					{
						try
						{
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.ITab_Pawn_Log_Utility.LogLineDisplayable>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ITab_Pawn_Log_Utility.LogLineDisplayable> IEnumerable<ITab_Pawn_Log_Utility.LogLineDisplayable>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ITab_Pawn_Log_Utility.<GenerateLogLinesFor>c__Iterator0 <GenerateLogLinesFor>c__Iterator = new ITab_Pawn_Log_Utility.<GenerateLogLinesFor>c__Iterator0();
				<GenerateLogLinesFor>c__Iterator.showSocial = showSocial;
				<GenerateLogLinesFor>c__Iterator.pawn = pawn;
				<GenerateLogLinesFor>c__Iterator.showCombat = showCombat;
				<GenerateLogLinesFor>c__Iterator.showAll = showAll;
				return <GenerateLogLinesFor>c__Iterator;
			}

			private sealed class <GenerateLogLinesFor>c__AnonStorey1
			{
				internal Pawn pawn;

				internal ITab_Pawn_Log_Utility.<GenerateLogLinesFor>c__Iterator0 <>f__ref$0;

				public <GenerateLogLinesFor>c__AnonStorey1()
				{
				}

				internal bool <>m__0(LogEntry e)
				{
					return e.Concerns(this.pawn);
				}
			}
		}
	}
}
