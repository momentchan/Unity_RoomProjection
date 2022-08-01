using System;
using UnityEngine;

namespace mj.gist.projection {
    public abstract class FieldBase : MonoBehaviour, IProjectionUser, IField {

        [SerializeField] protected int resolution = 1024;
        [SerializeField] protected RenderTexture fieldGPU;
        [SerializeField] protected Texture2D fieldCPU;

        [SerializeField] protected Material mat;
        public RoomProjection Projection { get; set; }

        protected Matrix4x4 matrix;

        #region IField
        public abstract string GetFieldShaderName();
        public abstract FieldType GetFieldType();
        public RenderTexture GetFieldGPU() => fieldGPU;
        public Texture2D GetFieldCPU() => fieldCPU;
        #endregion

        public virtual void Setup(RoomProjection proj) {
            ReleaseBuffers();
        }

        protected Texture2D Bake(RenderTexture src, TextureFormat format = TextureFormat.RGBAHalf) {
            var dst = new Texture2D(src.width, src.height, format, false);
            var tmp = RenderTexture.active;
            dst.ReadPixels(new Rect(0, 0, src.width, src.height), 0, 0);
            dst.Apply();
            RenderTexture.active = tmp;
            return dst;
        }

        private void Draw(int mode, Action draw) {
            if (mat == null) {
                mat = new Material(Shader.Find(GetFieldShaderName()));
                matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
            }
            mat.SetPass(0);

            GL.PushMatrix();
            {
                GL.LoadProjectionMatrix(matrix);
                GL.Begin(mode);
                draw();
                GL.End();
            }
            GL.PopMatrix();
        }

        protected void DrawUVRect(Vector2 center, Vector2 size, Vector3[] uv) {
            var hx = size.x * 0.5f;
            var hy = size.y * 0.5f;
            Draw(GL.QUADS, () => {
                GL.TexCoord(uv[0]); GL.Vertex(center + new Vector2(-hx,  hy));
                GL.TexCoord(uv[1]); GL.Vertex(center + new Vector2( hx,  hy));
                GL.TexCoord(uv[2]); GL.Vertex(center + new Vector2( hx, -hy));
                GL.TexCoord(uv[3]); GL.Vertex(center + new Vector2(-hx, -hy));
            });
        }

        protected virtual void ReleaseBuffers() {
            RTUtil.Destroy(fieldCPU);
            RTUtil.Destroy(fieldGPU);
        }

        private void OnDestroy() {
            Destroy(mat);
        }
    }
}