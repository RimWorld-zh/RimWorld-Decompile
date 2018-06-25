using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD1 RID: 3537
	public static class GraphicDatabaseUtility
	{
		// Token: 0x06004F46 RID: 20294 RVA: 0x00294C58 File Offset: 0x00293058
		public static IEnumerable<string> GraphicNamesInFolder(string folderPath)
		{
			HashSet<string> loadedAssetNames = new HashSet<string>();
			foreach (Texture2D tex in Resources.LoadAll<Texture2D>("Textures/" + folderPath))
			{
				string origAssetName = tex.name;
				string[] pieces = origAssetName.Split(new char[]
				{
					'_'
				});
				string assetName = "";
				if (pieces.Length <= 2)
				{
					assetName = pieces[0];
				}
				else if (pieces.Length == 3)
				{
					assetName = pieces[0] + "_" + pieces[1];
				}
				else if (pieces.Length == 4)
				{
					assetName = string.Concat(new string[]
					{
						pieces[0],
						"_",
						pieces[1],
						"_",
						pieces[2]
					});
				}
				else
				{
					Log.Error("Cannot load assets with >3 pieces.", false);
				}
				if (!loadedAssetNames.Contains(assetName))
				{
					loadedAssetNames.Add(assetName);
					yield return assetName;
				}
			}
			yield break;
		}
	}
}
