using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class Alert_BilliardsTableOnWall : Alert
	{
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		private IEnumerable<Thing> BadTables
		{
			get
			{
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> bList = maps[i].listerThings.ThingsOfDef(ThingDefOf.BilliardsTable);
					for (int j = 0; j < bList.Count; j++)
					{
						if (bList[j].Faction == ofPlayer && !JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(bList[j]))
						{
							yield return bList[j];
						}
					}
				}
				yield break;
			}
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal List<Map> <maps>__0;

			internal Faction <ofPlayer>__0;

			internal int <i>__1;

			internal List<Thing> <bList>__2;

			internal int <j>__3;

			internal Thing $current;

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
					maps = Find.Maps;
					ofPlayer = Faction.OfPlayer;
					i = 0;
					goto IL_113;
				case 1u:
					IL_DF:
					j++;
					break;
				default:
					return false;
				}
				IL_EE:
				if (j >= bList.Count)
				{
					i++;
				}
				else
				{
					if (bList[j].Faction == ofPlayer && !JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(bList[j]))
					{
						this.$current = bList[j];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_DF;
				}
				IL_113:
				if (i < maps.Count)
				{
					bList = maps[i].listerThings.ThingsOfDef(ThingDefOf.BilliardsTable);
					j = 0;
					goto IL_EE;
				}
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Alert_BilliardsTableOnWall.<>c__Iterator0();
			}
		}
	}
}
