using RimWorld;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Verse
{
	public static class LanguageDataWriter
	{
		public static void WriteBackstoryFile()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.DevOutputFolderPath);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			FileInfo fileInfo = new FileInfo(GenFilePaths.BackstoryOutputFilePath);
			if (fileInfo.Exists)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("Cannot write: File already exists at " + GenFilePaths.BackstoryOutputFilePath, (string)null, null, (string)null, null, (string)null, false));
			}
			else
			{
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Indent = true;
				xmlWriterSettings.IndentChars = "\t";
				using (XmlWriter xmlWriter = XmlWriter.Create(GenFilePaths.BackstoryOutputFilePath, xmlWriterSettings))
				{
					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement("BackstoryTranslations");
					foreach (KeyValuePair<string, Backstory> allBackstory in BackstoryDatabase.allBackstories)
					{
						Backstory value = allBackstory.Value;
						xmlWriter.WriteStartElement(value.identifier);
						xmlWriter.WriteElementString("title", value.Title);
						xmlWriter.WriteElementString("titleShort", value.TitleShort);
						xmlWriter.WriteElementString("desc", value.baseDesc);
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndDocument();
				}
				Messages.Message("Fresh backstory translation file saved to " + GenFilePaths.BackstoryOutputFilePath, MessageTypeDefOf.NeutralEvent);
			}
		}
	}
}
