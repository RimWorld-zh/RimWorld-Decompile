using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		public Alert_PasteDispenserNeedsHopper()
		{
			this.defaultLabel = "NeedFoodHopper".Translate();
			this.defaultExplanation = "NeedFoodHopperDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		private IEnumerable<Thing> BadDispensers
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Building disp in maps[i].listerBuildings.allBuildingsColonist)
					{
						if (disp.def.IsFoodDispenser)
						{
							bool foundHopper = false;
							ThingDef hopperDef = ThingDefOf.Hopper;
							foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(disp))
							{
								if (c.InBounds(maps[i]))
								{
									Thing building = c.GetEdifice(disp.Map);
									if (building != null && building.def == hopperDef)
									{
										foundHopper = true;
										break;
									}
								}
							}
							if (!foundHopper)
							{
								yield return disp;
							}
						}
					}
				}
				yield break;
			}
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadDispensers);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal List<Map> <maps>__0;

			internal int <i>__1;

			internal List<Building>.Enumerator $locvar0;

			internal Building <disp>__2;

			internal bool <foundHopper>__3;

			internal ThingDef <hopperDef>__3;

			internal IEnumerator<IntVec3> $locvar1;

			internal Thing <building>__4;

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
				bool flag = false;
				switch (num)
				{
				case 0u:
					maps = Find.Maps;
					i = 0;
					break;
				case 1u:
					Block_2:
					try
					{
						switch (num)
						{
						}
						while (enumerator.MoveNext())
						{
							disp = enumerator.Current;
							if (disp.def.IsFoodDispenser)
							{
								foundHopper = false;
								hopperDef = ThingDefOf.Hopper;
								enumerator2 = GenAdj.CellsAdjacentCardinal(disp).GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										IntVec3 c = enumerator2.Current;
										if (c.InBounds(maps[i]))
										{
											building = c.GetEdifice(disp.Map);
											if (building != null && building.def == hopperDef)
											{
												foundHopper = true;
												break;
											}
										}
									}
								}
								finally
								{
									if (enumerator2 != null)
									{
										enumerator2.Dispose();
									}
								}
								if (!foundHopper)
								{
									this.$current = disp;
									if (!this.$disposing)
									{
										this.$PC = 1;
									}
									flag = true;
									return true;
								}
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
					i++;
					break;
				default:
					return false;
				}
				if (i < maps.Count)
				{
					enumerator = maps[i].listerBuildings.allBuildingsColonist.GetEnumerator();
					num = 4294967293u;
					goto Block_2;
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
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Alert_PasteDispenserNeedsHopper.<>c__Iterator0();
			}
		}
	}
}
