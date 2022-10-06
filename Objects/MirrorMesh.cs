using System.Numerics;
using Veldrid.Utilities;

namespace gr.obj
{
    internal class MirrorMesh
    {
        public static Plane Plane { get; set; } = new Plane(Vector3.UnitY, 0);
    }
}
