using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Verse
{
	public static class DirectXmlLoader
	{
		private static LoadableXmlAsset loadingAsset;

		[DebuggerHidden]
		public static IEnumerable<LoadableXmlAsset> XmlAssetsInModFolder(ModContentPack mod, string folderPath)
		{
			DirectXmlLoader.<XmlAssetsInModFolder>c__Iterator222 <XmlAssetsInModFolder>c__Iterator = new DirectXmlLoader.<XmlAssetsInModFolder>c__Iterator222();
			<XmlAssetsInModFolder>c__Iterator.mod = mod;
			<XmlAssetsInModFolder>c__Iterator.folderPath = folderPath;
			<XmlAssetsInModFolder>c__Iterator.<$>mod = mod;
			<XmlAssetsInModFolder>c__Iterator.<$>folderPath = folderPath;
			DirectXmlLoader.<XmlAssetsInModFolder>c__Iterator222 expr_23 = <XmlAssetsInModFolder>c__Iterator;
			expr_23.$PC = -2;
			return expr_23;
		}

		[DebuggerHidden]
		public static IEnumerable<T> LoadXmlDataInResourcesFolder<T>(string folderPath) where T : new()
		{
			DirectXmlLoader.<LoadXmlDataInResourcesFolder>c__Iterator223<T> <LoadXmlDataInResourcesFolder>c__Iterator = new DirectXmlLoader.<LoadXmlDataInResourcesFolder>c__Iterator223<T>();
			<LoadXmlDataInResourcesFolder>c__Iterator.folderPath = folderPath;
			<LoadXmlDataInResourcesFolder>c__Iterator.<$>folderPath = folderPath;
			DirectXmlLoader.<LoadXmlDataInResourcesFolder>c__Iterator223<T> expr_15 = <LoadXmlDataInResourcesFolder>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		public static T ItemFromXmlFile<T>(string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (resolveCrossRefs && DirectXmlCrossRefLoader.LoadingInProgress)
			{
				Log.Error("Cannot call ItemFromXmlFile with resolveCrossRefs=true while loading is already in progress.");
			}
			FileInfo fileInfo = new FileInfo(filePath);
			if (!fileInfo.Exists)
			{
				return (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
			}
			T result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(File.ReadAllText(fileInfo.FullName));
				T t = DirectXmlToObject.ObjectFromXml<T>(xmlDocument.DocumentElement, false);
				if (resolveCrossRefs)
				{
					DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
				}
				result = t;
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading file at " + filePath + ". Loading defaults instead. Exception was: " + ex.ToString());
				result = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			}
			return result;
		}

		[DebuggerHidden]
		public static IEnumerable<Def> AllDefsFromAsset(LoadableXmlAsset asset)
		{
			DirectXmlLoader.<AllDefsFromAsset>c__Iterator224 <AllDefsFromAsset>c__Iterator = new DirectXmlLoader.<AllDefsFromAsset>c__Iterator224();
			<AllDefsFromAsset>c__Iterator.asset = asset;
			<AllDefsFromAsset>c__Iterator.<$>asset = asset;
			DirectXmlLoader.<AllDefsFromAsset>c__Iterator224 expr_15 = <AllDefsFromAsset>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		[DebuggerHidden]
		public static IEnumerable<T> AllGameItemsFromAsset<T>(LoadableXmlAsset asset) where T : new()
		{
			DirectXmlLoader.<AllGameItemsFromAsset>c__Iterator225<T> <AllGameItemsFromAsset>c__Iterator = new DirectXmlLoader.<AllGameItemsFromAsset>c__Iterator225<T>();
			<AllGameItemsFromAsset>c__Iterator.asset = asset;
			<AllGameItemsFromAsset>c__Iterator.<$>asset = asset;
			DirectXmlLoader.<AllGameItemsFromAsset>c__Iterator225<T> expr_15 = <AllGameItemsFromAsset>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}
	}
}
