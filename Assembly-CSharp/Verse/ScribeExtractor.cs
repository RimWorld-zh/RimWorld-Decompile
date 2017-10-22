using RimWorld.Planet;
using System;
using System.Xml;

namespace Verse
{
	public static class ScribeExtractor
	{
		public static T ValueFromNode<T>(XmlNode subNode, T defaultValue)
		{
			if (subNode == null)
			{
				return defaultValue;
			}
			try
			{
				try
				{
					return (T)ParseHelper.FromString(subNode.InnerText, typeof(T));
				}
				catch (Exception ex)
				{
					Log.Error("Exception parsing node " + subNode.OuterXml + " into a " + typeof(T) + ":\n" + ex.ToString());
				}
				return default(T);
			}
			catch (Exception arg)
			{
				Log.Error("Exception loading XML: " + arg);
				return defaultValue;
			}
		}

		public static T DefFromNode<T>(XmlNode subNode) where T : Def, new()
		{
			T result;
			if (subNode == null || subNode.InnerText == null || subNode.InnerText == "null")
			{
				result = (T)null;
			}
			else
			{
				string defName = BackCompatibility.BackCompatibleDefName(typeof(T), subNode.InnerText);
				T namedSilentFail = DefDatabase<T>.GetNamedSilentFail(defName);
				if (namedSilentFail == null)
				{
					Log.Error("Could not load reference to " + typeof(T) + " named " + subNode.InnerText);
				}
				result = namedSilentFail;
			}
			return result;
		}

		public static T DefFromNodeUnsafe<T>(XmlNode subNode)
		{
			return (T)GenGeneric.InvokeStaticGenericMethod(typeof(ScribeExtractor), typeof(T), "DefFromNode", subNode);
		}

		public static T SaveableFromNode<T>(XmlNode subNode, object[] ctorArgs)
		{
			T result;
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called SaveableFromNode(), but mode is " + Scribe.mode);
				result = default(T);
			}
			else if (subNode == null)
			{
				result = default(T);
			}
			else
			{
				XmlAttribute xmlAttribute = subNode.Attributes["IsNull"];
				T val;
				if (xmlAttribute != null && xmlAttribute.Value == "True")
				{
					val = default(T);
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
							Log.Error("Could not find class " + text + " while resolving node " + subNode.Name + ". Trying to use " + typeof(T) + " instead. Full node: " + subNode.OuterXml);
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
						Scribe.loader.curPathRelToParent = (string)null;
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
						val = (T)exposable;
					}
					catch (Exception ex)
					{
						val = default(T);
						Log.Error("SaveableFromNode exception: " + ex + "\nSubnode:\n" + subNode.OuterXml);
					}
				}
				result = val;
			}
			return result;
		}

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
						loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Thing), "thing");
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
			loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Thing), label + "/thing");
			return defaultValue;
		}

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
						string str = default(string);
						string targetLoadID = default(string);
						ScribeExtractor.ExtractCellAndMapPairFromTargetInfo(innerText, out str, out targetLoadID);
						loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(Map), "map");
						return new TargetInfo(IntVec3.FromString(str), null, true);
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Map), "map");
					return TargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Thing), label + "/thing");
			loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Map), label + "/map");
			return defaultValue;
		}

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
						string str = default(string);
						string targetLoadID = default(string);
						ScribeExtractor.ExtractCellAndMapPairFromTargetInfo(innerText, out str, out targetLoadID);
						loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(WorldObject), "worldObject");
						return new GlobalTargetInfo(IntVec3.FromString(str), null, true);
					}
					int tile = default(int);
					if (int.TryParse(innerText, out tile))
					{
						loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(WorldObject), "worldObject");
						return new GlobalTargetInfo(tile);
					}
					if (innerText.Length != 0 && innerText[0] == '@')
					{
						loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(innerText.Substring(1), typeof(WorldObject), "worldObject");
						return GlobalTargetInfo.Invalid;
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Map), "map");
					loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(WorldObject), "worldObject");
					return GlobalTargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Thing), label + "/thing");
			loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(Map), label + "/map");
			loadIDs.RegisterLoadIDReadFromXml((string)null, typeof(WorldObject), label + "/worldObject");
			return defaultValue;
		}

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
					if (tile >= 0)
					{
						return new GlobalTargetInfo(tile);
					}
					return GlobalTargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			return loaded;
		}

		private static void ExtractCellAndMapPairFromTargetInfo(string str, out string cell, out string map)
		{
			int num = str.IndexOf(')');
			cell = str.Substring(0, num + 1);
			int num2 = str.IndexOf(',', num + 1);
			map = str.Substring(num2 + 1);
			map = map.TrimStart(' ');
		}
	}
}
