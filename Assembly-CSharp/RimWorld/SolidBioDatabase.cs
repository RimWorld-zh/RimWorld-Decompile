using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E1 RID: 1249
	public class SolidBioDatabase
	{
		// Token: 0x0600164C RID: 5708 RVA: 0x000C6130 File Offset: 0x000C4530
		public static void Clear()
		{
			SolidBioDatabase.allBios.Clear();
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000C6140 File Offset: 0x000C4540
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

		// Token: 0x04000D03 RID: 3331
		public static List<PawnBio> allBios = new List<PawnBio>();
	}
}
