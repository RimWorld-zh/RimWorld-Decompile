using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D9E RID: 3486
	public class ScribeLoader
	{
		// Token: 0x06004DD0 RID: 19920 RVA: 0x0028A040 File Offset: 0x00288440
		public void InitLoading(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoading() but current mode is " + Scribe.mode, false);
				Scribe.ForceStop();
			}
			if (this.curParent != null)
			{
				Log.Error("Current parent is not null in InitLoading", false);
				this.curParent = null;
			}
			if (this.curPathRelToParent != null)
			{
				Log.Error("Current path relative to parent is not null in InitLoading", false);
				this.curPathRelToParent = null;
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(xmlTextReader);
						this.curXmlParent = xmlDocument.DocumentElement;
					}
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init loading file: ",
					filePath,
					"\n",
					ex
				}), false);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x0028A170 File Offset: 0x00288570
		public void InitLoadingMetaHeaderOnly(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoadingMetaHeaderOnly() but current mode is " + Scribe.mode, false);
				Scribe.ForceStop();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						if (!ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader))
						{
							return;
						}
						using (XmlReader xmlReader = xmlTextReader.ReadSubtree())
						{
							XmlDocument xmlDocument = new XmlDocument();
							xmlDocument.Load(xmlReader);
							XmlElement xmlElement = xmlDocument.CreateElement("root");
							xmlElement.AppendChild(xmlDocument.DocumentElement);
							this.curXmlParent = xmlElement;
						}
					}
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init loading meta header: ",
					filePath,
					"\n",
					ex
				}), false);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x0028A2B8 File Offset: 0x002886B8
		public void FinalizeLoading()
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called FinalizeLoading() but current mode is " + Scribe.mode, false);
			}
			else
			{
				try
				{
					Scribe.ExitNode();
					this.curXmlParent = null;
					this.curParent = null;
					this.curPathRelToParent = null;
					Scribe.mode = LoadSaveMode.Inactive;
					this.crossRefs.ResolveAllCrossReferences();
					this.initer.DoAllPostLoadInits();
				}
				catch (Exception arg)
				{
					Log.Error("Exception in FinalizeLoading(): " + arg, false);
					this.ForceStop();
					throw;
				}
			}
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x0028A35C File Offset: 0x0028875C
		public bool EnterNode(string nodeName)
		{
			if (this.curXmlParent != null)
			{
				XmlNode xmlNode = this.curXmlParent[nodeName];
				if (xmlNode == null && char.IsDigit(nodeName[0]))
				{
					xmlNode = this.curXmlParent.ChildNodes[int.Parse(nodeName)];
				}
				if (xmlNode == null)
				{
					return false;
				}
				this.curXmlParent = xmlNode;
			}
			this.curPathRelToParent = this.curPathRelToParent + '/' + nodeName;
			return true;
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x0028A3E8 File Offset: 0x002887E8
		public void ExitNode()
		{
			if (this.curXmlParent != null)
			{
				this.curXmlParent = this.curXmlParent.ParentNode;
			}
			if (this.curPathRelToParent != null)
			{
				int num = this.curPathRelToParent.LastIndexOf('/');
				this.curPathRelToParent = ((num <= 0) ? null : this.curPathRelToParent.Substring(0, num));
			}
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x0028A450 File Offset: 0x00288850
		public void ForceStop()
		{
			this.curXmlParent = null;
			this.curParent = null;
			this.curPathRelToParent = null;
			this.crossRefs.Clear(false);
			this.initer.Clear();
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x040033E6 RID: 13286
		public CrossRefHandler crossRefs = new CrossRefHandler();

		// Token: 0x040033E7 RID: 13287
		public PostLoadIniter initer = new PostLoadIniter();

		// Token: 0x040033E8 RID: 13288
		public IExposable curParent;

		// Token: 0x040033E9 RID: 13289
		public XmlNode curXmlParent;

		// Token: 0x040033EA RID: 13290
		public string curPathRelToParent;
	}
}
