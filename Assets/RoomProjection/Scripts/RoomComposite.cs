using mj.gist;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj.gist.projection {
        public class RoomComposite : MonoBehaviour, IRoomContainer, IProjectionUser {
        public RoomProjection Projection { get; set; }
        public Camera Cam => basis.Cam;

        public RenderTexture Front => front;
        public RenderTexture Back => back;
        public RenderTexture Right => right;
        public RenderTexture Left => left;
        public RenderTexture Floor => floor;

        [Header("Texture")]
        [SerializeField] private RenderTexture front;
        [SerializeField] private RenderTexture back;
        [SerializeField] private RenderTexture right;
        [SerializeField] private RenderTexture left;
        [SerializeField] private RenderTexture floor;

        [Header("PackingCamera")]
        [SerializeField] private RoomPackingCamera basis;

        private Material mat;

        public void Setup(RoomProjection proj) {
            (int w, int h) = RoomHelper.CalculateRosolution(proj, 0);
            front = RoomHelper.CreateFront(proj, w, h);
            back = RoomHelper.CreateBack(proj, w, h);
            right = RoomHelper.CreateRight(proj, w, h);
            left = RoomHelper.CreateLeft(proj, w, h);
            floor = RoomHelper.CreateFloor(proj, w, h);
        }

        private void OnEnable() {
            mat = new Material(Shader.Find("Hidden/Composite"));
        }

        void Update() { Composite(); }

        private void Composite() {
            ListUtil.Iterate<Direction>(dir => {
                var src = RoomHelper.GetContainer(basis.Pack, dir);
                var dst = RoomHelper.GetContainer(this, dir);

                mat.SetTexture("_Basis", src);
                mat.SetVector("_ST", RoomHelper.GetST(Projection, dir));
                Graphics.Blit(null, dst, mat, 0);
            });
        }

        private void OnDestroy() {
            RTUtil.Destroy(front);
            RTUtil.Destroy(back);
            RTUtil.Destroy(right);
            RTUtil.Destroy(left);
            RTUtil.Destroy(floor);
        }
    }
}