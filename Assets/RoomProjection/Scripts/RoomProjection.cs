using mj.gist;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj.gist.projection {
    public class RoomProjection : MonoBehaviour, IRoomContainer {
        [SerializeField] private int referenceHeightInPixel = 1920;
        [SerializeField] private float referenceHeightInMeter = 3f;
        [SerializeField] private RoomComposite composite;

        public RenderTexture Front => composite.Front;
        public RenderTexture Back => composite.Back;
        public RenderTexture Right => composite.Right;
        public RenderTexture Left => composite.Left;
        public RenderTexture Floor => composite.Floor;

        public float X => data.X;
        public float Y => data.Y;
        public float Width => data.Width;
        public float Height => data.Height;
        public float Depth => data.Depth;
        public float WholeWidth => (Width + Depth) * 2;
        public float WholeHeight => Height + Mathf.Max(Width, Depth);
        public float Aspect => WholeWidth / WholeHeight;
        public float Meter2PixelUnit => 1f * referenceHeightInPixel / referenceHeightInMeter;


        private Camera cam => composite.Cam;
        private RoomData data;

        public void Setup(RoomData data) {
            this.data = data;
        }

        private void OnDrawGizmos() {
            if (!Application.isPlaying) return;

            var d = cam.nearClipPlane;
            var p = transform.position;
            var ux = transform.right;
            var uy = -transform.up;
            var o = p + transform.forward * d - transform.right * WholeWidth * 0.5f + transform.up * WholeHeight * 0.5f;


            Gizmos.color = Color.white;
            GizmosUtil.DrawGizmosRect(o, Width, Height);
            GizmosUtil.DrawGizmosRect(o + ux * Width, Depth, Height);
            GizmosUtil.DrawGizmosRect(o + ux * (Width + Depth), Width, Height);
            GizmosUtil.DrawGizmosRect(o + ux * (Width + Depth + Width), Depth, Height);

            Gizmos.color = Color.white;
            o += uy * Height;
            GizmosUtil.DrawGizmosRect(o, Width, Depth);
            GizmosUtil.DrawGizmosRect(o + ux * Width, Depth, Width);
            GizmosUtil.DrawGizmosRect(o + ux * (Width + Depth), Width, Depth);
            GizmosUtil.DrawGizmosRect(o + ux * (Width + Depth + Width), Depth, Width);
        }
    }
}