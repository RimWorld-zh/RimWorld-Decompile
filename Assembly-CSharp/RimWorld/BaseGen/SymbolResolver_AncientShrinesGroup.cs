using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003BE RID: 958
	public class SymbolResolver_AncientShrinesGroup : SymbolResolver
	{
		// Token: 0x06001098 RID: 4248 RVA: 0x0008C938 File Offset: 0x0008AD38
		public override void Resolve(ResolveParams rp)
		{
			int num = (rp.rect.Width + 1) / (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.x + 1);
			int num2 = (rp.rect.Height + 1) / (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.z + 1);
			IntVec3 bottomLeft = rp.rect.BottomLeft;
			PodContentsType? podContentsType = rp.podContentsType;
			if (podContentsType == null)
			{
				float value = Rand.Value;
				if (value < 0.5f)
				{
					podContentsType = null;
				}
				else if (value < 0.7f)
				{
					podContentsType = new PodContentsType?(PodContentsType.Slave);
				}
				else
				{
					podContentsType = new PodContentsType?(PodContentsType.AncientHostile);
				}
			}
			int? ancientCryptosleepCasketGroupID = rp.ancientCryptosleepCasketGroupID;
			int value2 = (ancientCryptosleepCasketGroupID == null) ? Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID() : ancientCryptosleepCasketGroupID.Value;
			int num3 = 0;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					if (!Rand.Chance(0.25f))
					{
						if (num3 >= 6)
						{
							break;
						}
						CellRect rect = new CellRect(bottomLeft.x + j * (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.x + 1), bottomLeft.z + i * (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.z + 1), SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.x, SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.z);
						if (rect.FullyContainedWithin(rp.rect))
						{
							ResolveParams resolveParams = rp;
							resolveParams.rect = rect;
							resolveParams.ancientCryptosleepCasketGroupID = new int?(value2);
							resolveParams.podContentsType = podContentsType;
							BaseGen.symbolStack.Push("ancientShrine", resolveParams);
							num3++;
						}
					}
				}
			}
		}

		// Token: 0x04000A27 RID: 2599
		public static readonly IntVec2 StandardAncientShrineSize = new IntVec2(4, 3);

		// Token: 0x04000A28 RID: 2600
		private const int MaxNumCaskets = 6;

		// Token: 0x04000A29 RID: 2601
		private const float SkipShrineChance = 0.25f;

		// Token: 0x04000A2A RID: 2602
		public const int MarginCells = 1;
	}
}
