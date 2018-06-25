using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E3 RID: 1251
	public class SolidBioDatabase
	{
		// Token: 0x04000D03 RID: 3331
		public static List<PawnBio> allBios = new List<PawnBio>();

		// Token: 0x06001650 RID: 5712 RVA: 0x000C6280 File Offset: 0x000C4680
		public static void Clear()
		{
			SolidBioDatabase.allBios.Clear();
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x000C6290 File Offset: 0x000C4690
		public static void LoadAllBios()
		{
			foreach (PawnBio pawnBio in DirectXmlLoader.LoadXmlDataInResourcesFolder<PawnBio>("Backstories/Solid"))
			{
				pawnBio.name.ResolveMissingPieces(null);
				if (pawnBio.childhood == null || pawnBio.adulthood == null)
				{
					PawnNameDatabaseSolid.AddPlayerContentName(pawnBio.name, pawnBio.gender);
				}
				else
				{
					pawnBio.PostLoad();
					pawnBio.ResolveReferences();
					foreach (string text in pawnBio.ConfigErrors())
					{
						Log.Error(text, false);
					}
					SolidBioDatabase.allBios.Add(pawnBio);
					pawnBio.childhood.shuffleable = false;
					pawnBio.childhood.slot = BackstorySlot.Childhood;
					pawnBio.adulthood.shuffleable = false;
					pawnBio.adulthood.slot = BackstorySlot.Adulthood;
					BackstoryHardcodedData.InjectHardcodedData(pawnBio);
					BackstoryDatabase.AddBackstory(pawnBio.childhood);
					BackstoryDatabase.AddBackstory(pawnBio.adulthood);
				}
			}
		}
	}
}
