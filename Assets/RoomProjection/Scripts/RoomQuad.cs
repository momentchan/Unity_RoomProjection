using UnityEngine;

namespace mj.gist.projection {
    public class RoomQuad : MonoBehaviour {
        public Direction dir;
        private Block block;

        private void OnEnable() {
            block = new Block(GetComponent<Renderer>());
        }

        public void SetTexture(Texture t) {
            block.SetTexture("_MainTex", t);
            block.Apply();
        }
    }
}