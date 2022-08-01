using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj.gist.projection {
    public class FieldFractal : FieldBase {

        [SerializeField] protected Vector3 noiseParams0 = Vector3.one; // freq, speed, phase
        [SerializeField] protected Vector3 noiseParams1 = Vector3.one;
        [SerializeField] protected Vector3 noiseParams2 = Vector3.one;

        public override string GetFieldShaderName() => "Hidden/FieldFractal";
        public override FieldType GetFieldType() => FieldType.Fractal;

        public override void Setup(RoomProjection proj) {
            base.Setup(proj);
            fieldGPU = RTUtil.Create(resolution, Mathf.FloorToInt(resolution / proj.Aspect), 0, RenderTextureFormat.ARGBFloat);
            mat = new Material(Shader.Find(GetFieldShaderName()));
        }

        private void Update() {
            if (fieldGPU == null) return;

            mat.SetVector("_NoiseParams0", noiseParams0);
            mat.SetVector("_NoiseParams1", noiseParams1);
            mat.SetVector("_NoiseParams2", noiseParams2);
            mat.SetFloat("_Ratio", Projection.Height / Projection.WholeWidth);
            mat.SetFloat("_GlobalTime", Time.time);  // sync time if necessary
            Graphics.Blit(null, fieldGPU, mat, 0);
        }
    }
}