using System;
using System.IO;
using System.Xml;

namespace Verse
{
	public class ScribeSaver
	{
		public DebugLoadIDsSavingErrorsChecker loadIDsErrorsChecker = new DebugLoadIDsSavingErrorsChecker();

		public bool savingForDebug;

		private Stream saveStream;

		private XmlWriter writer;

		public void InitSaving(string filePath, string documentElementName)
		{
			if (Scribe.mode != 0)
			{
				Log.Error("Called InitSaving() but current mode is " + Scribe.mode);
				Scribe.ForceStop();
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
				Log.Error("Exception while init saving file: " + filePath + "\n" + ex);
				this.ForceStop();
				throw;
			}
		}

		public void FinalizeSaving()
		{
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Called FinalizeSaving() but current mode is " + Scribe.mode);
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
				}
				catch (Exception arg)
				{
					Log.Error("Exception in FinalizeLoading(): " + arg);
					this.ForceStop();
					throw;
				}
			}
		}

		public void WriteElement(string elementName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteElemenet(), but writer is null.");
			}
			else
			{
				this.writer.WriteElementString(elementName, value);
			}
		}

		public void WriteAttribute(string attributeName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteAttribute(), but writer is null.");
			}
			else
			{
				this.writer.WriteAttributeString(attributeName, value);
			}
		}

		public string DebugOutputFor(IExposable saveable)
		{
			if (Scribe.mode != 0)
			{
				Log.Error("DebugOutput needs current mode to be Inactive");
				return string.Empty;
			}
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
							Scribe_Deep.Look(ref saveable, "saveable");
							return stringWriter.ToString();
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
				Log.Error("Exception while getting debug output: " + arg);
				this.ForceStop();
				return string.Empty;
			}
		}

		public bool EnterNode(string nodeName)
		{
			if (this.writer == null)
			{
				return false;
			}
			this.writer.WriteStartElement(nodeName);
			return true;
		}

		public void ExitNode()
		{
			if (this.writer != null)
			{
				this.writer.WriteEndElement();
			}
		}

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
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}
	}
}
