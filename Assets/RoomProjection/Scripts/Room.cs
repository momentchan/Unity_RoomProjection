using System.Linq;
using UnityEngine;

namespace mj.gist.projection {
    public class Room : MonoBehaviour {
        public RoomData Data => data;
        public RoomProjection Projection => projection;

        [SerializeField] private RoomData data;
        [SerializeField] private RoomProjection projection;

        void Awake() {
            projection.Setup(data);

            GetComponentsInChildren<IProjectionUser>().ToList().ForEach(s => {
                s.Projection = projection;
                s.Setup(projection);
            });
        }
    }
}