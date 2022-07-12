using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projection {
    public class RoomProjection : MonoBehaviour, IRoomContainer {
        public RenderTexture Front => throw new System.NotImplementedException();

        public RenderTexture Back => throw new System.NotImplementedException();

        public RenderTexture Right => throw new System.NotImplementedException();

        public RenderTexture Left => throw new System.NotImplementedException();

        public RenderTexture Floor => throw new System.NotImplementedException();

        private RoomData data;
        public void Setup(RoomData data) {
            this.data = data;
        }


        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}