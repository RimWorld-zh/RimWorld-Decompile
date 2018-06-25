using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		private IEnumerable<Pawn> ShieldUsersWithRangedWeapon
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon)
					{
						List<Apparel> ap = p.apparel.WornApparel;
						for (int i = 0; i < ap.Count; i++)
						{
							if (ap[i] is ShieldBelt)
							{
								yield return p;
								break;
							}
						}
					}
				}
				yield break;
			}
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal List<Apparel> <ap>__2;

			internal int <i>__3;

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
					IL_11B:
					IL_11C:
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon)
						{
							ap = p.apparel.WornApparel;
							for (i = 0; i < ap.Count; i++)
							{
								if (ap[i] is ShieldBelt)
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
							goto IL_11B;
						}
						goto IL_11C;
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
				return new Alert_ShieldUserHasRangedWeapon.<>c__Iterator0();
			}
		}
	}
}
