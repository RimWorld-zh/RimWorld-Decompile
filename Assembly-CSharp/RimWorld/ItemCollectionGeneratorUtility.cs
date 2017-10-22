using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class ItemCollectionGeneratorUtility
	{
		public static List<ThingDef> allGeneratableItems = new List<ThingDef>();

		public static void Reset()
		{
			ItemCollectionGeneratorUtility.allGeneratableItems.Clear();
			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				if ((allDef.category == ThingCategory.Item || allDef.Minifiable) && !allDef.isUnfinishedThing && !allDef.IsCorpse && allDef.PlayerAcquirable && allDef.graphicData != null && !typeof(MinifiedThing).IsAssignableFrom(allDef.thingClass))
				{
					ItemCollectionGeneratorUtility.allGeneratableItems.Add(allDef);
				}
			}
			ItemCollectionGenerator_Weapons.Reset();
			ItemCollectionGenerator_Apparel.Reset();
			ItemCollectionGenerator_RawResources.Reset();
			ItemCollectionGenerator_Artifacts.Reset();
		}

		public static void AssignRandomBaseGenItemQuality(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				CompQuality compQuality = things[i].TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					compQuality.SetQuality(QualityUtility.RandomBaseGenItemQuality(), ArtGenerationContext.Outsider);
				}
			}
		}
	}
}
