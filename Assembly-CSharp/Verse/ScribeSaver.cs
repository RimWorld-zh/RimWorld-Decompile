using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000DA3 RID: 3491
	public class ScribeSaver
	{
		// Token: 0x06004DE4 RID: 19940 RVA: 0x0028A858 File Offset: 0x00288C58
		public void InitSaving(string filePath, string documentElementName)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitSaving() but current mode is " + Scribe.mode, false);
				Scribe.ForceStop();
			}
			if (this.curPath != null)
			{
				Log.Error("Current path is not null in InitSaving", false);
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
			}
			try
			{
				Scribe.mode = LoadSaveMode.Saving;
				this.saveStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Indent = true;
				xmlWriterSettings.IndentChars = "\t";
				this.writer = XmlWriter.Create(this.saveStream, xmlWriterSettings);
				this.writer.WriteStartDocument();
				this.EnterNode(documentElementName);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init saving file: ",
					filePath,
					"\n",
					ex
				}), false);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x0028A960 File Offset: 0x00288D60
		public void FinalizeSaving()
		{
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Called FinalizeSaving() but current mode is " + Scribe.mode, false);
			}
			else
			{
				try
				{
					if (this.writer != null)
					{
						this.ExitNode();
						this.writer.WriteEndDocument();
						this.writer.Flush();
						this.writer.Close();
						this.writer = null;
					}
					if (this.saveStream != null)
					{
						this.saveStream.Flush();
						this.saveStream.Close();
						this.saveStream = null;
					}
					Scribe.mode = LoadSaveMode.Inactive;
					this.savingForDebug = false;
					this.loadIDsErrorsChecker.CheckForErrorsAndClear();
					this.curPath = null;
					this.savedNodes.Clear();
					this.nextListElementTemporaryId = 0;
				}
				catch (Exception arg)
				{
					Log.Error("Exception in FinalizeLoading(): " + arg, false);
					this.ForceStop();
					throw;
				}
			}
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0028AA64 File Offset: 0x00288E64
		public void WriteElement(string elementName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteElemenet(), but writer is null.", false);
			}
			else
			{
				if (UnityData.isDebugBuild && elementName != "li")
				{
					string text = this.curPath + "/" + elementName;
					if (!this.savedNodes.Add(text))
					{
						Log.Warning(string.Concat(new string[]
						{
							"Trying to save 2 XML nodes with the same name \"",
							elementName,
							"\" path=\"",
							text,
							"\""
						}), false);
					}
				}
				this.writer.WriteElementString(elementName, value);
			}
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x0028AB0B File Offset: 0x00288F0B
		public void WriteAttribute(string attributeName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteAttribute(), but writer is null.", false);
			}
			else
			{
				this.writer.WriteAttributeString(attributeName, value);
			}
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0028AB38 File Offset: 0x00288F38
		public string DebugOutputFor(IExposable saveable)
		{
			string result;
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("DebugOutput needs current mode to be Inactive", false);
				result = "";
			}
			else
			{
				try
				{
					using (StringWriter stringWriter = new StringWriter())
					{
						XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
						xmlWriterSettings.Indent = true;
						xmlWriterSettings.IndentChars = "  ";
						xmlWriterSettings.OmitXmlDeclaration = true;
						try
						{
							using (this.writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
							{
								Scribe.mode = LoadSaveMode.Saving;
								this.savingForDebug = true;
								Scribe_Deep.Look<IExposable>(ref saveable, "saveable", new object[0]);
								result = stringWriter.ToString();
							}
						}
						finally
						{
							this.ForceStop();
						}
					}
				}
				catch (Exception arg)
				{
					Log.Error("Exception while getting debug output: " + arg, false);
					this.ForceStop();
					result = "";
				}
			}
			return result;
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x0028AC4C File Offset: 0x0028904C
		public bool EnterNode(string nodeName)
		{
			bool result;
			if (this.writer == null)
			{
				result = false;
			}
			else
			{
				this.writer.WriteStartElement(nodeName);
				if (UnityData.isDebugBuild)
				{
					this.curPath = this.curPath + "/" + nodeName;
					if (nodeName == "li" || nodeName == "thing")
					{
						this.curPath = this.curPath + "_" + this.nextListElementTemporaryId;
						this.nextListElementTemporaryId++;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x0028ACF4 File Offset: 0x002890F4
		public void ExitNode()
		{
			if (this.writer != null)
			{
				this.writer.WriteEndElement();
				if (UnityData.isDebugBuild && this.curPath != null)
				{
					int num = this.curPath.LastIndexOf('/');
					this.curPath = ((num <= 0) ? null : this.curPath.Substring(0, num));
				}
			}
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x0028AD64 File Offset: 0x00289164
		public void ForceStop()
		{
			if (this.writer != null)
			{
				this.writer.Close();
				this.writer = null;
			}
			if (this.saveStream != null)
			{
				this.saveStream.Close();
				this.saveStream = null;
			}
			this.savingForDebug = false;
			this.loadIDsErrorsChecker.Clear();
			this.curPath = null;
			this.savedNodes.Clear();
			this.nextListElementTemporaryId = 0;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x040033FB RID: 13307
		public DebugLoadIDsSavingErrorsChecker loadIDsErrorsChecker = new DebugLoadIDsSavingErrorsChecker();

		// Token: 0x040033FC RID: 13308
		public bool savingForDebug;

		// Token: 0x040033FD RID: 13309
		private Stream saveStream;

		// Token: 0x040033FE RID: 13310
		private XmlWriter writer;

		// Token: 0x040033FF RID: 13311
		private string curPath;

		// Token: 0x04003400 RID: 13312
		private HashSet<string> savedNodes = new HashSet<string>();

		// Token: 0x04003401 RID: 13313
		private int nextListElementTemporaryId;
	}
}
