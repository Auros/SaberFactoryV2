﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SaberFactory.Helpers;
using SaberFactory.Loaders;
using SaberFactory.Models;
using SaberFactory.Models.CustomSaber;
using SiraUtil.Tools;

namespace SaberFactory.DataStore
{
    internal class MainAssetStore
    {
        private readonly CustomSaberAssetLoader _customSaberAssetLoader;
        private readonly CustomSaberModelLoader _customSaberModelLoader;

        private readonly SiraLog _logger;

        private readonly Dictionary<string, ModelComposition> _modelCompositions;

        private MainAssetStore(
            SiraLog logger,
            CustomSaberModel.Factory customSaberModelFactory)
        {
            _logger = logger;

            _customSaberAssetLoader = new CustomSaberAssetLoader();
            _customSaberModelLoader = new CustomSaberModelLoader(customSaberModelFactory);

            _modelCompositions = new Dictionary<string, ModelComposition>();
        }

        public Task<ModelComposition> this[string name] => GetCompositionByPath(name);

        public async Task<ModelComposition> GetCompositionByPath(string path)
        {
            if (_modelCompositions.TryGetValue(path, out var result)) return result;

            var composition = await LoadModelCompositionAsync(path);
            _modelCompositions.Add(path, composition);
            return composition;
        }

        public async Task LoadAllCustomSabers()
        {
            foreach (var path in _customSaberAssetLoader.CollectFiles())
            {
                var relativePath = PathTools.ToRelativePath(path);
                _logger.Info(relativePath);
                if(_modelCompositions.ContainsKey(relativePath)) continue;

                var composition = await LoadModelCompositionAsync(relativePath);
                if(composition != null) _modelCompositions.Add(relativePath, composition);
            }
        }

        public void UnloadAll()
        {
            foreach (var modelCompositions in _modelCompositions.Values)
            {
                modelCompositions.Dispose();
            }
            _modelCompositions.Clear();
        }

        private void AddModelComposition(string key, ModelComposition modelComposition)
        {
            if(!_modelCompositions.ContainsKey(key)) _modelCompositions.Add(key, modelComposition);
        }

        private async Task<ModelComposition> LoadModelCompositionAsync(string bundlePath)
        {
            // TODO: Switch between customsaber and part implementation

            AssetBundleLoader loader = _customSaberAssetLoader;
            IStoreAssetParser modelCreator = _customSaberModelLoader;

            var storeAsset = await loader.LoadStoreAssetAsync(bundlePath);
            if (storeAsset == null) return null;
            var model = modelCreator.GetComposition(storeAsset);

            return model;
        }
    }
}