using UnityEngine;

namespace mj.gist.projection {
    public static partial class RoomHelper {
        public static (int w, int h) CalculateRosolution(RoomProjection proj, int lod) {
            var w = Mathf.FloorToInt(proj.WholeWidth * proj.Meter2PixelUnit) >> lod;
            var h = Mathf.FloorToInt(proj.WholeHeight * proj.Meter2PixelUnit) >> lod;
            return (w, h);
        }
        public static RenderTexture CreateFront(RoomProjection proj, int w, int h) => Create(GetFww(proj) * w, GetFhh(proj) * h);
        public static RenderTexture CreateRight(RoomProjection proj, int w, int h) => Create(GetFdw(proj) * w, GetFhh(proj) * h);
        public static RenderTexture CreateBack(RoomProjection proj, int w, int h) => Create(GetFww(proj) * w, GetFhh(proj) * h);
        public static RenderTexture CreateLeft(RoomProjection proj, int w, int h) => Create(GetFdw(proj) * w, GetFhh(proj) * h);
        public static RenderTexture CreateFloor(RoomProjection proj, int w, int h) => Create(proj.Width / proj.WholeWidth * w, proj.Depth / proj.WholeHeight * h);
        private static RenderTexture Create(float w, float h)
            => RTUtil.Create(Mathf.FloorToInt(w), Mathf.FloorToInt(h), 0, RenderTextureFormat.Default, true);

        private static float GetFww(RoomProjection proj) => proj.Width / proj.WholeWidth;
        private static float GetFdw(RoomProjection proj) => proj.Depth / proj.WholeWidth;
        private static float GetFhh(RoomProjection proj) => proj.Height / proj.WholeHeight;
        public static RenderTexture GetContainer(IRoomContainer container, Direction dir) {
            switch (dir) {
                case Direction.Front: return container.Front;
                case Direction.Right: return container.Right;
                case Direction.Back: return container.Back;
                case Direction.Left: return container.Left;
                case Direction.Floor: return container.Floor;
            }
            return container.Front;
        }

        public static Vector4 GetST(RoomProjection proj, Direction dir) => GetST(proj, Cast(dir));
        public static Vector4 GetST(RoomProjection proj, Area area) {
            float h = proj.Height, w = proj.Width, d = proj.Depth;
            float wh = proj.WholeHeight, ww = proj.WholeWidth;

            switch (area) {
                case Area.WallFront: return new Vector4(w / ww, h / wh, 0, 1 - h / wh);
                case Area.WallRight: return new Vector4(d / ww, h / wh, w / ww, 1 - h / wh);
                case Area.WallBack: return new Vector4(w / ww, h / wh, (w + d) / ww, 1 - h / wh);
                case Area.WallLeft: return new Vector4(d / ww, h / wh, (w + d + w) / ww, 1 - h / wh);

                case Area.FloorFront: return new Vector4(w / ww, d / wh, 0, 1 - (h + d) / wh);
                case Area.FloorRight: return new Vector4(d / ww, w / wh, w / ww, 1 - (h + w) / wh);
                case Area.FloorBack: return new Vector4(w / ww, d / wh, (w + d) / ww, 1 - (h + d) / wh);
                case Area.FloorLeft: return new Vector4(d / ww, w / wh, (w + d + w) / ww, 1 - (h + w) / wh);
            }
            return Vector4.zero;
        }
        private static Area Cast(Direction dir, int floorOffset = 0) {
            switch (dir) {
                case Direction.Front: return Area.WallFront;
                case Direction.Back: return Area.WallBack;
                case Direction.Right: return Area.WallRight;
                case Direction.Left: return Area.WallLeft;
                default: {// floor
                        switch (floorOffset) {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                return Area.FloorFront + floorOffset;
                            default: break;
                        }
                    }
                    return Area.FloorFront;
            }
        }
    }
}