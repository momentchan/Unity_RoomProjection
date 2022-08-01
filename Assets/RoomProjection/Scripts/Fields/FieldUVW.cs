using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj.gist.projection {
    public class FieldUVW : FieldBase {
        public override string GetFieldShaderName() => "Hidden/FieldUVW";

        public override FieldType GetFieldType() => FieldType.UVW;

        public override void Setup(RoomProjection proj) {
            base.Setup(proj);

            fieldGPU = RTUtil.Create(resolution, Mathf.FloorToInt(resolution / proj.Aspect), 0, RenderTextureFormat.ARGBFloat);

            var tmp = RenderTexture.active;
            {
                RenderTexture.active = fieldGPU;
                GL.Clear(false, true, Color.black);

                // floor uv
                var len = Mathf.Max(proj.Width, proj.Depth); // replace if multiple rooms
                Vector3 fO = new Vector3(proj.X, proj.Y, 0) / len,
                        fL = new Vector3(proj.Width, proj.Depth, 0) / len;

                var floorUV = new Vector3[] {
                    fO,
                    fO + new Vector3(fL.x, 0, 0),
                    fO + fL,
                    fO + new Vector3(0, fL.y, 0)
                };

                // wall uv why?
                Vector3 wO = new Vector3(-proj.Height / len, -proj.Height / len, 1);
                var wallUV = new Vector3[] {
                    floorUV[0] + wO,
                    floorUV[1] + wO,
                    floorUV[2] + wO,
                    floorUV[3] + wO,
                };

                float w = proj.Width,
                      h = proj.Height,
                      d = proj.Depth;

                var fww = w / proj.WholeWidth * 2f;
                var fdw = d / proj.WholeWidth * 2f;

                var fwh = w / proj.WholeHeight * 2f;
                var fhh = h / proj.WholeHeight * 2f;
                var fdh = d / proj.WholeHeight * 2f;

                var x0 = fww * 0.5f;
                var x1 = fww + fdw * 0.5f;
                var x2 = (fww + fdw) + fww * 0.5f;
                var x3 = (fww + fdw + fww) + fdw * 0.5f;

                var y0 = -h / proj.WholeHeight;
                var y1 = -h / proj.WholeHeight * 2f;

                var corner = new Vector2(-1, 1f);
                // Walls
                DrawUVRect(corner + new Vector2(x0, y0), new Vector2(fww, fhh), new Vector3[] { wallUV[0], wallUV[1], floorUV[1], floorUV[0] });
                DrawUVRect(corner + new Vector2(x1, y0), new Vector2(fdw, fhh), new Vector3[] { wallUV[1], wallUV[2], floorUV[2], floorUV[1] });
                DrawUVRect(corner + new Vector2(x2, y0), new Vector2(fww, fhh), new Vector3[] { wallUV[2], wallUV[3], floorUV[3], floorUV[2] });
                DrawUVRect(corner + new Vector2(x3, y0), new Vector2(fdw, fhh), new Vector3[] { wallUV[3], wallUV[0], floorUV[0], floorUV[3] });

                //// Floors
                DrawUVRect(corner + new Vector2(x0, y1 - fdh * 0.5f), new Vector2(fww, fdh), floorUV);
                DrawUVRect(corner + new Vector2(x1, y1 - fwh * 0.5f), new Vector2(fdw, fwh), new Vector3[] { floorUV[1], floorUV[2], floorUV[3], floorUV[0] });
                DrawUVRect(corner + new Vector2(x2, y1 - fdh * 0.5f), new Vector2(fww, fdh), new Vector3[] { floorUV[2], floorUV[3], floorUV[0], floorUV[1] });
                DrawUVRect(corner + new Vector2(x3, y1 - fwh * 0.5f), new Vector2(fdw, fwh), new Vector3[] { floorUV[3], floorUV[0], floorUV[1], floorUV[2] });
            }
            fieldCPU = Bake(fieldGPU);
            RenderTexture.active = tmp;
        }
    }
}