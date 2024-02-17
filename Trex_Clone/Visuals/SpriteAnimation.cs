using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trex_Clone.Visuals
{
    //This sprite animation class assumes that there is a dummy frame that signifies the end of the animation
    public class SpriteAnimation
    {
        private List<SpriteAnimationFrame> _frames = new List<SpriteAnimationFrame>();

        public SpriteAnimationFrame this[int index]
        {
            get
            {
                return GetFrame(index);
            }
        }
        public float Duration
        {
            get
            {
                if (!_frames.Any())
                {
                    return 0f;
                }
                else
                {
                    return _frames.Max(x => x.TimeStamp);
                }
            }
        }
        public bool IsPlaying { get; private set; }
        public bool ShouldLoop { get; set; } = true;
        public float PlaybackProgress { get; private set; }
        public SpriteAnimationFrame CurrentFrame
        {
            get
            {
                return _frames.Where(x => x.TimeStamp <= PlaybackProgress).OrderBy(x => x.TimeStamp).LastOrDefault();
            }
        }
        public void AddFrame(Sprite sprite, float timestamp)
        {
            SpriteAnimationFrame frame = new SpriteAnimationFrame(sprite, timestamp);
            _frames.Add(frame);
        }
        public void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                PlaybackProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (PlaybackProgress > Duration)
                {
                    if (ShouldLoop)
                    {
                        PlaybackProgress -= Duration;
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }
        public void Stop()
        {
            IsPlaying = false;
            PlaybackProgress = 0;
        }
        public void Play()
        {
            IsPlaying = true;
        }
        public SpriteAnimationFrame GetFrame(int frameIndex)
        {
            if (frameIndex < 0 || frameIndex >= _frames.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(frameIndex), "A frame with the index " + frameIndex + " does not exist in this animation");
            }
            return _frames[frameIndex];
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            SpriteAnimationFrame frame = CurrentFrame;
            if (frame != null)
            {
                frame.Sprite.Draw(spriteBatch, position);
            }
        }
        public void Clear()
        {
            Stop();
            _frames.Clear();
        }
    }
}
