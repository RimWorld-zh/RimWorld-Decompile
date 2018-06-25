using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_OnPawnDeathExplode : ScenPart
	{
		private float radius = 5.9f;

		private DamageDef damage;

		private string radiusBuf;

		[CompilerGenerated]
		private static Func<DamageDef, string> <>f__am$cache0;

		public ScenPart_OnPawnDeathExplode()
		{
		}

		public override void Randomize()
		{
			this.radius = (float)Rand.RangeInclusive(3, 8) - 0.1f;
			this.damage = this.PossibleDamageDefs().RandomElement<DamageDef>();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damage, "damage");
		}

		public override string Summary(Scenario scen)
		{
			return "ScenPart_OnPawnDeathExplode".Translate(new object[]
			{
				this.damage.label,
				this.radius.ToString()
			});
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect.TopHalf(), "radius".Translate(), ref this.radius, ref this.radiusBuf, 0f, 1E+09f);
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.damage.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<DamageDef>(this.PossibleDamageDefs(), (DamageDef d) => d.LabelCap, (DamageDef d) => delegate()
				{
					this.damage = d;
				});
			}
		}

		public override void Notify_PawnDied(Corpse corpse)
		{
			if (corpse.Spawned)
			{
				GenExplosion.DoExplosion(corpse.Position, corpse.Map, this.radius, this.damage, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
		}

		private IEnumerable<DamageDef> PossibleDamageDefs()
		{
			yield return DamageDefOf.Bomb;
			yield return DamageDefOf.Flame;
			yield break;
		}

		[CompilerGenerated]
		private static string <DoEditInterface>m__0(DamageDef d)
		{
			return d.LabelCap;
		}

		[CompilerGenerated]
		private Action <DoEditInterface>m__1(DamageDef d)
		{
			return delegate()
			{
				this.damage = d;
			};
		}

		[CompilerGenerated]
		private sealed class <PossibleDamageDefs>c__Iterator0 : IEnumerable, IEnumerable<DamageDef>, IEnumerator, IDisposable, IEnumerator<DamageDef>
		{
			internal DamageDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PossibleDamageDefs>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = DamageDefOf.Bomb;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = DamageDefOf.Flame;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			DamageDef IEnumerator<DamageDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.DamageDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<DamageDef> IEnumerable<DamageDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new ScenPart_OnPawnDeathExplode.<PossibleDamageDefs>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <DoEditInterface>c__AnonStorey1
		{
			internal DamageDef d;

			internal ScenPart_OnPawnDeathExplode $this;

			public <DoEditInterface>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.$this.damage = this.d;
			}
		}
	}
}
