﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chameleon.Services.Media;
using Chameleon.Services.Resources;
using MediaManager.Media;

namespace Chameleon.Services.Services
{
    public class PlaylistService : IPlaylistService
    {
        public PlaylistService()
        {
        }

        public Task<IList<IMediaItem>> GetPlaylist()
        {
            var json = ExoPlayerSamples.GetEmbeddedResourceString("media.exolist.json");
            var jsonList = ExoPlayerSamples.FromJson(json);
            IList<IMediaItem> items = new List<IMediaItem>();

            foreach (var item in jsonList)
            {
                foreach (var sample in item.Samples)
                {
                    if (!string.IsNullOrEmpty(sample.Uri))
                    {
                        var mediaItem = new MediaItem(sample.Uri)
                        {
                            Title = sample.Name,
                            Album = item.Name,
                            FileExtension = sample.Extension ?? ""
                        };
                        if (mediaItem.FileExtension == "mpd" || mediaItem.MediaUri.EndsWith(".mpd"))
                            mediaItem.MediaType = MediaType.Dash;
                        else if (mediaItem.FileExtension == "ism" || mediaItem.MediaUri.EndsWith(".ism"))
                            mediaItem.MediaType = MediaType.SmoothStreaming;
                        else if (mediaItem.FileExtension == "m3u8" || mediaItem.MediaUri.EndsWith(".m3u8"))
                            mediaItem.MediaType = MediaType.Hls;

                        items.Add(mediaItem);
                    }
                }
            }

            return Task.FromResult(items);
        }

        public Task<IList<IPlaylist>> GetPlaylists()
        {
            IList<IPlaylist> playlists = new List<IPlaylist>();
            playlists.Add(new Media.Playlist() { Title = "ExoPlayer" });

            return Task.FromResult(playlists);
        }
    }
}