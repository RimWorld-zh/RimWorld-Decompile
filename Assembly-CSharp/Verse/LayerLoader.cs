using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC6 RID: 3782
	public static class LayerLoader
	{
		// Token: 0x06005951 RID: 22865 RVA: 0x002DBDB4 File Offset: 0x002DA1B4
		public static void LoadFileIntoList(TextAsset ass, List<DiaNodeMold> NodeListToFill, List<DiaNodeList> ListListToFill, DiaNodeType NodesType)
		{
			TextReader reader = new StringReader(ass.text);
			XPathDocument xpathDocument = new XPathDocument(reader);
			XPathNavigator xpathNavigator = xpathDocument.CreateNavigator();
			xpathNavigator.MoveToFirst();
			xpathNavigator.MoveToFirstChild();
			IEnumerator enumerator = xpathNavigator.Select("Node").GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XPathNavigator xpathNavigator2 = (XPathNavigator)obj;
					try
					{
						TextReader textReader = new StringReader(xpathNavigator2.OuterXml);
						XmlSerializer xmlSerializer = new XmlSerializer(typeof(DiaNodeMold));
						DiaNodeMold diaNodeMold = (DiaNodeMold)xmlSerializer.Deserialize(textReader);
						diaNodeMold.nodeType = NodesType;
						NodeListToFill.Add(diaNodeMold);
						textReader.Dispose();
					}
					catch (Exception ex)
					{
						Log.Message(string.Concat(new object[]
						{
							"Exception deserializing ",
							xpathNavigator2.OuterXml,
							":\n",
							ex.InnerException
						}), false);
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
			IEnumerator enumerator2 = xpathNavigator.Select("NodeList").GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					XPathNavigator xpathNavigator3 = (XPathNavigator)obj2;
					try
					{
						TextReader textReader2 = new StringReader(xpathNavigator3.OuterXml);
						XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(DiaNodeList));
						DiaNodeList item = (DiaNodeList)xmlSerializer2.Deserialize(textReader2);
						ListListToFill.Add(item);
					}
					catch (Exception ex2)
					{
						Log.Message(string.Concat(new object[]
						{
							"Exception deserializing ",
							xpathNavigator3.OuterXml,
							":\n",
							ex2.InnerException
						}), false);
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

		// Token: 0x06005952 RID: 22866 RVA: 0x002DBFC0 File Offset: 0x002DA3C0
		public static void MarkNonRootNodes(List<DiaNodeMold> NodeList)
		{
			foreach (DiaNodeMold d in NodeList)
			{
				LayerLoader.RecursiveSetIsRootFalse(d);
			}
			foreach (DiaNodeMold diaNodeMold in NodeList)
			{
				foreach (DiaNodeMold diaNodeMold2 in NodeList)
				{
					foreach (DiaOptionMold diaOptionMold in diaNodeMold2.optionList)
					{
						bool flag = false;
						foreach (string a in diaOptionMold.ChildNodeNames)
						{
							if (a == diaNodeMold.name)
							{
								flag = true;
							}
						}
						if (flag)
						{
							diaNodeMold.isRoot = false;
						}
					}
				}
			}
		}

		// Token: 0x06005953 RID: 22867 RVA: 0x002DC15C File Offset: 0x002DA55C
		private static void RecursiveSetIsRootFalse(DiaNodeMold d)
		{
			foreach (DiaOptionMold diaOptionMold in d.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.isRoot = false;
					LayerLoader.RecursiveSetIsRootFalse(diaNodeMold);
				}
			}
		}
	}
}
