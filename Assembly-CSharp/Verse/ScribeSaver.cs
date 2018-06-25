using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000DA1 RID: 3489
	public class ScribeSaver
	{
		// Token: 0x04003404 RID: 13316
		public DebugLoadIDsSavingErrorsChecker loadIDsErrorsChecker = new DebugLoadIDsSavingErrorsChecker();

		// Token: 0x04003405 RID: 13317
		public bool savingForDebug;

		// Token: 0x04003406 RID: 13318
		private Stream saveStream;

		// Token: 0x04003407 RID: 13319
		private XmlWriter writer;

		// Token: 0x04003408 RID: 13320
		private string curPath;

		// Token: 0x04003409 RID: 13321
		private HashSet<string> savedNodes = new HashSet<string>();

		// Token: 0x0400340A RID: 13322
		private int nextListElementTemporaryId;

		// Token: 0x06004DFB RID: 19963 RVA: 0x0028BF14 File Offset: 0x0028A314
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

		// Token: 0x06004DFC RID: 19964 RVA: 0x0028C01C File Offset: 0x0028A41C
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

		// Token: 0x06004DFD RID: 19965 RVA: 0x0028C120 File Offset: 0x0028A520
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

		// Token: 0x06004DFE RID: 19966 RVA: 0x0028C1C7 File Offset: 0x0028A5C7
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

		// Token: 0x06004DFF RID: 19967 RVA: 0x0028C1F4 File Offset: 0x0028A5F4
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

		// Token: 0x06004E00 RID: 19968 RVA: 0x0028C308 File Offset: 0x0028A708
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

		// Token: 0x06004E01 RID: 19969 RVA: 0x0028C3B0 File Offset: 0x0028A7B0
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

		// Token: 0x06004E02 RID: 19970 RVA: 0x0028C420 File Offset: 0x0028A820
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
	}
}
