using System;
using System.Collections;
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
				int num = 0;
				if (num < files2.Length)
				{
					FileInfo file = files2[num];
					LoadableXmlAsset asset = new LoadableXmlAsset(file.Name, file.Directory.FullName, File.ReadAllText(file.FullName));
					yield return asset;
					/*Error: Unable to find new state assignment for yield return*/;
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
				TextAsset textAsset = (TextAsset)array[j];
				LoadableXmlAsset loadableXmlAsset = new LoadableXmlAsset(textAsset.name, "", textAsset.text);
				XmlInheritance.TryRegisterAllFrom(loadableXmlAsset, null);
				assets.Add(loadableXmlAsset);
			}
			XmlInheritance.Resolve();
			for (int i = 0; i < assets.Count; i++)
			{
				using (IEnumerator<T> enumerator = DirectXmlLoader.AllGameItemsFromAsset<T>(assets[i]).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						T item = enumerator.Current;
						yield return item;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			XmlInheritance.Clear();
			yield break;
			IL_019e:
			/*Error near IL_019f: Unexpected return in MoveNext()*/;
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
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading file at " + filePath + ". Loading defaults instead. Exception was: " + ex.ToString());
				return new T();
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
				IEnumerator enumerator = assetNodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						XmlNode node = (XmlNode)enumerator.Current;
						if (node.NodeType == XmlNodeType.Element)
						{
							XmlAttribute abstractAtt = node.Attributes["Abstract"];
							if (abstractAtt != null && abstractAtt.Value.ToLower() == "true")
							{
								gotData = true;
							}
							else
							{
								Type defType = GenTypes.GetTypeInAnyAssembly(node.Name);
								if (defType != null && typeof(Def).IsAssignableFrom(defType))
								{
									MethodInfo method = typeof(DirectXmlToObject).GetMethod("ObjectFromXml");
									MethodInfo gen = method.MakeGenericMethod(defType);
									Def def = null;
									try
									{
										def = (Def)gen.Invoke(null, new object[2]
										{
											node,
											true
										});
										gotData = true;
									}
									catch (Exception ex)
									{
										Log.Error("Exception loading def from file " + asset.name + ": " + ex);
									}
									if (def != null)
									{
										yield return def;
										/*Error: Unable to find new state assignment for yield return*/;
									}
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					IDisposable disposable2 = disposable = (enumerator as IDisposable);
					if (disposable != null)
					{
						disposable2.Dispose();
					}
				}
				if (!gotData)
				{
					Log.Error("Found no usable data when trying to get defs from file " + asset.name);
				}
				DirectXmlLoader.loadingAsset = null;
			}
			yield break;
			IL_02eb:
			/*Error near IL_02ec: Unexpected return in MoveNext()*/;
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
				IEnumerator enumerator = assetNodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						XmlNode node = (XmlNode)enumerator.Current;
						XmlAttribute abstractAtt = node.Attributes["Abstract"];
						if (abstractAtt != null && abstractAtt.Value.ToLower() == "true")
						{
							continue;
						}
						T item;
						try
						{
							item = DirectXmlToObject.ObjectFromXml<T>(node, true);
							gotData = true;
						}
						catch (Exception ex)
						{
							Log.Error("Exception loading data from file " + asset.name + ": " + ex);
							continue;
						}
						yield return item;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				finally
				{
					IDisposable disposable;
					IDisposable disposable2 = disposable = (enumerator as IDisposable);
					if (disposable != null)
					{
						disposable2.Dispose();
					}
				}
				if (!gotData)
				{
					Log.Error("Found no usable data when trying to get " + typeof(T) + "s from file " + asset.name);
				}
				DirectXmlLoader.loadingAsset = null;
			}
			yield break;
			IL_024e:
			/*Error near IL_024f: Unexpected return in MoveNext()*/;
		}
	}
}
