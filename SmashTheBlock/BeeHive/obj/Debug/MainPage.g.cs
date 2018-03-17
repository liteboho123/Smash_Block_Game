﻿#pragma checksum "C:\Users\cna\Desktop\SmashTheBlock\BeeHive\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "66202B2DFDDA4CABB5E1FD80DFA8CA94"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BeeHive;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace BeeHive {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.VisualStateGroup GameStatus;
        
        internal System.Windows.VisualState WaitStart;
        
        internal System.Windows.VisualState StartGame;
        
        internal System.Windows.VisualState ShowGameOver;
        
        internal System.Windows.VisualStateGroup Levels;
        
        internal System.Windows.VisualState Level1;
        
        internal System.Windows.VisualState Level2;
        
        internal System.Windows.VisualState Level3;
        
        internal System.Windows.VisualState GameOverLevel;
        
        internal System.Windows.Controls.Canvas BeeHive;
        
        internal BeeHive.GameEnvironment gameEnvironment;
        
        internal System.Windows.Shapes.Path background;
        
        internal System.Windows.Controls.Image grid;
        
        internal BeeHive.LevelHostControl levelHostControl;
        
        internal System.Windows.Controls.Image score;
        
        internal System.Windows.Controls.Image lives;
        
        internal System.Windows.Controls.Image LEFT_WALL;
        
        internal System.Windows.Controls.Image TOP_WALL;
        
        internal System.Windows.Controls.Image RIGHT_WALL;
        
        internal System.Windows.Controls.Image BOTTOM_WALL;
        
        internal System.Windows.Controls.Image BALL;
        
        internal System.Windows.Controls.Image PADDLE;
        
        internal System.Windows.Controls.TextBlock ScoreText;
        
        internal System.Windows.Controls.TextBlock LivesText;
        
        internal System.Windows.Controls.Image ClicktoStart;
        
        internal System.Windows.Controls.Image GameOver;
        
        internal System.Windows.Controls.TextBlock LastScoreText;
        
        internal System.Windows.Controls.TextBlock HiScoreText;
        
        internal System.Windows.Controls.Grid frame;
        
        internal System.Windows.Controls.Grid masterHead;
        
        internal System.Windows.Shapes.Rectangle masterHead_bg;
        
        internal System.Windows.Shapes.Rectangle footer;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/BeeHive;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.GameStatus = ((System.Windows.VisualStateGroup)(this.FindName("GameStatus")));
            this.WaitStart = ((System.Windows.VisualState)(this.FindName("WaitStart")));
            this.StartGame = ((System.Windows.VisualState)(this.FindName("StartGame")));
            this.ShowGameOver = ((System.Windows.VisualState)(this.FindName("ShowGameOver")));
            this.Levels = ((System.Windows.VisualStateGroup)(this.FindName("Levels")));
            this.Level1 = ((System.Windows.VisualState)(this.FindName("Level1")));
            this.Level2 = ((System.Windows.VisualState)(this.FindName("Level2")));
            this.Level3 = ((System.Windows.VisualState)(this.FindName("Level3")));
            this.GameOverLevel = ((System.Windows.VisualState)(this.FindName("GameOverLevel")));
            this.BeeHive = ((System.Windows.Controls.Canvas)(this.FindName("BeeHive")));
            this.gameEnvironment = ((BeeHive.GameEnvironment)(this.FindName("gameEnvironment")));
            this.background = ((System.Windows.Shapes.Path)(this.FindName("background")));
            this.grid = ((System.Windows.Controls.Image)(this.FindName("grid")));
            this.levelHostControl = ((BeeHive.LevelHostControl)(this.FindName("levelHostControl")));
            this.score = ((System.Windows.Controls.Image)(this.FindName("score")));
            this.lives = ((System.Windows.Controls.Image)(this.FindName("lives")));
            this.LEFT_WALL = ((System.Windows.Controls.Image)(this.FindName("LEFT_WALL")));
            this.TOP_WALL = ((System.Windows.Controls.Image)(this.FindName("TOP_WALL")));
            this.RIGHT_WALL = ((System.Windows.Controls.Image)(this.FindName("RIGHT_WALL")));
            this.BOTTOM_WALL = ((System.Windows.Controls.Image)(this.FindName("BOTTOM_WALL")));
            this.BALL = ((System.Windows.Controls.Image)(this.FindName("BALL")));
            this.PADDLE = ((System.Windows.Controls.Image)(this.FindName("PADDLE")));
            this.ScoreText = ((System.Windows.Controls.TextBlock)(this.FindName("ScoreText")));
            this.LivesText = ((System.Windows.Controls.TextBlock)(this.FindName("LivesText")));
            this.ClicktoStart = ((System.Windows.Controls.Image)(this.FindName("ClicktoStart")));
            this.GameOver = ((System.Windows.Controls.Image)(this.FindName("GameOver")));
            this.LastScoreText = ((System.Windows.Controls.TextBlock)(this.FindName("LastScoreText")));
            this.HiScoreText = ((System.Windows.Controls.TextBlock)(this.FindName("HiScoreText")));
            this.frame = ((System.Windows.Controls.Grid)(this.FindName("frame")));
            this.masterHead = ((System.Windows.Controls.Grid)(this.FindName("masterHead")));
            this.masterHead_bg = ((System.Windows.Shapes.Rectangle)(this.FindName("masterHead_bg")));
            this.footer = ((System.Windows.Shapes.Rectangle)(this.FindName("footer")));
        }
    }
}

