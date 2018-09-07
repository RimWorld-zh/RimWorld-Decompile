using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class Alert_Boredom : Alert
	{
		private const float JoyNeedThreshold = 0.24000001f;

		public Alert_Boredom()
		{
			this.defaultLabel = "Boredom".Translate();
			this.defaultPriority = AlertPriority.Medium;
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BoredPawns());
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pawn pawn = null;
			foreach (Pawn pawn2 in this.BoredPawns())
			{
				stringBuilder.AppendLine("   " + pawn2.Label);
				if (pawn == null)
				{
					pawn = pawn2;
				}
			}
			string text = JoyUtility.JoyKindsOnMapString(pawn.Map);
			return "BoredomDesc".Translate(new object[]
			{
				stringBuilder.ToString().TrimEndNewlines(),
				pawn.LabelShort,
				text
			});
		}

		private IEnumerable<Pawn> BoredPawns()
		{
			foreach (Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if ((p.needs.joy.CurLevelPercentage < 0.24000001f || p.GetTimeAssignment() == TimeAssignmentDefOf.Joy) && p.needs.joy.tolerances.BoredOfAllAvailableJoyKinds(p))
				{
					yield return p;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <BoredPawns>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <BoredPawns>c__Iterator0()
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
					enumerator = PawnsFinder.AllMaps_FreeColonistsSpawned.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if ((p.needs.joy.CurLevelPercentage < 0.24000001f || p.GetTimeAssignment() == TimeAssignmentDefOf.Joy) && p.needs.joy.tolerances.BoredOfAllAvailableJoyKinds(p))
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
				this.$PC = -1;
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
				return new Alert_Boredom.<BoredPawns>c__Iterator0();
			}
		}
	}
}
