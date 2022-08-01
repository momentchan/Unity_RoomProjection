using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mj.gist.projection {
    public class Room : MonoBehaviour {
        public RoomData Data => data;
        public RoomProjection Projection => projection;

        [SerializeField] private RoomData data;
        [SerializeField] private RoomProjection projection;


        protected List<IField> fields;

        void Awake() {
            projection.Setup(data);

            GetComponentsInChildren<IProjectionUser>().ToList().ForEach(s => {
                s.Projection = projection;
                s.Setup(projection);
            });

            // setup field
            fields = GetComponentsInChildren<IField>().ToList();
            GetComponentsInChildren<IFieldUser>().ToList().ForEach(user => {
                var founds = fields.FindAll(f => user.GetFieldTypes().Contains(f.GetFieldType()));
                user.Fields = founds.ToDictionary(f => f.GetFieldType());
            });
        }
    }
}