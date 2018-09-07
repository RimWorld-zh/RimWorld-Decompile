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
	public abstract class Alert_Thought : Alert
	{
		protected string explanationKey;

		private static List<Thought> tmpThoughts = new List<Thought>();

		protected Alert_Thought()
		{
		}

		protected abstract ThoughtDef Thought { get; }

		private IEnumerable<Pawn> AffectedPawns()
		{
			foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
			{
				if (p.Dead)
				{
					Log.Error("Dead pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists:" + p, false);
				}
				else
				{
					p.needs.mood.thoughts.GetAllMoodThoughts(Alert_Thought.tmpThoughts);
					try
					{
						ThoughtDef requiredDef = this.Thought;
						for (int i = 0; i < Alert_Thought.tmpThoughts.Count; i++)
						{
							if (Alert_Thought.tmpThoughts[i].def == requiredDef)
							{
								yield return p;
								break;
							}
						}
					}
					finally
					{
						Alert_Thought.tmpThoughts.Clear();
					}
				}
			}
			yield break;
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AffectedPawns());
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AffectedPawns())
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return this.explanationKey.Translate(new object[]
			{
				stringBuilder.ToString()
			});
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Alert_Thought()
		{
		}

		[CompilerGenerated]
		private sealed class <AffectedPawns>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal ThoughtDef <requiredDef>__2;

			internal int <i>__3;

			internal Alert_Thought $this;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AffectedPawns>c__Iterator0()
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
					enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.GetEnumerator();
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
					case 1u:
						Block_5:
						try
						{
							switch (num)
							{
							case 1u:
								break;
							default:
								requiredDef = this.Thought;
								for (i = 0; i < Alert_Thought.tmpThoughts.Count; i++)
								{
									if (Alert_Thought.tmpThoughts[i].def == requiredDef)
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
								break;
							}
						}
						finally
						{
							if (!flag)
							{
								this.<>__Finally0();
							}
						}
						break;
					}
					while (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (!p.Dead)
						{
							p.needs.mood.thoughts.GetAllMoodThoughts(Alert_Thought.tmpThoughts);
							num = 4294967293u;
							goto Block_5;
						}
						Log.Error("Dead pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists:" + p, false);
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
						try
						{
						}
						finally
						{
							this.<>__Finally0();
						}
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
				Alert_Thought.<AffectedPawns>c__Iterator0 <AffectedPawns>c__Iterator = new Alert_Thought.<AffectedPawns>c__Iterator0();
				<AffectedPawns>c__Iterator.$this = this;
				return <AffectedPawns>c__Iterator;
			}

			private void <>__Finally0()
			{
				Alert_Thought.tmpThoughts.Clear();
			}
		}
	}
}
