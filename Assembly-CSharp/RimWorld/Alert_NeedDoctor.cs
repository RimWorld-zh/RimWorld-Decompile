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
	public class Alert_NeedDoctor : Alert
	{
		public Alert_NeedDoctor()
		{
			this.defaultLabel = "NeedDoctor".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		private IEnumerable<Pawn> Patients
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						bool healthyDoc = false;
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (!pawn.Downed && pawn.workSettings != null && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Doctor))
							{
								healthyDoc = true;
								break;
							}
						}
						if (!healthyDoc)
						{
							foreach (Pawn p in maps[i].mapPawns.FreeColonistsSpawned)
							{
								if ((p.Downed && p.needs.food.CurCategory < HungerCategory.Fed && p.InBed()) || HealthAIUtility.ShouldBeTendedNowByPlayer(p))
								{
									yield return p;
								}
							}
						}
					}
				}
				yield break;
			}
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.Patients)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("NeedDoctorDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			AlertReport result;
			if (Find.AnyPlayerHomeMap == null)
			{
				result = false;
			}
			else
			{
				result = AlertReport.CulpritsAre(this.Patients);
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__0;

			internal int <i>__1;

			internal bool <healthyDoc>__2;

			internal IEnumerator<Pawn> $locvar0;

			internal IEnumerator<Pawn> $locvar1;

			internal Pawn <p>__3;

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
					goto IL_20D;
				case 1u:
					Block_5:
					try
					{
						switch (num)
						{
						}
						IL_1CD:
						if (enumerator2.MoveNext())
						{
							p = enumerator2.Current;
							if ((p.Downed && p.needs.food.CurCategory < HungerCategory.Fed && p.InBed()) || HealthAIUtility.ShouldBeTendedNowByPlayer(p))
							{
								this.$current = p;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
							goto IL_1CD;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					break;
				default:
					return false;
				}
				IL_1FF:
				i++;
				IL_20D:
				if (i >= maps.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (!maps[i].IsPlayerHome)
					{
						goto IL_1FF;
					}
					healthyDoc = false;
					enumerator = maps[i].mapPawns.FreeColonistsSpawned.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Pawn pawn = enumerator.Current;
							if (!pawn.Downed && pawn.workSettings != null && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Doctor))
							{
								healthyDoc = true;
								break;
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					if (healthyDoc)
					{
						goto IL_1FF;
					}
					enumerator2 = maps[i].mapPawns.FreeColonistsSpawned.GetEnumerator();
					num = 4294967293u;
					goto Block_5;
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
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
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
				return new Alert_NeedDoctor.<>c__Iterator0();
			}
		}
	}
}
