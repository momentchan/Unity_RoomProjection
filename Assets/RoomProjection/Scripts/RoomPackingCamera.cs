using mj.gist;
using UnityEngine;
using UnityEngine.Rendering;

namespace mj.gist.projection {
        [RequireComponent(typeof(Camera))]
    public class RoomPackingCamera : MonoBehaviour, IProjectionUser {
        public RoomTexturePack Pack => pack;
        public RoomProjection Projection { get; set; }
        public Camera Cam => cam;

        [SerializeField] private RenderTexture tgt;
        [SerializeField] private BlendMode srcBlend = BlendMode.One, dstBlend = BlendMode.One;

        private Camera cam;
        private RoomTexturePack pack;

        public void Setup(RoomProjection proj) {
            cam = GetComponent<Camera>();
            cam.orthographicSize = proj.WholeHeight * 0.5f;
            cam.backgroundColor = Color.black;
            cam.aspect = proj.Aspect;
            cam.depthTextureMode = DepthTextureMode.Depth;

            (int w, int h) = RoomHelper.CalculateRosolution(proj, 0);
            tgt = RTUtil.CreateRenderTexture(w, h, 24, RenderTextureFormat.ARGBFloat);
            cam.targetTexture = tgt;

            pack = new RoomTexturePack(proj, tgt.width, tgt.height);
        }

        void OnRenderImage(RenderTexture src, RenderTexture dst) {
            pack.srcBlend = srcBlend;
            pack.dstBlend = dstBlend;
            pack.Distribute(Projection, src);
            Graphics.Blit(src, dst);
        }

        void OnDestroy() {
            cam.targetTexture = null;
            RTUtil.Destroy(tgt);
            pack.Dispose();
        }
    }
}