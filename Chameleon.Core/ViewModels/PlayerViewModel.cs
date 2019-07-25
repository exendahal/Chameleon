﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using MediaManager;
using MediaManager.Media;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Xamarin.Forms;

namespace Chameleon.Core.ViewModels
{
    public class PlayerViewModel : BaseViewModel<IMediaItem>
    {
        public PlayerViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IMediaManager mediaManager) : base(logProvider, navigationService)
        {
            MediaManager = mediaManager ?? throw new ArgumentNullException(nameof(mediaManager));
            MediaManager.PositionChanged += MediaManager_PositionChanged;
        }

        private void MediaManager_PositionChanged(object sender, MediaManager.Playback.PositionChangedEventArgs e)
        {
            float percentComplete = (float)Math.Round((double)(100 * e.Position.TotalSeconds) / MediaManager.Duration.TotalSeconds) / 100;
            Position = percentComplete;
        }

        private IMediaItem _source;
        public IMediaItem Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public IMediaManager MediaManager { get; }

        private bool _showControls;
        public bool ShowControls
        {
            get => _showControls;
            set => SetProperty(ref _showControls, value);
        }

        private double _position;
        public double Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private IMvxAsyncCommand _previousCommand;
        public IMvxAsyncCommand PreviousCommand => _previousCommand ?? (_previousCommand = new MvxAsyncCommand(() => MediaManager.PlayPrevious()));

        private IMvxAsyncCommand _skipBackwardsCommand;
        public IMvxAsyncCommand SkipBackwardsCommand => _skipBackwardsCommand ?? (_skipBackwardsCommand = new MvxAsyncCommand(() => MediaManager.StepBackward()));

        private IMvxAsyncCommand _playCommand;
        public IMvxAsyncCommand PlayCommand => _playCommand ?? (_playCommand = new MvxAsyncCommand(() => MediaManager.PlayPause()));

        private IMvxAsyncCommand _skipForwardCommand;
        public IMvxAsyncCommand SkipForwardCommand => _skipForwardCommand ?? (_skipForwardCommand = new MvxAsyncCommand(() => MediaManager.StepForward()));

        private IMvxAsyncCommand _nextCommand;
        public IMvxAsyncCommand NextCommand => _nextCommand ?? (_nextCommand = new MvxAsyncCommand(() => MediaManager.PlayNext()));

        private IMvxCommand _controlsCommand;
        public IMvxCommand ControlsCommand => _controlsCommand ?? (_controlsCommand = new MvxCommand(ShowHideControls));

        private IMvxAsyncCommand _queueCommand;
        public IMvxAsyncCommand QueueCommand => _queueCommand ?? (_queueCommand = new MvxAsyncCommand(
            () => NavigationService.Navigate<QueueViewModel>()));

        public override void Prepare(IMediaItem parameter)
        {
            Source = parameter;
        }

        private void ShowHideControls()
        {
            ShowControls = !ShowControls;
        }
    }
}