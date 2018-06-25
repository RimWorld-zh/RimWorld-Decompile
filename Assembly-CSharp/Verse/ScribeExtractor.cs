using System;
using System.Xml;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000D9D RID: 3485
	public static class ScribeExtractor
	{
		// Token: 0x06004DDC RID: 19932 RVA: 0x0028AD30 File Offset: 0x00289130
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

		// Token: 0x06004DDD RID: 19933 RVA: 0x0028AE04 File Offset: 0x00289204
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

		// Token: 0x06004DDE RID: 19934 RVA: 0x0028AF08 File Offset: 0x00289308
		public static T DefFromNodeUnsafe<T>(XmlNode subNode)
		{
			return (T)((object)GenGeneric.InvokeStaticGenericMethod(typeof(ScribeExtractor), typeof(T), "DefFromNode", new object[]
			{
				subNode
			}));
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x0028AF4C File Offset: 0x0028934C
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

		// Token: 0x06004DE0 RID: 19936 RVA: 0x0028B234 File Offset: 0x00289634
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

		// Token: 0x06004DE1 RID: 19937 RVA: 0x0028B30C File Offset: 0x0028970C
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

		// Token: 0x06004DE2 RID: 19938 RVA: 0x0028B43C File Offset: 0x0028983C
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

		// Token: 0x06004DE3 RID: 19939 RVA: 0x0028B68C File Offset: 0x00289A8C
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

		// Token: 0x06004DE4 RID: 19940 RVA: 0x0028B704 File Offset: 0x00289B04
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

		// Token: 0x06004DE5 RID: 19941 RVA: 0x0028B7B0 File Offset: 0x00289BB0
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

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0028B8B4 File Offset: 0x00289CB4
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

		// Token: 0x06004DE7 RID: 19943 RVA: 0x0028B990 File Offset: 0x00289D90
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
