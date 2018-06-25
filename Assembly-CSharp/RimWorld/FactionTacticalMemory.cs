using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class FactionTacticalMemory : IExposable
	{
		private List<TrapMemory> traps = new List<TrapMemory>();

		private const float TrapRememberChance = 0.2f;

		[CompilerGenerated]
		private static Predicate<TrapMemory> <>f__am$cache0;

		public FactionTacticalMemory()
		{
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<TrapMemory>(ref this.traps, "traps", LookMode.Deep, new object[0]);
		}

		public void Notify_MapRemoved(Map map)
		{
			this.traps.RemoveAll((TrapMemory x) => x.map == map);
		}

		public List<TrapMemory> TrapMemories()
		{
			this.traps.RemoveAll((TrapMemory tl) => tl.Expired);
			return this.traps;
		}

		public void TrapRevealed(IntVec3 c, Map map)
		{
			if (Rand.Value < 0.2f)
			{
				this.traps.Add(new TrapMemory(c, map, Find.TickManager.TicksGame));
			}
		}

		[CompilerGenerated]
		private static bool <TrapMemories>m__0(TrapMemory tl)
		{
			return tl.Expired;
		}

		[CompilerGenerated]
		private sealed class <Notify_MapRemoved>c__AnonStorey0
		{
			internal Map map;

			public <Notify_MapRemoved>c__AnonStorey0()
			{
			}

			internal bool <>m__0(TrapMemory x)
			{
				return x.map == this.map;
			}
		}
	}
}
