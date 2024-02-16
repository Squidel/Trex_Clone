using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trex_Clone.Visuals
{
    public class SpriteAnimationFrame
    {
        private Sprite _sprite;
        public Sprite Sprite
        {
            get
            {
                return _sprite;
            }
            set
            {
                if (value != _sprite)
                {
                    _sprite = value;
                }
                else
                {
                    throw new ArgumentNullException("value", "The sprite cannot be null");
                }
            }
        }
        public float TimeStamp { get; }
        public SpriteAnimationFrame(Sprite sprite, float timeStamp)
        {
            Sprite = sprite;
            TimeStamp = timeStamp;
        }
    }
}
