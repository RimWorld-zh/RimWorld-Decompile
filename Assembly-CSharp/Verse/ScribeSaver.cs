using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D9F RID: 3487
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

		// Token: 0x06004DF7 RID: 19959 RVA: 0x0028BDE8 File Offset: 0x0028A1E8
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

		// Token: 0x06004DF8 RID: 19960 RVA: 0x0028BEF0 File Offset: 0x0028A2F0
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

		// Token: 0x06004DF9 RID: 19961 RVA: 0x0028BFF4 File Offset: 0x0028A3F4
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

		// Token: 0x06004DFA RID: 19962 RVA: 0x0028C09B File Offset: 0x0028A49B
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

		// Token: 0x06004DFB RID: 19963 RVA: 0x0028C0C8 File Offset: 0x0028A4C8
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

		// Token: 0x06004DFC RID: 19964 RVA: 0x0028C1DC File Offset: 0x0028A5DC
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

		// Token: 0x06004DFD RID: 19965 RVA: 0x0028C284 File Offset: 0x0028A684
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

		// Token: 0x06004DFE RID: 19966 RVA: 0x0028C2F4 File Offset: 0x0028A6F4
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
