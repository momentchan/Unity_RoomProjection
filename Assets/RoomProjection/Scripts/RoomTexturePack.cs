using mj.gist;
using UnityEngine;
using UnityEngine.Rendering;

namespace mj.gist.projection {
    public class RoomTexturePack : System.IDisposable, IRoomContainer {
        public RenderTexture Front => front;
        public RenderTexture Back => back;
        public RenderTexture Right => right;
        public RenderTexture Left => left;
        public RenderTexture Floor => floor;

        public BlendMode srcBlend = BlendMode.One, dstBlend = BlendMode.One;

        protected RenderTexture front, right, back, left, floor;
        private Material distributeMat, combineMat;

        public RoomTexturePack(RoomProjection proj, int w, int h) {
            distributeMat = new Material(Shader.Find("Hidden/Distribute"));
            combineMat = new Material(Shader.Find("Hidden/Combine"));

            front = RoomHelper.CreateFront(proj, w, h);
            back = RoomHelper.CreateBack(proj, w, h);
            right = RoomHelper.CreateRight(proj, w, h);
            left = RoomHelper.CreateLeft(proj, w, h);
            floor = RoomHelper.CreateFloor(proj, w, h);
        }

        public void Distribute(RoomProjection proj, RenderTexture src) {
            MapWall(src, front, RoomHelper.GetST(proj, Area.WallFront));
            MapWall(src, back, RoomHelper.GetST(proj, Area.WallBack));
            MapWall(src, right, RoomHelper.GetST(proj, Area.WallRight));
            MapWall(src, left, RoomHelper.GetST(proj, Area.WallLeft));

            RTUtil.Clear(floor);
            var tmp = RenderTexture.GetTemporary(src.descriptor);
            {
                MapFloor(src, tmp, RoomHelper.GetST(proj, Area.FloorFront), 0f);
                MapFloor(src, tmp, RoomHelper.GetST(proj, Area.FloorRight), Mathf.PI * 0.5f);
                MapFloor(src, tmp, RoomHelper.GetST(proj, Area.FloorBack), Mathf.PI);
                MapFloor(src, tmp, RoomHelper.GetST(proj, Area.FloorLeft), Mathf.PI * 1.5f);
            }
            RenderTexture.ReleaseTemporary(tmp);
        }

        private void MapWall(RenderTexture src, RenderTexture dst, Vector4 st) {
            distributeMat.SetTexture("_MainTex", src);
            distributeMat.SetVector("_ST", st);
            Graphics.Blit(src, dst, distributeMat, 0);
        }
        private void MapFloor(RenderTexture src, RenderTexture dst, Vector4 st, float theta = 0f) {
            distributeMat.SetTexture("_MainTex", src);
            distributeMat.SetVector("_ST", st);
            Graphics.Blit(src, dst, distributeMat, 1);
            Combine(dst, floor, theta);
        }

        private void Combine(RenderTexture src, RenderTexture dst, float theta = 0f) {
            combineMat.SetInt("_SrcBlend", (int)srcBlend);
            combineMat.SetInt("_DstBlend", (int)dstBlend);
            combineMat.SetTexture("_MainTex", src);
            combineMat.SetFloat("_Theta", theta);
            Graphics.Blit(src, dst, combineMat, 0);
        }

        public void Dispose() {
            RTUtil.Destroy(front);
            RTUtil.Destroy(back);
            RTUtil.Destroy(right);
            RTUtil.Destroy(left);
            RTUtil.Destroy(floor);
        }
    }
}