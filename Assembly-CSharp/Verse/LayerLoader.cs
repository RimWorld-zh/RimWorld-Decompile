using System;
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
			foreach (XPathNavigator item2 in xPathNavigator.Select("Node"))
			{
				try
				{
					TextReader textReader2 = new StringReader(item2.OuterXml);
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(DiaNodeMold));
					DiaNodeMold diaNodeMold = (DiaNodeMold)xmlSerializer.Deserialize(textReader2);
					diaNodeMold.nodeType = NodesType;
					NodeListToFill.Add(diaNodeMold);
					textReader2.Dispose();
				}
				catch (Exception ex)
				{
					Log.Message("Exception deserializing " + item2.OuterXml + ":\n" + ex.InnerException);
					goto end_IL_0096;
					IL_00d0:
					end_IL_0096:;
				}
			}
			foreach (XPathNavigator item3 in xPathNavigator.Select("NodeList"))
			{
				try
				{
					TextReader textReader3 = new StringReader(item3.OuterXml);
					XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(DiaNodeList));
					DiaNodeList item = (DiaNodeList)xmlSerializer2.Deserialize(textReader3);
					ListListToFill.Add(item);
				}
				catch (Exception ex2)
				{
					Log.Message("Exception deserializing " + item3.OuterXml + ":\n" + ex2.InnerException);
					goto end_IL_015d;
					IL_0198:
					end_IL_015d:;
				}
			}
		}

		public static void MarkNonRootNodes(List<DiaNodeMold> NodeList)
		{
			List<DiaNodeMold>.Enumerator enumerator = NodeList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DiaNodeMold current = enumerator.Current;
					LayerLoader.RecursiveSetIsRootFalse(current);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			List<DiaNodeMold>.Enumerator enumerator2 = NodeList.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					DiaNodeMold current2 = enumerator2.Current;
					List<DiaNodeMold>.Enumerator enumerator3 = NodeList.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							DiaNodeMold current3 = enumerator3.Current;
							List<DiaOptionMold>.Enumerator enumerator4 = current3.optionList.GetEnumerator();
							try
							{
								while (enumerator4.MoveNext())
								{
									DiaOptionMold current4 = enumerator4.Current;
									bool flag = false;
									List<string>.Enumerator enumerator5 = current4.ChildNodeNames.GetEnumerator();
									try
									{
										while (enumerator5.MoveNext())
										{
											string current5 = enumerator5.Current;
											if (current5 == current2.name)
											{
												flag = true;
											}
										}
									}
									finally
									{
										((IDisposable)(object)enumerator5).Dispose();
									}
									if (flag)
									{
										current2.isRoot = false;
									}
								}
							}
							finally
							{
								((IDisposable)(object)enumerator4).Dispose();
							}
						}
					}
					finally
					{
						((IDisposable)(object)enumerator3).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
		}

		private static void RecursiveSetIsRootFalse(DiaNodeMold d)
		{
			List<DiaOptionMold>.Enumerator enumerator = d.optionList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DiaOptionMold current = enumerator.Current;
					List<DiaNodeMold>.Enumerator enumerator2 = current.ChildNodes.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							DiaNodeMold current2 = enumerator2.Current;
							current2.isRoot = false;
							LayerLoader.RecursiveSetIsRootFalse(current2);
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
