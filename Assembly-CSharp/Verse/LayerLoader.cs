using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;
using UnityEngine;

namespace Verse
{
	public static class LayerLoader
	{
		public static void LoadFileIntoList(TextAsset ass, List<DiaNodeMold> NodeListToFill, List<DiaNodeList> ListListToFill, DiaNodeType NodesType)
		{
			TextReader textReader = new StringReader(ass.text);
			XPathDocument xPathDocument = new XPathDocument(textReader);
			XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();
			xPathNavigator.MoveToFirst();
			xPathNavigator.MoveToFirstChild();
			IEnumerator enumerator = xPathNavigator.Select("Node").GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					XPathNavigator xPathNavigator2 = (XPathNavigator)enumerator.Current;
					try
					{
						TextReader textReader2 = new StringReader(xPathNavigator2.OuterXml);
						XmlSerializer xmlSerializer = new XmlSerializer(typeof(DiaNodeMold));
						DiaNodeMold diaNodeMold = (DiaNodeMold)xmlSerializer.Deserialize(textReader2);
						diaNodeMold.nodeType = NodesType;
						NodeListToFill.Add(diaNodeMold);
						textReader2.Dispose();
					}
					catch (Exception ex)
					{
						Log.Message("Exception deserializing " + xPathNavigator2.OuterXml + ":\n" + ex.InnerException);
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
			IEnumerator enumerator2 = xPathNavigator.Select("NodeList").GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					XPathNavigator xPathNavigator3 = (XPathNavigator)enumerator2.Current;
					try
					{
						TextReader textReader3 = new StringReader(xPathNavigator3.OuterXml);
						XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(DiaNodeList));
						DiaNodeList item = (DiaNodeList)xmlSerializer2.Deserialize(textReader3);
						ListListToFill.Add(item);
					}
					catch (Exception ex2)
					{
						Log.Message("Exception deserializing " + xPathNavigator3.OuterXml + ":\n" + ex2.InnerException);
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

		public static void MarkNonRootNodes(List<DiaNodeMold> NodeList)
		{
			foreach (DiaNodeMold Node in NodeList)
			{
				LayerLoader.RecursiveSetIsRootFalse(Node);
			}
			foreach (DiaNodeMold Node2 in NodeList)
			{
				foreach (DiaNodeMold Node3 in NodeList)
				{
					foreach (DiaOptionMold option in Node3.optionList)
					{
						bool flag = false;
						foreach (string childNodeName in option.ChildNodeNames)
						{
							if (childNodeName == Node2.name)
							{
								flag = true;
							}
						}
						if (flag)
						{
							Node2.isRoot = false;
						}
					}
				}
			}
		}

		private static void RecursiveSetIsRootFalse(DiaNodeMold d)
		{
			foreach (DiaOptionMold option in d.optionList)
			{
				foreach (DiaNodeMold childNode in option.ChildNodes)
				{
					childNode.isRoot = false;
					LayerLoader.RecursiveSetIsRootFalse(childNode);
				}
			}
		}
	}
}
