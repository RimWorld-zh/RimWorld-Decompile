using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_AncientShrinesGroup : SymbolResolver
	{
		public static readonly IntVec2 StandardAncientShrineSize = new IntVec2(4, 3);

		private const int MaxNumCaskets = 6;

		private const float SkipShrineChance = 0.25f;

		public const int MarginCells = 1;

		public override void Resolve(ResolveParams rp)
		{
			int num = rp.rect.Width + 1;
			IntVec2 standardAncientShrineSize = SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize;
			int num2 = num / (standardAncientShrineSize.x + 1);
			int num3 = rp.rect.Height + 1;
			IntVec2 standardAncientShrineSize2 = SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize;
			int num4 = num3 / (standardAncientShrineSize2.z + 1);
			IntVec3 bottomLeft = rp.rect.BottomLeft;
			PodContentsType? podContentsType = rp.podContentsType;
			if (!podContentsType.HasValue)
			{
				float value = Rand.Value;
				podContentsType = ((!(value < 0.5)) ? ((!(value < 0.699999988079071)) ? new PodContentsType?(PodContentsType.SpacerHostile) : new PodContentsType?(PodContentsType.Slave)) : default(PodContentsType?));
			}
			int? ancientCryptosleepCasketGroupID = rp.ancientCryptosleepCasketGroupID;
			int value2 = (!ancientCryptosleepCasketGroupID.HasValue) ? Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID() : ancientCryptosleepCasketGroupID.Value;
			int num5 = 0;
			for (int num6 = 0; num6 < num4; num6++)
			{
				for (int num7 = 0; num7 < num2; num7++)
				{
					if (!Rand.Chance(0.25f))
					{
						if (num5 >= 6)
						{
							break;
						}
						int x = bottomLeft.x;
						int num8 = num7;
						IntVec2 standardAncientShrineSize3 = SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize;
						int minX = x + num8 * (standardAncientShrineSize3.x + 1);
						int z = bottomLeft.z;
						int num9 = num6;
						IntVec2 standardAncientShrineSize4 = SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize;
						int minZ = z + num9 * (standardAncientShrineSize4.z + 1);
						IntVec2 standardAncientShrineSize5 = SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize;
						int x2 = standardAncientShrineSize5.x;
						IntVec2 standardAncientShrineSize6 = SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize;
						CellRect rect = new CellRect(minX, minZ, x2, standardAncientShrineSize6.z);
						if (rect.FullyContainedWithin(rp.rect))
						{
							ResolveParams resolveParams = rp;
							resolveParams.rect = rect;
							resolveParams.ancientCryptosleepCasketGroupID = new int?(value2);
							resolveParams.podContentsType = podContentsType;
							BaseGen.symbolStack.Push("ancientShrine", resolveParams);
							num5++;
						}
					}
				}
			}
		}
	}
}
