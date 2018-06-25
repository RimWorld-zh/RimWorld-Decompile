using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000672 RID: 1650
	public static class MinifyUtility
	{
		// Token: 0x060022A2 RID: 8866 RVA: 0x0012AC48 File Offset: 0x00129048
		public static MinifiedThing MakeMinified(this Thing thing)
		{
			MinifiedThing result;
			if (!thing.def.Minifiable)
			{
				Log.Warning("Tried to minify " + thing + " which is not minifiable.", false);
				result = null;
			}
			else
			{
				if (thing.Spawned)
				{
					thing.DeSpawn(DestroyMode.Vanish);
				}
				if (thing.holdingOwner != null)
				{
					Log.Warning("Can't minify thing which is in a ThingOwner because we don't know how to handle it. Remove it from the container first. holder=" + thing.ParentHolder, false);
					result = null;
				}
				else
				{
					Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(thing);
					MinifiedThing minifiedThing = (MinifiedThing)ThingMaker.MakeThing(thing.def.minifiedDef, null);
					minifiedThing.InnerThing = thing;
					if (blueprint_Install != null)
					{
						blueprint_Install.SetThingToInstallFromMinified(minifiedThing);
					}
					if (minifiedThing.InnerThing.stackCount > 1)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Tried to minify ",
							thing.LabelCap,
							" with stack count ",
							minifiedThing.InnerThing.stackCount,
							". Clamped stack count to 1."
						}), false);
						minifiedThing.InnerThing.stackCount = 1;
					}
					result = minifiedThing;
				}
			}
			return result;
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x0012AD60 File Offset: 0x00129160
		public static Thing TryMakeMinified(this Thing thing)
		{
			Thing result;
			if (thing.def.Minifiable)
			{
				result = thing.MakeMinified();
			}
			else
			{
				result = thing;
			}
			return result;
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x0012AD94 File Offset: 0x00129194
		public static Thing GetInnerIfMinified(this Thing outerThing)
		{
			MinifiedThing minifiedThing = outerThing as MinifiedThing;
			Thing result;
			if (minifiedThing != null)
			{
				result = minifiedThing.InnerThing;
			}
			else
			{
				result = outerThing;
			}
			return result;
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x0012ADC4 File Offset: 0x001291C4
		public static MinifiedThing Uninstall(this Thing th)
		{
			MinifiedThing result;
			if (!th.Spawned)
			{
				Log.Warning("Can't uninstall unspawned thing " + th, false);
				result = null;
			}
			else
			{
				Map map = th.Map;
				MinifiedThing minifiedThing = th.MakeMinified();
				GenPlace.TryPlaceThing(minifiedThing, th.Position, map, ThingPlaceMode.Near, null, null);
				SoundDefOf.ThingUninstalled.PlayOneShot(new TargetInfo(th.Position, map, false));
				result = minifiedThing;
			}
			return result;
		}
	}
}
