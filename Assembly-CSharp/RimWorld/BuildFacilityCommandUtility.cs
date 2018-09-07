using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public static class BuildFacilityCommandUtility
	{
		public static IEnumerable<Command> BuildFacilityCommands(BuildableDef building)
		{
			ThingDef thingDef = building as ThingDef;
			if (thingDef == null)
			{
				yield break;
			}
			CompProperties_AffectedByFacilities affectedByFacilities = thingDef.GetCompProperties<CompProperties_AffectedByFacilities>();
			if (affectedByFacilities == null)
			{
				yield break;
			}
			for (int i = 0; i < affectedByFacilities.linkableFacilities.Count; i++)
			{
				ThingDef facility = affectedByFacilities.linkableFacilities[i];
				Designator_Build des = BuildCopyCommandUtility.FindAllowedDesignator(facility, true);
				if (des != null)
				{
					yield return des;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <BuildFacilityCommands>c__Iterator0 : IEnumerable, IEnumerable<Command>, IEnumerator, IDisposable, IEnumerator<Command>
		{
			internal BuildableDef building;

			internal ThingDef <thingDef>__0;

			internal CompProperties_AffectedByFacilities <affectedByFacilities>__0;

			internal int <i>__1;

			internal ThingDef <facility>__2;

			internal Designator_Build <des>__2;

			internal Command $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <BuildFacilityCommands>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					thingDef = (building as ThingDef);
					if (thingDef == null)
					{
						return false;
					}
					affectedByFacilities = thingDef.GetCompProperties<CompProperties_AffectedByFacilities>();
					if (affectedByFacilities == null)
					{
						return false;
					}
					i = 0;
					break;
				case 1u:
					IL_CD:
					i++;
					break;
				default:
					return false;
				}
				if (i >= affectedByFacilities.linkableFacilities.Count)
				{
					this.$PC = -1;
				}
				else
				{
					facility = affectedByFacilities.linkableFacilities[i];
					des = BuildCopyCommandUtility.FindAllowedDesignator(facility, true);
					if (des == null)
					{
						goto IL_CD;
					}
					this.$current = des;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				return false;
			}

			Command IEnumerator<Command>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Command>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Command> IEnumerable<Command>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				BuildFacilityCommandUtility.<BuildFacilityCommands>c__Iterator0 <BuildFacilityCommands>c__Iterator = new BuildFacilityCommandUtility.<BuildFacilityCommands>c__Iterator0();
				<BuildFacilityCommands>c__Iterator.building = building;
				return <BuildFacilityCommands>c__Iterator;
			}
		}
	}
}
