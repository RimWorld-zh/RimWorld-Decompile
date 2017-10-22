using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace Verse
{
	public static class DirectXmlLoader
	{
		private static LoadableXmlAsset loadingAsset;

		public static IEnumerable<LoadableXmlAsset> XmlAssetsInModFolder(ModContentPack mod, string folderPath)
		{
			DirectoryInfo di = new DirectoryInfo(Path.Combine(mod.RootDir, folderPath));
			if (di.Exists)
			{
				FileInfo[] files2;
				FileInfo[] files = files2 = di.GetFiles("*.xml", SearchOption.AllDirectories);
				for (int i = 0; i < files2.Length; i++)
				{
					FileInfo file = files2[i];
					LoadableXmlAsset asset = new LoadableXmlAsset(file.Name, file.Directory.FullName, File.ReadAllText(file.FullName));
					yield return asset;
				}
			}
		}

		public static IEnumerable<T> LoadXmlDataInResourcesFolder<T>(string folderPath) where T : new()
		{
			XmlInheritance.Clear();
			List<LoadableXmlAsset> assets = new List<LoadableXmlAsset>();
			object[] array;
			object[] textObjects = array = Resources.LoadAll<TextAsset>(folderPath);
			for (int j = 0; j < array.Length; j++)
			{
				TextAsset text = (TextAsset)array[j];
				LoadableXmlAsset asset = new LoadableXmlAsset(text.name, string.Empty, text.text);
				XmlInheritance.TryRegisterAllFrom(asset, null);
				assets.Add(asset);
			}
			XmlInheritance.Resolve();
			for (int i = 0; i < assets.Count; i++)
			{
				foreach (T item in DirectXmlLoader.AllGameItemsFromAsset<T>(assets[i]))
				{
					yield return item;
				}
			}
			XmlInheritance.Clear();
		}

		public static T ItemFromXmlFile<T>(string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (resolveCrossRefs && DirectXmlCrossRefLoader.LoadingInProgress)
			{
				Log.Error("Cannot call ItemFromXmlFile with resolveCrossRefs=true while loading is already in progress.");
			}
			FileInfo fileInfo = new FileInfo(filePath);
			if (!fileInfo.Exists)
			{
				return new T();
			}
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(File.ReadAllText(fileInfo.FullName));
				T result = DirectXmlToObject.ObjectFromXml<T>((XmlNode)xmlDocument.DocumentElement, false);
				if (resolveCrossRefs)
				{
					DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
				}
				return result;
				IL_008a:
				T result2;
				return result2;
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading file at " + filePath + ". Loading defaults instead. Exception was: " + ex.ToString());
				return new T();
				IL_00d7:
				T result2;
				return result2;
			}
		}

		public static IEnumerable<Def> AllDefsFromAsset(LoadableXmlAsset asset)
		{
			if (DirectXmlLoader.loadingAsset != null)
			{
				Log.Error("Tried to load " + asset + " while loading " + DirectXmlLoader.loadingAsset + ". This will corrupt the internal state of DataLoader.");
			}
			if (asset.xmlDoc != null)
			{
				DirectXmlLoader.loadingAsset = asset;
				XmlNodeList assetNodes = asset.xmlDoc.DocumentElement.ChildNodes;
				bool gotData = false;
				foreach (XmlNode item in assetNodes)
				{
					if (item.NodeType == XmlNodeType.Element)
					{
						XmlAttribute abstractAtt = item.Attributes["Abstract"];
						if (abstractAtt != null && abstractAtt.Value.ToLower() == "true")
						{
							gotData = true;
						}
						else
						{
							Type defType = GenTypes.GetTypeInAnyAssembly(item.Name);
							if (defType != null && typeof(Def).IsAssignableFrom(defType))
							{
								MethodInfo method = typeof(DirectXmlToObject).GetMethod("ObjectFromXml");
								MethodInfo gen = method.MakeGenericMethod(defType);
								Def def = null;
								try
								{
									def = (Def)gen.Invoke(null, new object[2]
									{
										item,
										true
									});
									gotData = true;
								}
								catch (Exception ex)
								{
									Exception e;
									Exception ex2 = e = ex;
									Log.Error("Exception loading def from file " + asset.name + ": " + e);
								}
								if (def != null)
								{
									yield return def;
								}
							}
						}
					}
				}
				if (!gotData)
				{
					Log.Error("Found no usable data when trying to get defs from file " + asset.name);
				}
				DirectXmlLoader.loadingAsset = null;
			}
		}

		public static IEnumerable<T> AllGameItemsFromAsset<T>(LoadableXmlAsset asset) where T : new()
		{
			if (DirectXmlLoader.loadingAsset != null)
			{
				Log.Error("Tried to load " + asset + " while loading " + DirectXmlLoader.loadingAsset + ". This will corrupt the internal state of DataLoader.");
			}
			if (asset.xmlDoc != null)
			{
				DirectXmlLoader.loadingAsset = asset;
				XmlNodeList assetNodes = asset.xmlDoc.DocumentElement.SelectNodes(typeof(T).Name);
				bool gotData = false;
				foreach (XmlNode item2 in assetNodes)
				{
					XmlAttribute abstractAtt = item2.Attributes["Abstract"];
					if (abstractAtt == null || !(abstractAtt.Value.ToLower() == "true"))
					{
						T item;
						try
						{
							item = DirectXmlToObject.ObjectFromXml<T>(item2, true);
							gotData = true;
						}
						catch (Exception ex)
						{
							Exception e;
							Exception ex2 = e = ex;
							Log.Error("Exception loading data from file " + asset.name + ": " + e);
							continue;
						}
						yield return item;
					}
				}
				if (!gotData)
				{
					Log.Error("Found no usable data when trying to get " + typeof(T) + "s from file " + asset.name);
				}
				DirectXmlLoader.loadingAsset = null;
			}
		}
	}
}
