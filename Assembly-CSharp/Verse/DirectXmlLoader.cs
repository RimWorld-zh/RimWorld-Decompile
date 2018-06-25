using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using UnityEngine;

namespace Verse
{
	public static class DirectXmlLoader
	{
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

		[CompilerGenerated]
		private sealed class <XmlAssetsInModFolder>c__Iterator0 : IEnumerable, IEnumerable<LoadableXmlAsset>, IEnumerator, IDisposable, IEnumerator<LoadableXmlAsset>
		{
			internal ModContentPack mod;

			internal string folderPath;

			internal DirectoryInfo <di>__0;

			internal FileInfo[] <files>__0;

			internal FileInfo[] $locvar0;

			internal int $locvar1;

			internal FileInfo <file>__1;

			internal LoadableXmlAsset <asset>__2;

			internal LoadableXmlAsset $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <XmlAssetsInModFolder>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					di = new DirectoryInfo(Path.Combine(mod.RootDir, folderPath));
					if (!di.Exists)
					{
						return false;
					}
					files = di.GetFiles("*.xml", SearchOption.AllDirectories);
					array = files;
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < array.Length)
				{
					file = array[i];
					LoadableXmlAsset asset = new LoadableXmlAsset(file.Name, file.Directory.FullName, File.ReadAllText(file.FullName));
					asset.mod = mod;
					this.$current = asset;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			LoadableXmlAsset IEnumerator<LoadableXmlAsset>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.LoadableXmlAsset>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<LoadableXmlAsset> IEnumerable<LoadableXmlAsset>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DirectXmlLoader.<XmlAssetsInModFolder>c__Iterator0 <XmlAssetsInModFolder>c__Iterator = new DirectXmlLoader.<XmlAssetsInModFolder>c__Iterator0();
				<XmlAssetsInModFolder>c__Iterator.mod = mod;
				<XmlAssetsInModFolder>c__Iterator.folderPath = folderPath;
				return <XmlAssetsInModFolder>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <LoadXmlDataInResourcesFolder>c__Iterator1<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T> where T : new()
		{
			internal List<LoadableXmlAsset> <assets>__0;

			internal string folderPath;

			internal object[] <textObjects>__0;

			internal object[] $locvar0;

			internal int $locvar1;

			internal int <i>__1;

			internal IEnumerator<T> $locvar2;

			internal T <item>__2;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <LoadXmlDataInResourcesFolder>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					XmlInheritance.Clear();
					assets = new List<LoadableXmlAsset>();
					textObjects = Resources.LoadAll<TextAsset>(folderPath);
					array = textObjects;
					for (j = 0; j < array.Length; j++)
					{
						TextAsset textAsset = (TextAsset)array[j];
						LoadableXmlAsset loadableXmlAsset = new LoadableXmlAsset(textAsset.name, "", textAsset.text);
						XmlInheritance.TryRegisterAllFrom(loadableXmlAsset, null);
						assets.Add(loadableXmlAsset);
					}
					XmlInheritance.Resolve();
					i = 0;
					break;
				case 1u:
					Block_3:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							item = enumerator.Current;
							this.$current = item;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					i++;
					break;
				default:
					return false;
				}
				if (i < assets.Count)
				{
					enumerator = DirectXmlLoader.AllGameItemsFromAsset<T>(assets[i]).GetEnumerator();
					num = 4294967293u;
					goto Block_3;
				}
				XmlInheritance.Clear();
				this.$PC = -1;
				return false;
			}

			T IEnumerator<T>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DirectXmlLoader.<LoadXmlDataInResourcesFolder>c__Iterator1<T> <LoadXmlDataInResourcesFolder>c__Iterator = new DirectXmlLoader.<LoadXmlDataInResourcesFolder>c__Iterator1<T>();
				<LoadXmlDataInResourcesFolder>c__Iterator.folderPath = folderPath;
				return <LoadXmlDataInResourcesFolder>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <AllGameItemsFromAsset>c__Iterator2<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T> where T : new()
		{
			internal LoadableXmlAsset asset;

			internal XmlNodeList <assetNodes>__0;

			internal bool <gotData>__0;

			internal IEnumerator $locvar0;

			internal XmlNode <node>__1;

			internal IDisposable $locvar1;

			internal XmlAttribute <abstractAtt>__2;

			internal T <item>__3;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllGameItemsFromAsset>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (asset.xmlDoc == null)
					{
						return false;
					}
					assetNodes = asset.xmlDoc.DocumentElement.SelectNodes(typeof(T).Name);
					gotData = false;
					enumerator = assetNodes.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						node = (XmlNode)enumerator.Current;
						abstractAtt = node.Attributes["Abstract"];
						if (abstractAtt == null || !(abstractAtt.Value.ToLower() == "true"))
						{
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
							this.$current = item;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
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
				this.$PC = -1;
				return false;
			}

			T IEnumerator<T>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DirectXmlLoader.<AllGameItemsFromAsset>c__Iterator2<T> <AllGameItemsFromAsset>c__Iterator = new DirectXmlLoader.<AllGameItemsFromAsset>c__Iterator2<T>();
				<AllGameItemsFromAsset>c__Iterator.asset = asset;
				return <AllGameItemsFromAsset>c__Iterator;
			}
		}
	}
}
