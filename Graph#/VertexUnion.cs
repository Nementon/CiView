using System;
using System.Runtime.InteropServices;
using QuickGraph;

namespace GraphSharp
{
    [StructLayout(LayoutKind.Explicit)]
    public struct VertexUnion<T,E> where E : IEdge<T>
    {
        [FieldOffset(0)]
        private readonly bool isSimple;
        [FieldOffset(sizeof(bool))]
        private readonly T simple;
        [FieldOffset(sizeof(bool))] private readonly IBidirectionalGraph<T,E> compound;

        bool IsSimple { get { return isSimple; } }
        bool IsCompound { get { return !isSimple; } }
        public T GetSimple()
        {
            if (isSimple) return simple;
            else throw new System.Exception("Not a simple vertex");
        }
        public IBidirectionalGraph<T,E> GetCompound()
        {
            if (isSimple) throw new System.Exception("Not a compound vertex");
            else return compound; 
        }
        public VertexUnion(T vertex)
        {
            isSimple = true;
            compound = null;
            simple = vertex;
        }
        public VertexUnion(IBidirectionalGraph<T, E> compound_vertex)
        {
            isSimple = false;
            simple = default(T) ;
            compound = compound_vertex;
        }
    }
}
