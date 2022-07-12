using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projection {
    public class RoomComposite : MonoBehaviour, IProjectionUser {
        public RoomProjection Projection { get; set; }

        public void Setup(RoomProjection proj) {
            throw new System.NotImplementedException();
        }
    }
}