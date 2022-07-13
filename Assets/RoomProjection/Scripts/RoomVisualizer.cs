using System.Collections.Generic;
using UnityEngine;

namespace mj.gist.projection {
    public class RoomVisualizer : MonoBehaviour {
        [SerializeField] private Room room;
        [SerializeField] private RoomQuad quadPrefab;

        private List<RoomQuad> quads;

        void Start() {
            var rd = room.Data;
            float w = rd.Width,
                  h = rd.Height,
                  d = rd.Depth,
                  x = rd.X,
                  y = rd.Y;

            quads = new List<RoomQuad>();
            var matrices = new Dictionary<Direction, Matrix4x4>() {
                { Direction.Front, Matrix4x4.TRS(Vector3.forward * d * 0.5f + Vector3.up * h * 0.5f, Quaternion.identity, new Vector3(w, h, 1f)) },
                { Direction.Back, Matrix4x4.TRS(Vector3.back * d*0.5f + Vector3.up * h * 0.5f, Quaternion.AngleAxis(180f, Vector3.up), new Vector3(w, h, 1f)) },
                { Direction.Right, Matrix4x4.TRS(Vector3.right * w * 0.5f + Vector3.up * h * 0.5f, Quaternion.AngleAxis(90f, Vector3.up), new Vector3(d, h, 1f)) },
                { Direction.Left, Matrix4x4.TRS(Vector3.left * w * 0.5f + Vector3.up * h * 0.5f, Quaternion.AngleAxis(-90f, Vector3.up), new Vector3(d, h, 1f)) },
                { Direction.Floor, Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(90f, Vector3.right), new Vector3(w, d, 1f)) },
            };

            ListUtil.Iterate<Direction>(dir => {
                var quad = Instantiate(quadPrefab);
                quad.transform.SetParent(transform);
                var trs = matrices[dir];
                quad.transform.localPosition = trs.GetColumn(3);
                quad.transform.localScale = trs.lossyScale;
                quad.transform.localRotation = trs.rotation;
                quad.dir = dir;
                quad.SetTexture(RoomHelper.GetContainer(room.Projection, dir));
                quads.Add(quad);
            });
        }
    }
}