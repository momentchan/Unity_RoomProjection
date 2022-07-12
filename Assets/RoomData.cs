using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projection {
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
    public interface IProjectionUser {
        RoomProjection Projection { get; set; }
        void Setup(RoomProjection proj);
    }
}