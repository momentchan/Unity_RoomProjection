using UnityEngine;

namespace mj.gist.projection {
    [System.Serializable]
    public class RoomData {
        const float mm2m = 0.001f;
        public float X => x * mm2m;
        public float Y => y * mm2m;
        public float Width => width * mm2m;
        public float Height => height * mm2m;
        public float Depth => depth * mm2m;

        [SerializeField] private int x, y, width, height, depth;
    }
    public enum Area { WallFront, WallRight, WallBack, WallLeft, FloorFront, FloorRight, FloorBack, FloorLeft };
    public enum Direction { Front, Right, Back, Left, Floor };

}