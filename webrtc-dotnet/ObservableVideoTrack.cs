﻿using System;
using System.Reactive.Subjects;

namespace WonderMediaProductions.WebRtc
{
    public sealed class ObservableVideoTrack : VideoTrack
    {
        private readonly Subject<VideoFrameMessage> _localVideoFrameEncodedStream = new Subject<VideoFrameMessage>();

        public new ObservablePeerConnection PeerConnection => (ObservablePeerConnection) base.PeerConnection;

        public ObservableVideoTrack(ObservablePeerConnection peerConnection, Action<VideoEncoderOptions> configure) 
            : base(peerConnection, configure)
        {
        }

        public IObservable<VideoFrameMessage> LocalVideoFrameEncodedStream => _localVideoFrameEncodedStream;

        protected override void OnLocalVideoFrameEncoded(PeerConnection pc, int trackId, long frameId, IntPtr rgbaPixels)
        {
            _localVideoFrameEncodedStream.OnNext(new VideoFrameMessage(trackId, frameId, rgbaPixels));
            base.OnLocalVideoFrameEncoded(pc, trackId, frameId, rgbaPixels);
        }

        protected override void OnDispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _localVideoFrameEncodedStream.Dispose();
            }

            base.OnDispose(isDisposing);
        }
    }
}