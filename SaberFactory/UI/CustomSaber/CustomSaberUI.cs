﻿using SaberFactory.Models;
using SaberFactory.UI.CustomSaber.Views;
using SaberFactory.UI.Lib;
using SiraUtil.Tools;
using UnityEngine;
using VRUIControls;
using Zenject;

namespace SaberFactory.UI.CustomSaber
{
    internal class CustomSaberUI : SaberFactoryUI
    {
        private readonly SaberSet _saberSet;

        #region Views
        private MainView _mainView;
        private NavigationView _navigationView;
        #endregion

        protected CustomSaberUI(
            SiraLog logger,
            CustomScreen.Factory screenFactory,
            BaseGameUiHandler baseGameUiHandler,
            PhysicsRaycasterWithCache physicsRaycaster,
            SaberSet saberSet)
            : base(logger, screenFactory, baseGameUiHandler, physicsRaycaster)
        {
            _saberSet = saberSet;
        }

        public override void SetupUI()
        {
            var mainScreenInitData = new CustomScreen.InitData
            (
                "Main Screen",
                new Vector3(-25, -10, 0),
                Quaternion.identity, 
                new Vector2(105, 140),
                isCurved: true
            );

            var navigationInitData = new CustomScreen.InitData(
                "Navigation Screen",
                new Vector3(-90, 0, 0),
                Quaternion.Euler(0, 305, 0),
                new Vector2(30, 70),
                isCurved: true
            );

            _mainView = AddScreen(mainScreenInitData).CreateViewController<MainView>();
            _navigationView = AddScreen(navigationInitData).CreateViewController<NavigationView>();
        }

        protected override void DidOpen()
        {
            base.DidOpen();
            _navigationView.OnExit += ClosePressed;
            _navigationView.OnCategoryChanged += _mainView.ChangeCategory;
        }

        protected override void DidClose()
        {
            base.DidClose();
            _navigationView.OnExit -= ClosePressed;
            _navigationView.OnCategoryChanged -= _mainView.ChangeCategory;

            _saberSet.Save();
        }
    }
}