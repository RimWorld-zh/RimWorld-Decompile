using System;
using System.Xml;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000D9D RID: 3485
	public static class ScribeExtractor
	{
		// Token: 0x06004DC3 RID: 19907 RVA: 0x00289374 File Offset: 0x00287774
		public static T ValueFromNode<T>(XmlNode subNode, T defaultValue)
		{
			T result;
			if (subNode == null)
			{
				result = defaultValue;
			}
			else
			{
				try
				{
					try
					{
						return (T)((object)ParseHelper.FromString(subNode.InnerText, typeof(T)));
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception parsing node ",
							subNode.OuterXml,
							" into a ",
							typeof(T),
							":\n",
							ex.ToString()
						}), false);
					}
					result = default(T);
				}
				catch (Exception arg)
				{
					Log.Error("Exception loading XML: " + arg, false);
					result = defaultValue;
				}
			}
			return result;
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x00289448 File Offset: 0x00287848
		public static T DefFromNode<T>(XmlNode subNode) where T : Def, new()
		{
			T result;
			if (subNode == null || subNode.InnerText == null || subNode.InnerText == "null")
			{
				result = (T)((object)null);
			}
			else
			{
				string text = BackCompatibility.BackCompatibleDefName(typeof(T), subNode.InnerText, false);
				T namedSilentFail = DefDatabase<T>.GetNamedSilentFail(text);
				if (namedSilentFail == null)
				{
					if (text == subNode.InnerText)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not load reference to ",
							typeof(T),
							" named ",
							subNode.InnerText
						}), false);
					}
					else
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not load reference to ",
							typeof(T),
							" named ",
							subNode.InnerText,
							" after compatibility-conversion to ",
							text
						}), false);
					}
				}
				result = namedSilentFail;
			}
			return result;
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x0028954C File Offset: 0x0028794C
		public static T DefFromNodeUnsafe<T>(XmlNode subNode)
		{
			return (T)((object)GenGeneric.InvokeStaticGenericMethod(typeof(ScribeExtractor), typeof(T), "DefFromNode", new object[]
			{
				subNode
			}));
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x00289590 File Offset: 0x00287990
		public static T SaveableFromNode<T>(XmlNode subNode, object[] ctorArgs)
		{
			T result;
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called SaveableFromNode(), but mode is " + Scribe.mode, false);
				result = default(T);
			}
			else if (subNode == null)
			{
				result = default(T);
			}
			else
			{
				XmlAttribute xmlAttribute = subNode.Attributes["IsNull"];
				T t;
				if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "true")
				{
					t = default(T);
				}
				else
				{
					try
					{
						XmlAttribute xmlAttribute2 = subNode.Attributes["Class"];
						string text = (xmlAttribute2 == null) ? typeof(T).FullName : xmlAttribute2.Value;
						Type type = BackCompatibility.GetBackCompatibleType(typeof(T), text, subNode);
						if (type == null)
						{
							Log.Error(string.Concat(new object[]
							{
								"Could not find class ",
								text,
								" while resolving node ",
								subNode.Name,
								". Trying to use ",
								typeof(T),
								" instead. Full node: ",
								subNode.OuterXml
							}), false);
							type = typeof(T);
						}
						if (type.IsAbstract)
						{
							throw new ArgumentException("Can't load abstract class " + type);
						}
						IExposable exposable = (IExposable)Activator.CreateInstance(type, ctorArgs);
						bool flag = typeof(T).IsValueType || typeof(Name).IsAssignableFrom(typeof(T));
						if (!flag)
						{
							Scribe.loader.crossRefs.RegisterForCrossRefResolve(exposable);
						}
						XmlNode curXmlParent = Scribe.loader.curXmlParent;
						IExposable curParent = Scribe.loader.curParent;
						string curPathRelToParent = Scribe.loader.curPathRelToParent;
						Scribe.loader.curXmlParent = subNode;
						Scribe.loader.curParent = exposable;
						Scribe.loader.curPathRelToParent = null;
						try
						{
							exposable.ExposeData();
						}
						finally
						{
							Scribe.loader.curXmlParent = curXmlParent;
							Scribe.loader.curParent = curParent;
							Scribe.loader.curPathRelToParent = curPathRelToParent;
						}
						if (!flag)
						{
							Scribe.loader.initer.RegisterForPostLoadInit(exposable);
						}
						t = (T)((object)exposable);
					}
					catch (Exception ex)
					{
						t = default(T);
						Log.Error(string.Concat(new object[]
						{
							"SaveableFromNode exception: ",
							ex,
							"\nSubnode:\n",
							subNode.OuterXml
						}), false);
					}
				}
				result = t;
			}
			return result;
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x00289878 File Offset: 0x00287C78
		public static LocalTargetInfo LocalTargetInfoFromNode(XmlNode node, string label, LocalTargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						return new LocalTargetInfo(IntVec3.FromString(innerText));
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					return LocalTargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			return defaultValue;
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x00289950 File Offset: 0x00287D50
		public static TargetInfo TargetInfoFromNode(XmlNode node, string label, TargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						string str;
						string targetLoadID;
						ScribeExtractor.ExtractCellAndMapPairFromTargetInfo(innerText, out str, out targetLoadID);
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(Map), "map");
						return new TargetInfo(IntVec3.FromString(str), null, true);
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
					return TargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), label + "/map");
			return defaultValue;
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x00289A80 File Offset: 0x00287E80
		public static GlobalTargetInfo GlobalTargetInfoFromNode(XmlNode node, string label, GlobalTargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						string str;
						string targetLoadID;
						ScribeExtractor.ExtractCellAndMapPairFromTargetInfo(innerText, out str, out targetLoadID);
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
						return new GlobalTargetInfo(IntVec3.FromString(str), null, true);
					}
					int tile;
					if (int.TryParse(innerText, out tile))
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
						return new GlobalTargetInfo(tile);
					}
					if (innerText.Length != 0 && innerText[0] == '@')
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(innerText.Substring(1), typeof(WorldObject), "worldObject");
						return GlobalTargetInfo.Invalid;
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
					return GlobalTargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), label + "/map");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), label + "/worldObject");
			return defaultValue;
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x00289CD0 File Offset: 0x002880D0
		public static LocalTargetInfo ResolveLocalTargetInfo(LocalTargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					IntVec3 cell = loaded.Cell;
					if (thing != null)
					{
						return new LocalTargetInfo(thing);
					}
					return new LocalTargetInfo(cell);
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			return loaded;
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x00289D48 File Offset: 0x00288148
		public static TargetInfo ResolveTargetInfo(TargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					Map map = Scribe.loader.crossRefs.TakeResolvedRef<Map>("map");
					IntVec3 cell = loaded.Cell;
					if (thing != null)
					{
						return new TargetInfo(thing);
					}
					if (cell.IsValid && map != null)
					{
						return new TargetInfo(cell, map, false);
					}
					return TargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			return loaded;
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x00289DF4 File Offset: 0x002881F4
		public static GlobalTargetInfo ResolveGlobalTargetInfo(GlobalTargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					Map map = Scribe.loader.crossRefs.TakeResolvedRef<Map>("map");
					WorldObject worldObject = Scribe.loader.crossRefs.TakeResolvedRef<WorldObject>("worldObject");
					IntVec3 cell = loaded.Cell;
					int tile = loaded.Tile;
					if (thing != null)
					{
						return new GlobalTargetInfo(thing);
					}
					if (worldObject != null)
					{
						return new GlobalTargetInfo(worldObject);
					}
					if (cell.IsValid)
					{
						if (map != null)
						{
							return new GlobalTargetInfo(cell, map, false);
						}
						return GlobalTargetInfo.Invalid;
					}
					else
					{
						if (tile >= 0)
						{
							return new GlobalTargetInfo(tile);
						}
						return GlobalTargetInfo.Invalid;
					}
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			return loaded;
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x00289EF8 File Offset: 0x002882F8
		public static BodyPartRecord BodyPartFromNode(XmlNode node, string label, BodyPartRecord defaultValue)
		{
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					XmlAttribute xmlAttribute = node.Attributes["IsNull"];
					if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "true")
					{
						return null;
					}
					BodyDef bodyDef = ScribeExtractor.DefFromNode<BodyDef>(Scribe.loader.curXmlParent["body"]);
					XmlElement xmlElement = Scribe.loader.curXmlParent["index"];
					int index = (xmlElement == null) ? -1 : int.Parse(xmlElement.InnerText);
					if (bodyDef == null)
					{
						return null;
					}
					return bodyDef.GetPartAtIndex(index);
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			return defaultValue;
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x00289FD4 File Offset: 0x002883D4
		private static void ExtractCellAndMapPairFromTargetInfo(string str, out string cell, out string map)
		{
			int num = str.IndexOf(')');
			cell = str.Substring(0, num + 1);
			int num2 = str.IndexOf(',', num + 1);
			map = str.Substring(num2 + 1);
			map = map.TrimStart(new char[]
			{
				' '
			});
		}
	}
}
