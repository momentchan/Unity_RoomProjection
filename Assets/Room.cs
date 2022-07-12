using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projection {
    public class Room : MonoBehaviour {
        [SerializeField] private RoomData data;
        [SerializeField] private RoomProjection projection;

        void Start() {
            projection.Setup(data);
        }

        // Update is called once per frame
        void Update() {

        }
    }
}