using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	public class DeathLetter : ChoiceLetter
	{
		[CompilerGenerated]
		private static Func<LogEntry, int> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<LogEntry, string> <>f__am$cache1;

		public DeathLetter()
		{
		}

		protected DiaOption ReadMore
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("ReadMore".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target);
					Find.LetterStack.RemoveLetter(this);
					InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Log));
				};
				diaOption.resolveTree = true;
				if (!target.IsValid)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.Option_Close;
				if (this.lookTargets.IsValid())
				{
					yield return this.ReadMore;
				}
				yield break;
			}
		}

		public override void OpenLetter()
		{
			Pawn targetPawn = this.lookTargets.TryGetPrimaryTarget().Thing as Pawn;
			string text = this.text;
			string text2 = (from entry in (from entry in (from battle in Find.BattleLog.Battles
			where battle.Concerns(targetPawn)
			select battle).SelectMany((Battle battle) => from entry in battle.Entries
			where entry.Concerns(targetPawn) && entry.ShowInCompactView()
			select entry)
			orderby entry.Age
			select entry).Take(5).Reverse<LogEntry>()
			select "  " + entry.ToGameStringFromPOV(null, false)).ToLineList("");
			if (text2.Length > 0)
			{
				text = string.Format("{0}\n\n{1}\n{2}", text, "LastEventsInLife".Translate(new object[]
				{
					targetPawn.LabelDefinite()
				}) + ":", text2);
			}
			DiaNode diaNode = new DiaNode(text);
			diaNode.options.AddRange(this.Choices);
			WindowStack windowStack = Find.WindowStack;
			DiaNode nodeRoot = diaNode;
			Faction relatedFaction = this.relatedFaction;
			bool radioMode = this.radioMode;
			windowStack.Add(new Dialog_NodeTreeWithFactionInfo(nodeRoot, relatedFaction, false, radioMode, this.title));
		}

		[CompilerGenerated]
		private static int <OpenLetter>m__0(LogEntry entry)
		{
			return entry.Age;
		}

		[CompilerGenerated]
		private static string <OpenLetter>m__1(LogEntry entry)
		{
			return "  " + entry.ToGameStringFromPOV(null, false);
		}

		[CompilerGenerated]
		private sealed class <>c__AnonStorey1
		{
			internal GlobalTargetInfo target;

			internal DeathLetter $this;

			public <>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				CameraJumper.TryJumpAndSelect(this.target);
				Find.LetterStack.RemoveLetter(this.$this);
				InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Log));
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<DiaOption>, IEnumerator, IDisposable, IEnumerator<DiaOption>
		{
			internal DeathLetter $this;

			internal DiaOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = base.Option_Close;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					if (this.lookTargets.IsValid())
					{
						this.$current = base.ReadMore;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 2u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			DiaOption IEnumerator<DiaOption>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.DiaOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<DiaOption> IEnumerable<DiaOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DeathLetter.<>c__Iterator0 <>c__Iterator = new DeathLetter.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <OpenLetter>c__AnonStorey2
		{
			internal Pawn targetPawn;

			public <OpenLetter>c__AnonStorey2()
			{
			}

			internal bool <>m__0(Battle battle)
			{
				return battle.Concerns(this.targetPawn);
			}

			internal IEnumerable<LogEntry> <>m__1(Battle battle)
			{
				return from entry in battle.Entries
				where entry.Concerns(this.targetPawn) && entry.ShowInCompactView()
				select entry;
			}

			internal bool <>m__2(LogEntry entry)
			{
				return entry.Concerns(this.targetPawn) && entry.ShowInCompactView();
			}
		}
	}
}
