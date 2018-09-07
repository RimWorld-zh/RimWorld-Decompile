using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class Building_AncientCryptosleepCasket : Building_CryptosleepCasket
	{
		public int groupID = -1;

		[CompilerGenerated]
		private static Func<Building_AncientCryptosleepCasket, IEnumerable<Thing>> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache1;

		public Building_AncientCryptosleepCasket()
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
		}

		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (!this.contentsKnown && this.innerContainer.Count > 0 && dinfo.Def.harmsHealth && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				bool flag = false;
				foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.EjectContents();
				}
			}
			absorbed = false;
		}

		public override void EjectContents()
		{
			List<Thing> list = new List<Thing>();
			if (!this.contentsKnown)
			{
				list.AddRange(this.innerContainer);
				list.AddRange(this.UnopenedCasketsInGroup().SelectMany((Building_AncientCryptosleepCasket c) => c.innerContainer));
			}
			bool contentsKnown = this.contentsKnown;
			base.EjectContents();
			if (!contentsKnown)
			{
				ThingDef filth_Slime = ThingDefOf.Filth_Slime;
				FilthMaker.MakeFilth(base.Position, base.Map, filth_Slime, Rand.Range(8, 12));
				this.SetFaction(null, null);
				foreach (Building_AncientCryptosleepCasket building_AncientCryptosleepCasket in this.UnopenedCasketsInGroup())
				{
					building_AncientCryptosleepCasket.EjectContents();
				}
				List<Pawn> source = list.OfType<Pawn>().ToList<Pawn>();
				IEnumerable<Pawn> enumerable = from p in source
				where p.RaceProps.Humanlike && p.GetLord() == null && p.Faction == Faction.OfAncientsHostile
				select p;
				if (enumerable.Any<Pawn>())
				{
					LordMaker.MakeNewLord(Faction.OfAncientsHostile, new LordJob_AssaultColony(Faction.OfAncientsHostile, false, false, false, false, false), base.Map, enumerable);
				}
			}
		}

		private IEnumerable<Building_AncientCryptosleepCasket> UnopenedCasketsInGroup()
		{
			yield return this;
			if (this.groupID != -1)
			{
				foreach (Thing t in base.Map.listerThings.ThingsOfDef(ThingDefOf.AncientCryptosleepCasket))
				{
					Building_AncientCryptosleepCasket casket = t as Building_AncientCryptosleepCasket;
					if (casket.groupID == this.groupID && !casket.contentsKnown)
					{
						yield return casket;
					}
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private static IEnumerable<Thing> <EjectContents>m__0(Building_AncientCryptosleepCasket c)
		{
			return c.innerContainer;
		}

		[CompilerGenerated]
		private static bool <EjectContents>m__1(Pawn p)
		{
			return p.RaceProps.Humanlike && p.GetLord() == null && p.Faction == Faction.OfAncientsHostile;
		}

		[CompilerGenerated]
		private sealed class <UnopenedCasketsInGroup>c__Iterator0 : IEnumerable, IEnumerable<Building_AncientCryptosleepCasket>, IEnumerator, IDisposable, IEnumerator<Building_AncientCryptosleepCasket>
		{
			internal List<Thing>.Enumerator $locvar0;

			internal Thing <t>__1;

			internal Building_AncientCryptosleepCasket <casket>__2;

			internal Building_AncientCryptosleepCasket $this;

			internal Building_AncientCryptosleepCasket $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <UnopenedCasketsInGroup>c__Iterator0()
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
					this.$current = this;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					if (this.groupID == -1)
					{
						goto IL_12B;
					}
					enumerator = base.Map.listerThings.ThingsOfDef(ThingDefOf.AncientCryptosleepCasket).GetEnumerator();
					num = 4294967293u;
					break;
				case 2u:
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
						t = enumerator.Current;
						casket = (t as Building_AncientCryptosleepCasket);
						if (casket.groupID == this.groupID && !casket.contentsKnown)
						{
							this.$current = casket;
							if (!this.$disposing)
							{
								this.$PC = 2;
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
						((IDisposable)enumerator).Dispose();
					}
				}
				IL_12B:
				this.$PC = -1;
				return false;
			}

			Building_AncientCryptosleepCasket IEnumerator<Building_AncientCryptosleepCasket>.Current
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
				case 2u:
					try
					{
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Building_AncientCryptosleepCasket>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Building_AncientCryptosleepCasket> IEnumerable<Building_AncientCryptosleepCasket>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Building_AncientCryptosleepCasket.<UnopenedCasketsInGroup>c__Iterator0 <UnopenedCasketsInGroup>c__Iterator = new Building_AncientCryptosleepCasket.<UnopenedCasketsInGroup>c__Iterator0();
				<UnopenedCasketsInGroup>c__Iterator.$this = this;
				return <UnopenedCasketsInGroup>c__Iterator;
			}
		}
	}
}
