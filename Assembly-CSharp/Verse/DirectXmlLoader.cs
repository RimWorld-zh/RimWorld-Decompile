using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D82 RID: 3458
	public static class DirectXmlLoader
	{
		// Token: 0x06004D55 RID: 19797 RVA: 0x00283B58 File Offset: 0x00281F58
		public static IEnumerable<LoadableXmlAsset> XmlAssetsInModFolder(ModContentPack mod, string folderPath)
		{
			DirectoryInfo di = new DirectoryInfo(Path.Combine(mod.RootDir, folderPath));
			if (!di.Exists)
			{
				yield break;
			}
			FileInfo[] files = di.GetFiles("*.xml", SearchOption.AllDirectories);
			foreach (FileInfo file in files)
			{
				yield return new LoadableXmlAsset(file.Name, file.Directory.FullName, File.ReadAllText(file.FullName))
				{
					mod = mod
				};
			}
			yield break;
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x00283B8C File Offset: 0x00281F8C
		public static IEnumerable<T> LoadXmlDataInResourcesFolder<T>(string folderPath) where T : new()
		{
			XmlInheritance.Clear();
			List<LoadableXmlAsset> assets = new List<LoadableXmlAsset>();
			object[] textObjects = Resources.LoadAll<TextAsset>(folderPath);
			foreach (TextAsset textAsset in textObjects)
			{
				LoadableXmlAsset loadableXmlAsset = new LoadableXmlAsset(textAsset.name, "", textAsset.text);
				XmlInheritance.TryRegisterAllFrom(loadableXmlAsset, null);
				assets.Add(loadableXmlAsset);
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
			yield break;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x00283BB8 File Offset: 0x00281FB8
		public static T ItemFromXmlFile<T>(string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (resolveCrossRefs && DirectXmlCrossRefLoader.LoadingInProgress)
			{
				Log.Error("Cannot call ItemFromXmlFile with resolveCrossRefs=true while loading is already in progress.", false);
			}
			FileInfo fileInfo = new FileInfo(filePath);
			T result;
			if (!fileInfo.Exists)
			{
				result = Activator.CreateInstance<T>();
			}
			else
			{
				try
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(File.ReadAllText(fileInfo.FullName));
					T t = DirectXmlToObject.ObjectFromXml<T>(xmlDocument.DocumentElement, false);
					if (resolveCrossRefs)
					{
						DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
					}
					result = t;
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading file at " + filePath + ". Loading defaults instead. Exception was: " + ex.ToString(), false);
					result = Activator.CreateInstance<T>();
				}
			}
			return result;
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x00283C74 File Offset: 0x00282074
		public static Def DefFromNode(XmlNode node, LoadableXmlAsset loadingAsset)
		{
			Def result;
			if (node.NodeType != XmlNodeType.Element)
			{
				result = null;
			}
			else
			{
				XmlAttribute xmlAttribute = node.Attributes["Abstract"];
				if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "true")
				{
					result = null;
				}
				else
				{
					Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(node.Name);
					if (typeInAnyAssembly == null)
					{
						result = null;
					}
					else if (!typeof(Def).IsAssignableFrom(typeInAnyAssembly))
					{
						result = null;
					}
					else
					{
						MethodInfo method = typeof(DirectXmlToObject).GetMethod("ObjectFromXml");
						MethodInfo methodInfo = method.MakeGenericMethod(new Type[]
						{
							typeInAnyAssembly
						});
						Def def = null;
						try
						{
							def = (Def)methodInfo.Invoke(null, new object[]
							{
								node,
								true
							});
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception loading def from file ",
								(loadingAsset == null) ? "(unknown)" : loadingAsset.name,
								": ",
								ex
							}), false);
						}
						result = def;
					}
				}
			}
			return result;
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x00283DB8 File Offset: 0x002821B8
		public static IEnumerable<T> AllGameItemsFromAsset<T>(LoadableXmlAsset asset) where T : new()
		{
			if (asset.xmlDoc == null)
			{
				yield break;
			}
			XmlNodeList assetNodes = asset.xmlDoc.DocumentElement.SelectNodes(typeof(T).Name);
			bool gotData = false;
			IEnumerator enumerator = assetNodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlNode node = (XmlNode)obj;
					XmlAttribute abstractAtt = node.Attributes["Abstract"];
					if (abstractAtt == null || !(abstractAtt.Value.ToLower() == "true"))
					{
						T item;
						try
						{
							item = DirectXmlToObject.ObjectFromXml<T>(node, true);
							gotData = true;
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception loading data from file ",
								asset.name,
								": ",
								ex
							}), false);
							continue;
						}
						yield return item;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (!gotData)
			{
				Log.Error(string.Concat(new object[]
				{
					"Found no usable data when trying to get ",
					typeof(T),
					"s from file ",
					asset.name
				}), false);
			}
			yield break;
		}
	}
}
