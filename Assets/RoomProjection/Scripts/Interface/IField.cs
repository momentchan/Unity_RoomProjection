using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj.gist.projection {
    public interface IField  {
        string GetFieldShaderName();
        FieldType GetFieldType();
        RenderTexture GetFieldGPU();
        Texture2D GetFieldCPU();
    }

    public enum FieldType {
        UVW,
        Fractal,
        Wind,
        Light,
        Touch,
        UVWInversed,
        FractalInversed,
        FractalUV,
        Fbm
    }
}