using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003AF RID: 943
	public class SymbolResolver_EmptyRoom : SymbolResolver
	{
		// Token: 0x06001058 RID: 4184 RVA: 0x00089F74 File Offset: 0x00088374
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.CorrespondingTerrainDef(thingDef, true);
			if (rp.noRoof == null || !rp.noRoof.Value)
			{
				BaseGen.symbolStack.Push("roof", rp);
			}
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = thingDef;
			BaseGen.symbolStack.Push("edgeWalls", resolveParams);
			ResolveParams resolveParams2 = rp;
			resolveParams2.floorDef = floorDef;
			BaseGen.symbolStack.Push("floor", resolveParams2);
			BaseGen.symbolStack.Push("clear", rp);
			if (rp.addRoomCenterToRootsToUnfog != null && rp.addRoomCenterToRootsToUnfog.Value && Current.ProgramState == ProgramState.MapInitializing)
			{
				MapGenerator.rootsToUnfog.Add(rp.rect.CenterCell);
			}
		}
	}
}
