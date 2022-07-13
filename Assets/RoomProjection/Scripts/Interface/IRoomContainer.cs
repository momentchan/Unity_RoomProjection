using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj.gist.projection {
    public interface IRoomContainer {
        RenderTexture Front { get; }
        RenderTexture Back { get; }
        RenderTexture Right { get; }
        RenderTexture Left { get; }
        RenderTexture Floor { get; }
    }
}