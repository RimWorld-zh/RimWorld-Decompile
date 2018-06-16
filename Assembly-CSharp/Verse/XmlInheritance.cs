using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D8C RID: 3468
	public static class XmlInheritance
	{
		// Token: 0x06004D72 RID: 19826 RVA: 0x00285FE4 File Offset: 0x002843E4
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

		// Token: 0x06004D73 RID: 19827 RVA: 0x0028604C File Offset: 0x0028444C
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

		// Token: 0x06004D74 RID: 19828 RVA: 0x0028619F File Offset: 0x0028459F
		public static void Resolve()
		{
			XmlInheritance.ResolveParentsAndChildNodesLinks();
			XmlInheritance.ResolveXmlNodes();
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x002861AC File Offset: 0x002845AC
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

		// Token: 0x06004D76 RID: 19830 RVA: 0x00286273 File Offset: 0x00284673
		public static void Clear()
		{
			XmlInheritance.resolvedNodes.Clear();
			XmlInheritance.unresolvedNodes.Clear();
			XmlInheritance.nodesByName.Clear();
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x00286294 File Offset: 0x00284694
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

		// Token: 0x06004D78 RID: 19832 RVA: 0x0028634C File Offset: 0x0028474C
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

		// Token: 0x06004D79 RID: 19833 RVA: 0x00286454 File Offset: 0x00284854
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

		// Token: 0x06004D7A RID: 19834 RVA: 0x002864D4 File Offset: 0x002848D4
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

		// Token: 0x06004D7B RID: 19835 RVA: 0x002866C8 File Offset: 0x00284AC8
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

		// Token: 0x06004D7C RID: 19836 RVA: 0x00286758 File Offset: 0x00284B58
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

		// Token: 0x06004D7D RID: 19837 RVA: 0x00286B24 File Offset: 0x00284F24
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

		// Token: 0x040033B2 RID: 13234
		private static Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode> resolvedNodes = new Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode>();

		// Token: 0x040033B3 RID: 13235
		private static List<XmlInheritance.XmlInheritanceNode> unresolvedNodes = new List<XmlInheritance.XmlInheritanceNode>();

		// Token: 0x040033B4 RID: 13236
		private static Dictionary<string, List<XmlInheritance.XmlInheritanceNode>> nodesByName = new Dictionary<string, List<XmlInheritance.XmlInheritanceNode>>();

		// Token: 0x040033B5 RID: 13237
		private const string NameAttributeName = "Name";

		// Token: 0x040033B6 RID: 13238
		private const string ParentNameAttributeName = "ParentName";

		// Token: 0x040033B7 RID: 13239
		private const string InheritAttributeName = "Inherit";

		// Token: 0x040033B8 RID: 13240
		private static HashSet<string> tempUsedNodeNames = new HashSet<string>();

		// Token: 0x02000D8D RID: 3469
		private class XmlInheritanceNode
		{
			// Token: 0x040033BA RID: 13242
			public XmlNode xmlNode;

			// Token: 0x040033BB RID: 13243
			public XmlNode resolvedXmlNode;

			// Token: 0x040033BC RID: 13244
			public ModContentPack mod;

			// Token: 0x040033BD RID: 13245
			public XmlInheritance.XmlInheritanceNode parent;

			// Token: 0x040033BE RID: 13246
			public List<XmlInheritance.XmlInheritanceNode> children = new List<XmlInheritance.XmlInheritanceNode>();
		}
	}
}
