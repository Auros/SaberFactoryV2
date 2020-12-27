﻿using System;
using SaberFactory.DataStore;
using SaberFactory.Loaders;
using UnityEngine;

namespace SaberFactory.Models
{
    /// <summary>
    /// Model related to everything that makes up a saber
    /// like parts, halos, accessories, custom sabers
    /// </summary>
    internal class BasePieceModel : IDisposable
    {
        public ModelComposition ModelComposition { get; set; }

        public readonly StoreAsset StoreAsset;

        public GameObject Prefab => StoreAsset.Prefab;

        protected BasePieceModel(StoreAsset storeAsset)
        {
            StoreAsset = storeAsset;
        }

        public virtual void Dispose()
        {
        }
    }
}