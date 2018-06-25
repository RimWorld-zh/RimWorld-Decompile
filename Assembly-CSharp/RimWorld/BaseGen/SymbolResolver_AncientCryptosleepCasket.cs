using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A6 RID: 934
	public class SymbolResolver_AncientCryptosleepCasket : SymbolResolver
	{
		// Token: 0x0600103A RID: 4154 RVA: 0x00088924 File Offset: 0x00086D24
		public override void Resolve(ResolveParams rp)
		{
			int? ancientCryptosleepCasketGroupID = rp.ancientCryptosleepCasketGroupID;
			int groupID = (ancientCryptosleepCasketGroupID == null) ? Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID() : ancientCryptosleepCasketGroupID.Value;
			PodContentsType? podContentsType = rp.podContentsType;
			PodContentsType value = (podContentsType == null) ? Gen.RandomEnumValue<PodContentsType>(true) : podContentsType.Value;
			Rot4? thingRot = rp.thingRot;
			Rot4 rot = (thingRot == null) ? Rot4.North : thingRot.Value;
			Building_AncientCryptosleepCasket building_AncientCryptosleepCasket = (Building_AncientCryptosleepCasket)ThingMaker.MakeThing(ThingDefOf.AncientCryptosleepCasket, null);
			building_AncientCryptosleepCasket.groupID = groupID;
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.podContentsType = new PodContentsType?(value);
			List<Thing> list = ThingSetMakerDefOf.MapGen_AncientPodContents.root.Generate(parms);
			for (int i = 0; i < list.Count; i++)
			{
				if (!building_AncientCryptosleepCasket.TryAcceptThing(list[i], false))
				{
					Pawn pawn = list[i] as Pawn;
					if (pawn != null)
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					else
					{
						list[i].Destroy(DestroyMode.Vanish);
					}
				}
			}
			GenSpawn.Spawn(building_AncientCryptosleepCasket, rp.rect.RandomCell, BaseGen.globalSettings.map, rot, WipeMode.Vanish, false);
		}
	}
}
