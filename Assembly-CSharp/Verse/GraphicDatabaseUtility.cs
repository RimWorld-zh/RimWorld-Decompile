using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class GraphicDatabaseUtility
	{
		public static IEnumerable<string> GraphicNamesInFolder(string folderPath)
		{
			HashSet<string> loadedAssetNames = new HashSet<string>();
			Texture2D[] array = Resources.LoadAll<Texture2D>("Textures/" + folderPath);
			int num = 0;
			string assetName;
			while (true)
			{
				if (num < array.Length)
				{
					Texture2D tex = array[num];
					string origAssetName = tex.name;
					string[] pieces = origAssetName.Split('_');
					assetName = "";
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
						assetName = pieces[0] + "_" + pieces[1] + "_" + pieces[2];
					}
					else
					{
						Log.Error("Cannot load assets with >3 pieces.");
					}
					if (!loadedAssetNames.Contains(assetName))
						break;
					num++;
					continue;
				}
				yield break;
			}
			loadedAssetNames.Add(assetName);
			yield return assetName;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
