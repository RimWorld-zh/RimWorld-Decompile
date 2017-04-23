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
			DirectXmlLoader.<XmlAssetsInModFolder>c__Iterator21C <XmlAssetsInModFolder>c__Iterator21C = new DirectXmlLoader.<XmlAssetsInModFolder>c__Iterator21C();
			<XmlAssetsInModFolder>c__Iterator21C.mod = mod;
			<XmlAssetsInModFolder>c__Iterator21C.folderPath = folderPath;
			<XmlAssetsInModFolder>c__Iterator21C.<$>mod = mod;
			<XmlAssetsInModFolder>c__Iterator21C.<$>folderPath = folderPath;
			DirectXmlLoader.<XmlAssetsInModFolder>c__Iterator21C expr_23 = <XmlAssetsInModFolder>c__Iterator21C;
			expr_23.$PC = -2;
			return expr_23;
		}

		[DebuggerHidden]
		public static IEnumerable<T> LoadXmlDataInResourcesFolder<T>(string folderPath) where T : new()
		{
			DirectXmlLoader.<LoadXmlDataInResourcesFolder>c__Iterator21D<T> <LoadXmlDataInResourcesFolder>c__Iterator21D = new DirectXmlLoader.<LoadXmlDataInResourcesFolder>c__Iterator21D<T>();
			<LoadXmlDataInResourcesFolder>c__Iterator21D.folderPath = folderPath;
			<LoadXmlDataInResourcesFolder>c__Iterator21D.<$>folderPath = folderPath;
			DirectXmlLoader.<LoadXmlDataInResourcesFolder>c__Iterator21D<T> expr_15 = <LoadXmlDataInResourcesFolder>c__Iterator21D;
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
			DirectXmlLoader.<AllDefsFromAsset>c__Iterator21E <AllDefsFromAsset>c__Iterator21E = new DirectXmlLoader.<AllDefsFromAsset>c__Iterator21E();
			<AllDefsFromAsset>c__Iterator21E.asset = asset;
			<AllDefsFromAsset>c__Iterator21E.<$>asset = asset;
			DirectXmlLoader.<AllDefsFromAsset>c__Iterator21E expr_15 = <AllDefsFromAsset>c__Iterator21E;
			expr_15.$PC = -2;
			return expr_15;
		}

		[DebuggerHidden]
		public static IEnumerable<T> AllGameItemsFromAsset<T>(LoadableXmlAsset asset) where T : new()
		{
			DirectXmlLoader.<AllGameItemsFromAsset>c__Iterator21F<T> <AllGameItemsFromAsset>c__Iterator21F = new DirectXmlLoader.<AllGameItemsFromAsset>c__Iterator21F<T>();
			<AllGameItemsFromAsset>c__Iterator21F.asset = asset;
			<AllGameItemsFromAsset>c__Iterator21F.<$>asset = asset;
			DirectXmlLoader.<AllGameItemsFromAsset>c__Iterator21F<T> expr_15 = <AllGameItemsFromAsset>c__Iterator21F;
			expr_15.$PC = -2;
			return expr_15;
		}
	}
}
