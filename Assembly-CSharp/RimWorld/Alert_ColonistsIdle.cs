using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class Alert_ColonistsIdle : Alert
	{
		public const int MinDaysPassed = 1;

		public Alert_ColonistsIdle()
		{
		}

		private IEnumerable<Pawn> IdleColonists
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						foreach (Pawn p in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (p.mindState.IsIdle)
							{
								yield return p;
							}
						}
					}
				}
				yield break;
			}
		}

		public override string GetLabel()
		{
			return string.Format("ColonistsIdle".Translate(), this.IdleColonists.Count<Pawn>().ToStringCached());
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.IdleColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("ColonistsIdleDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 1)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.IdleColonists);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__0;

			internal int <i>__1;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__2;

			internal Pawn $current;

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
				bool flag = false;
				switch (num)
				{
				case 0u:
					maps = Find.Maps;
					i = 0;
					goto IL_115;
				case 1u:
					Block_3:
					try
					{
						switch (num)
						{
						}
						while (enumerator.MoveNext())
						{
							p = enumerator.Current;
							if (p.mindState.IsIdle)
							{
								this.$current = p;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					break;
				default:
					return false;
				}
				IL_107:
				i++;
				IL_115:
				if (i >= maps.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (maps[i].IsPlayerHome)
					{
						enumerator = maps[i].mapPawns.FreeColonistsSpawned.GetEnumerator();
						num = 4294967293u;
						goto Block_3;
					}
					goto IL_107;
				}
				return false;
			}

			Pawn IEnumerator<Pawn>.Current
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
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
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
				return this.System.Collections.Generic.IEnumerable<Verse.Pawn>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Alert_ColonistsIdle.<>c__Iterator0();
			}
		}
	}
}
