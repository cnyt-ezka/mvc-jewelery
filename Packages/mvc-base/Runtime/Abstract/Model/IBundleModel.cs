using System.Collections.Generic;
using MVC.Base.Runtime.Concrete.Data.ValueObject;
using MVC.Base.Runtime.Concrete.Promise;
using UnityEngine;

namespace MVC.Base.Runtime.Abstract.Model
{
    public interface IBundleModel
    {
        void AddBundleData(string key, BundleDataVO vo);

        Dictionary<string, BundleDataVO> GetBundleDatas();

        IPromise<BundleLoadData> LoadBundle(string name, string path, bool load = false);

        IPromise<BundleLoadData> LoadBundle(BundleLoadData loadData);

        void Clear(string name, bool clearAll = true);

        void ClearLayers(string[] names);

        BundleLoadData GetBundleByName(string name);

        GameObject GetPrefabByAssetName(string bundleKey, string assetKey);

        void SetBundleData(Dictionary<string, BundleDataVO> dir);

 	    void AddAssetBundleData(string key, Dictionary<int, string> val);

        Dictionary<string, Dictionary<int, string>> GetChaptersAndLevels();

        void SetAssetBundleData(Dictionary<string, Dictionary<int, string>> val);

    }
}