using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Verse
{
	public static class XmlInheritance
	{
		private static Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode> resolvedNodes = new Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode>();

		private static List<XmlInheritance.XmlInheritanceNode> unresolvedNodes = new List<XmlInheritance.XmlInheritanceNode>();

		private static Dictionary<string, List<XmlInheritance.XmlInheritanceNode>> nodesByName = new Dictionary<string, List<XmlInheritance.XmlInheritanceNode>>();

		private const string NameAttributeName = "Name";

		private const string ParentNameAttributeName = "ParentName";

		private const string InheritAttributeName = "Inherit";

		private static HashSet<string> tempUsedNodeNames = new HashSet<string>();

		[CompilerGenerated]
		private static Func<XmlInheritance.XmlInheritanceNode, bool> <>f__am$cache0;

		public static void TryRegisterAllFrom(LoadableXmlAsset xmlAsset, ModContentPack mod)
		{
			if (xmlAsset.xmlDoc != null)
			{
				XmlNodeList childNodes = xmlAsset.xmlDoc.DocumentElement.ChildNodes;
				for (int i = 0; i < childNodes.Count; i++)
				{
					if (childNodes[i].NodeType == XmlNodeType.Element)
					{
						XmlInheritance.TryRegister(childNodes[i], mod);
					}
				}
			}
		}

		public static void TryRegister(XmlNode node, ModContentPack mod)
		{
			XmlAttribute xmlAttribute = node.Attributes["Name"];
			XmlAttribute xmlAttribute2 = node.Attributes["ParentName"];
			if (xmlAttribute != null || xmlAttribute2 != null)
			{
				List<XmlInheritance.XmlInheritanceNode> list = null;
				if (xmlAttribute != null && XmlInheritance.nodesByName.TryGetValue(xmlAttribute.Value, out list))
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].mod == mod)
						{
							if (mod == null)
							{
								Log.Error("XML error: Could not register node named \"" + xmlAttribute.Value + "\" because this name is already used.", false);
							}
							else
							{
								Log.Error(string.Concat(new string[]
								{
									"XML error: Could not register node named \"",
									xmlAttribute.Value,
									"\" in mod ",
									mod.ToString(),
									" because this name is already used in this mod."
								}), false);
							}
							return;
						}
					}
				}
				XmlInheritance.XmlInheritanceNode xmlInheritanceNode = new XmlInheritance.XmlInheritanceNode();
				xmlInheritanceNode.xmlNode = node;
				xmlInheritanceNode.mod = mod;
				XmlInheritance.unresolvedNodes.Add(xmlInheritanceNode);
				if (xmlAttribute != null)
				{
					if (list != null)
					{
						list.Add(xmlInheritanceNode);
					}
					else
					{
						list = new List<XmlInheritance.XmlInheritanceNode>();
						list.Add(xmlInheritanceNode);
						XmlInheritance.nodesByName.Add(xmlAttribute.Value, list);
					}
				}
			}
		}

		public static void Resolve()
		{
			XmlInheritance.ResolveParentsAndChildNodesLinks();
			XmlInheritance.ResolveXmlNodes();
		}

		public static XmlNode GetResolvedNodeFor(XmlNode originalNode)
		{
			if (originalNode.Attributes["ParentName"] != null)
			{
				XmlInheritance.XmlInheritanceNode xmlInheritanceNode;
				if (XmlInheritance.resolvedNodes.TryGetValue(originalNode, out xmlInheritanceNode))
				{
					return xmlInheritanceNode.resolvedXmlNode;
				}
				if (XmlInheritance.unresolvedNodes.Any((XmlInheritance.XmlInheritanceNode x) => x.xmlNode == originalNode))
				{
					Log.Error("XML error: XML node \"" + originalNode.Name + "\" has not been resolved yet. There's probably a Resolve() call missing somewhere.", false);
				}
				else
				{
					Log.Error("XML error: Tried to get resolved node for node \"" + originalNode.Name + "\" which uses a ParentName attribute, but it is not in a resolved nodes collection, which means that it was never registered or there was an error while resolving it.", false);
				}
			}
			return originalNode;
		}

		public static void Clear()
		{
			XmlInheritance.resolvedNodes.Clear();
			XmlInheritance.unresolvedNodes.Clear();
			XmlInheritance.nodesByName.Clear();
		}

		private static void ResolveParentsAndChildNodesLinks()
		{
			for (int i = 0; i < XmlInheritance.unresolvedNodes.Count; i++)
			{
				XmlAttribute xmlAttribute = XmlInheritance.unresolvedNodes[i].xmlNode.Attributes["ParentName"];
				if (xmlAttribute != null)
				{
					XmlInheritance.unresolvedNodes[i].parent = XmlInheritance.GetBestParentFor(XmlInheritance.unresolvedNodes[i], xmlAttribute.Value);
					if (XmlInheritance.unresolvedNodes[i].parent != null)
					{
						XmlInheritance.unresolvedNodes[i].parent.children.Add(XmlInheritance.unresolvedNodes[i]);
					}
				}
			}
		}

		private static void ResolveXmlNodes()
		{
			List<XmlInheritance.XmlInheritanceNode> list = (from x in XmlInheritance.unresolvedNodes
			where x.parent == null || x.parent.resolvedXmlNode != null
			select x).ToList<XmlInheritance.XmlInheritanceNode>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlInheritance.ResolveXmlNodesRecursively(list[i]);
			}
			for (int j = 0; j < XmlInheritance.unresolvedNodes.Count; j++)
			{
				if (XmlInheritance.unresolvedNodes[j].resolvedXmlNode == null)
				{
					Log.Error("XML error: Cyclic inheritance hierarchy detected for node \"" + XmlInheritance.unresolvedNodes[j].xmlNode.Name + "\". Full node: " + XmlInheritance.unresolvedNodes[j].xmlNode.OuterXml, false);
				}
				else
				{
					XmlInheritance.resolvedNodes.Add(XmlInheritance.unresolvedNodes[j].xmlNode, XmlInheritance.unresolvedNodes[j]);
				}
			}
			XmlInheritance.unresolvedNodes.Clear();
		}

		private static void ResolveXmlNodesRecursively(XmlInheritance.XmlInheritanceNode node)
		{
			if (node.resolvedXmlNode != null)
			{
				Log.Error("XML error: Cyclic inheritance hierarchy detected for node \"" + node.xmlNode.Name + "\". Full node: " + node.xmlNode.OuterXml, false);
			}
			else
			{
				XmlInheritance.ResolveXmlNodeFor(node);
				for (int i = 0; i < node.children.Count; i++)
				{
					XmlInheritance.ResolveXmlNodesRecursively(node.children[i]);
				}
			}
		}

		private static XmlInheritance.XmlInheritanceNode GetBestParentFor(XmlInheritance.XmlInheritanceNode node, string parentName)
		{
			XmlInheritance.XmlInheritanceNode xmlInheritanceNode = null;
			List<XmlInheritance.XmlInheritanceNode> list;
			if (XmlInheritance.nodesByName.TryGetValue(parentName, out list))
			{
				if (node.mod == null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].mod == null)
						{
							xmlInheritanceNode = list[i];
							break;
						}
					}
					if (xmlInheritanceNode == null)
					{
						for (int j = 0; j < list.Count; j++)
						{
							if (xmlInheritanceNode == null || list[j].mod.loadOrder < xmlInheritanceNode.mod.loadOrder)
							{
								xmlInheritanceNode = list[j];
							}
						}
					}
				}
				else
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].mod != null)
						{
							if (list[k].mod.loadOrder <= node.mod.loadOrder && (xmlInheritanceNode == null || list[k].mod.loadOrder > xmlInheritanceNode.mod.loadOrder))
							{
								xmlInheritanceNode = list[k];
							}
						}
					}
					if (xmlInheritanceNode == null)
					{
						for (int l = 0; l < list.Count; l++)
						{
							if (list[l].mod == null)
							{
								xmlInheritanceNode = list[l];
								break;
							}
						}
					}
				}
			}
			XmlInheritance.XmlInheritanceNode result;
			if (xmlInheritanceNode == null)
			{
				Log.Error(string.Concat(new string[]
				{
					"XML error: Could not find parent node named \"",
					parentName,
					"\" for node \"",
					node.xmlNode.Name,
					"\". Full node: ",
					node.xmlNode.OuterXml
				}), false);
				result = null;
			}
			else
			{
				result = xmlInheritanceNode;
			}
			return result;
		}

		private static void ResolveXmlNodeFor(XmlInheritance.XmlInheritanceNode node)
		{
			if (node.parent == null)
			{
				node.resolvedXmlNode = node.xmlNode;
			}
			else if (node.parent.resolvedXmlNode == null)
			{
				Log.Error("XML error: Internal error. Tried to resolve node whose parent has not been resolved yet. This means that this method was called in incorrect order.", false);
				node.resolvedXmlNode = node.xmlNode;
			}
			else
			{
				XmlInheritance.CheckForDuplicateNodes(node.xmlNode, node.xmlNode);
				XmlNode xmlNode = node.parent.resolvedXmlNode.CloneNode(true);
				XmlInheritance.RecursiveNodeCopyOverwriteElements(node.xmlNode, xmlNode);
				node.resolvedXmlNode = xmlNode;
			}
		}

		private static void RecursiveNodeCopyOverwriteElements(XmlNode child, XmlNode current)
		{
			XmlAttribute xmlAttribute = child.Attributes["Inherit"];
			if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "false")
			{
				while (current.HasChildNodes)
				{
					current.RemoveChild(current.FirstChild);
				}
				IEnumerator enumerator = child.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						XmlNode node = (XmlNode)obj;
						XmlNode newChild = current.OwnerDocument.ImportNode(node, true);
						current.AppendChild(newChild);
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
			}
			else
			{
				current.Attributes.RemoveAll();
				XmlAttributeCollection attributes = child.Attributes;
				for (int i = 0; i < attributes.Count; i++)
				{
					XmlAttribute node2 = (XmlAttribute)current.OwnerDocument.ImportNode(attributes[i], true);
					current.Attributes.Append(node2);
				}
				List<XmlElement> list = new List<XmlElement>();
				XmlNode xmlNode = null;
				IEnumerator enumerator2 = child.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.NodeType == XmlNodeType.Text)
						{
							xmlNode = xmlNode2;
						}
						else if (xmlNode2.NodeType == XmlNodeType.Element)
						{
							list.Add((XmlElement)xmlNode2);
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				if (xmlNode != null)
				{
					for (int j = current.ChildNodes.Count - 1; j >= 0; j--)
					{
						XmlNode xmlNode3 = current.ChildNodes[j];
						if (xmlNode3.NodeType != XmlNodeType.Attribute)
						{
							current.RemoveChild(xmlNode3);
						}
					}
					XmlNode newChild2 = current.OwnerDocument.ImportNode(xmlNode, true);
					current.AppendChild(newChild2);
				}
				else if (!list.Any<XmlElement>())
				{
					bool flag = false;
					IEnumerator enumerator3 = current.ChildNodes.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							object obj3 = enumerator3.Current;
							XmlNode xmlNode4 = (XmlNode)obj3;
							if (xmlNode4.NodeType == XmlNodeType.Element)
							{
								flag = true;
								break;
							}
						}
					}
					finally
					{
						IDisposable disposable3;
						if ((disposable3 = (enumerator3 as IDisposable)) != null)
						{
							disposable3.Dispose();
						}
					}
					if (!flag)
					{
						IEnumerator enumerator4 = current.ChildNodes.GetEnumerator();
						try
						{
							while (enumerator4.MoveNext())
							{
								object obj4 = enumerator4.Current;
								XmlNode xmlNode5 = (XmlNode)obj4;
								if (xmlNode5.NodeType != XmlNodeType.Attribute)
								{
									current.RemoveChild(xmlNode5);
								}
							}
						}
						finally
						{
							IDisposable disposable4;
							if ((disposable4 = (enumerator4 as IDisposable)) != null)
							{
								disposable4.Dispose();
							}
						}
					}
				}
				else
				{
					for (int k = 0; k < list.Count; k++)
					{
						XmlElement xmlElement = list[k];
						if (xmlElement.Name == "li")
						{
							XmlNode newChild3 = current.OwnerDocument.ImportNode(xmlElement, true);
							current.AppendChild(newChild3);
						}
						else
						{
							XmlElement xmlElement2 = current[xmlElement.Name];
							if (xmlElement2 != null)
							{
								XmlInheritance.RecursiveNodeCopyOverwriteElements(xmlElement, xmlElement2);
							}
							else
							{
								XmlNode newChild4 = current.OwnerDocument.ImportNode(xmlElement, true);
								current.AppendChild(newChild4);
							}
						}
					}
				}
			}
		}

		private static void CheckForDuplicateNodes(XmlNode node, XmlNode root)
		{
			XmlInheritance.tempUsedNodeNames.Clear();
			IEnumerator enumerator = node.ChildNodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.NodeType == XmlNodeType.Element && !(xmlNode.Name == "li"))
					{
						if (XmlInheritance.tempUsedNodeNames.Contains(xmlNode.Name))
						{
							Log.Error(string.Concat(new string[]
							{
								"XML error: Duplicate XML node name ",
								xmlNode.Name,
								" in this XML block: ",
								node.OuterXml,
								(node == root) ? "" : ("\n\nRoot node: " + root.OuterXml)
							}), false);
						}
						else
						{
							XmlInheritance.tempUsedNodeNames.Add(xmlNode.Name);
						}
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
			XmlInheritance.tempUsedNodeNames.Clear();
			IEnumerator enumerator2 = node.ChildNodes.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					XmlNode xmlNode2 = (XmlNode)obj2;
					if (xmlNode2.NodeType == XmlNodeType.Element)
					{
						XmlInheritance.CheckForDuplicateNodes(xmlNode2, root);
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static XmlInheritance()
		{
		}

		[CompilerGenerated]
		private static bool <ResolveXmlNodes>m__0(XmlInheritance.XmlInheritanceNode x)
		{
			return x.parent == null || x.parent.resolvedXmlNode != null;
		}

		private class XmlInheritanceNode
		{
			public XmlNode xmlNode;

			public XmlNode resolvedXmlNode;

			public ModContentPack mod;

			public XmlInheritance.XmlInheritanceNode parent;

			public List<XmlInheritance.XmlInheritanceNode> children = new List<XmlInheritance.XmlInheritanceNode>();

			public XmlInheritanceNode()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <GetResolvedNodeFor>c__AnonStorey0
		{
			internal XmlNode originalNode;

			public <GetResolvedNodeFor>c__AnonStorey0()
			{
			}

			internal bool <>m__0(XmlInheritance.XmlInheritanceNode x)
			{
				return x.xmlNode == this.originalNode;
			}
		}
	}
}
