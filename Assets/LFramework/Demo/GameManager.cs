using System;
using System.Collections;
using System.Collections.Generic;
using Command;
using LFramework;
using Model;
using UnityEngine;

namespace Game.Demo
{
    /// <summary>
    /// 架构
    /// </summary>
    public class GameApp : Architecture<GameApp>
    {
        protected override void Init()
        {
            // 注册model
            // this.RegisterModel(new CountDownModel());
            // this.RegisterModel(new ChooseRoleNameModel());
            // this.RegisterModel(new T2dListModel());
            // this.RegisterModel(new EnterFaceFusionModel());
            // this.RegisterModel(new EnterWritePanelModel());

            // 注册utility
            // this.RegisterUtility(new Storage());
            // this.RegisterUtility(new TencentV3());
            // this.RegisterUtility(new MattingPeople());
            // this.RegisterUtility(new DownloadImage());

            // 注册system
            // this.RegisterSystem(new ShotPicSystem());

            // this.RegisterModel(new EnterThreePanelModel());
            // this.RegisterModel(new EnterTwoPanelModel());
        }
    }

    public class GameManager : MonoSingleton<GameManager>, IController
    {
        private void Start()
        {
            // this.GetModel<>()
            // this.GetUtility<>()
            // this.SendCommand()
        }


        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }
    }
}