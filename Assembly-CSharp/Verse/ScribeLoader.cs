using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D9D RID: 3485
	public class ScribeLoader
	{
		// Token: 0x040033F1 RID: 13297
		public CrossRefHandler crossRefs = new CrossRefHandler();

		// Token: 0x040033F2 RID: 13298
		public PostLoadIniter initer = new PostLoadIniter();

		// Token: 0x040033F3 RID: 13299
		public IExposable curParent;

		// Token: 0x040033F4 RID: 13300
		public XmlNode curXmlParent;

		// Token: 0x040033F5 RID: 13301
		public string curPathRelToParent;

		// Token: 0x06004DE9 RID: 19945 RVA: 0x0028B71C File Offset: 0x00289B1C
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

		// Token: 0x06004DEA RID: 19946 RVA: 0x0028B84C File Offset: 0x00289C4C
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

		// Token: 0x06004DEB RID: 19947 RVA: 0x0028B994 File Offset: 0x00289D94
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

		// Token: 0x06004DEC RID: 19948 RVA: 0x0028BA38 File Offset: 0x00289E38
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

		// Token: 0x06004DED RID: 19949 RVA: 0x0028BAC4 File Offset: 0x00289EC4
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

		// Token: 0x06004DEE RID: 19950 RVA: 0x0028BB2C File Offset: 0x00289F2C
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
	}
}
